using Key2Joy.Contracts.Mapping;
using System.Collections.Generic;
using System.Linq;

namespace Key2Joy.Mapping
{
    public class CombinedTriggerListener : CoreTriggerListener
    {
        public static CombinedTriggerListener instance;
        public static CombinedTriggerListener Instance
        {
            get
            {
                instance ??= new CombinedTriggerListener();

                return instance;
            }
        }

        protected IDictionary<CombinedTrigger, IList<AbstractMappedOption>> lookup = new Dictionary<CombinedTrigger, IList<AbstractMappedOption>>();
        private IDictionary<AbstractMappedOption, CombinedTrigger> optionsToExecute;

        public override void AddMappedOption(AbstractMappedOption mappedOption)
        {
            var trigger = mappedOption.Trigger as CombinedTrigger;

            if (!lookup.TryGetValue(trigger, out var mappedOptions))
            {
                lookup.Add(trigger, mappedOptions = new List<AbstractMappedOption>());
            }

            foreach (var realTrigger in trigger.Triggers)
            {
                // Disable these options from ever executing since we will do that manually in Listener_TriggerActivated
                realTrigger.Executing += (s, e) => e.Handled = true;
            }

            mappedOptions.Add(mappedOption);
        }

        public override bool GetIsTriggered(AbstractTrigger trigger) => false;

        protected override void Start()
        {
            base.Start();

            // Only listen to listeners that we have triggers for
            foreach (var listener in allListeners)
            {
                var foundListener = false;

                foreach (var combinedTrigger in lookup.Keys)
                {
                    foreach (var trigger in combinedTrigger.Triggers)
                    {
                        if (trigger.GetTriggerListener() == listener)
                        {
                            foundListener = true;
                            break;
                        }
                    }

                    if (foundListener)
                    {
                        break;
                    }
                }

                if (foundListener)
                {
                    listener.TriggerActivating += Listener_TriggerActivating;
                    listener.TriggerActivated += Listener_TriggerActivated;
                }
            }
        }

        private void Listener_TriggerActivating(object sender, TriggerActivatingEventArgs e)
        {
            optionsToExecute = new Dictionary<AbstractMappedOption, CombinedTrigger>();

            foreach (var combinedTrigger in lookup.Keys)
            {
                var found = false;

                foreach (var trigger in combinedTrigger.Triggers)
                {
                    if (e.GetIsMappedOptionCandidate(trigger))
                    {
                        foreach (var mappedOption in lookup[combinedTrigger])
                        {
                            optionsToExecute.Add(new MappedOption()
                            {
                                Trigger = trigger,
                                Action = mappedOption.Action
                            }, combinedTrigger);
                        }
                        found = true;
                        break;
                    }
                }

                if (found)
                {
                    break;
                }
            }

            foreach (var key in optionsToExecute.Keys)
            {
                e.MappedOptionCandidates.Add(key);
            }
        }

        private void Listener_TriggerActivated(object sender, TriggerActivatedEventArgs e)
        {
            foreach (var mappedOption in e.MappedOptions)
            {
                // Skip every mapped option that we didn't add as candidates ourselves
                if (!optionsToExecute.TryGetValue(mappedOption, out var combinedTrigger))
                {
                    continue;
                }

                optionsToExecute.Remove(mappedOption);

                // Check if all triggers for this mapped option are matched. Only then Executes the actions.
                var allTriggered = true;

                foreach (var trigger in combinedTrigger.Triggers)
                {
                    if (!trigger.GetTriggerListener().GetIsTriggered(trigger))
                    {
                        allTriggered = false;
                        break;
                    }
                }

                if (allTriggered)
                {
                    CombinedInputBag inputBag = new()
                    {
                        InputBags = combinedTrigger.Triggers.Select(t => t.LastInputBag).ToList()
                    };

                    // TODO: Test: If we don't provide a filter, will we get an infinite loop for events that try to add options to CombinedTriggerListener?
                    DoExecuteTrigger(
                        lookup[combinedTrigger],
                        inputBag);

                    break;
                }
            }
        }

        protected override void Stop()
        {
            instance = null;
            base.Stop();
        }
    }
}
