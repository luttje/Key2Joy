using System.Collections.Generic;
using System.Linq;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Triggers;

namespace Key2Joy.Mapping.Triggers.Logic
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

            if (!this.lookup.TryGetValue(trigger, out var mappedOptions))
            {
                this.lookup.Add(trigger, mappedOptions = new List<AbstractMappedOption>());
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
            foreach (var listener in this.allListeners)
            {
                var foundListener = false;

                foreach (var combinedTrigger in this.lookup.Keys)
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
                    listener.TriggerActivating += this.Listener_TriggerActivating;
                    listener.TriggerActivated += this.Listener_TriggerActivated;
                }
            }
        }

        private void Listener_TriggerActivating(object sender, TriggerActivatingEventArgs e)
        {
            this.optionsToExecute = new Dictionary<AbstractMappedOption, CombinedTrigger>();

            foreach (var combinedTrigger in this.lookup.Keys)
            {
                var found = false;

                foreach (var trigger in combinedTrigger.Triggers)
                {
                    if (e.GetIsMappedOptionCandidate(trigger))
                    {
                        foreach (var mappedOption in this.lookup[combinedTrigger])
                        {
                            this.optionsToExecute.Add(new MappedOption()
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

            foreach (var key in this.optionsToExecute.Keys)
            {
                e.MappedOptionCandidates.Add(key);
            }
        }

        private void Listener_TriggerActivated(object sender, TriggerActivatedEventArgs e)
        {
            foreach (var mappedOption in e.MappedOptions)
            {
                // Skip every mapped option that we didn't add as candidates ourselves
                if (!this.optionsToExecute.TryGetValue(mappedOption, out var combinedTrigger))
                {
                    continue;
                }

                this.optionsToExecute.Remove(mappedOption);

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
                    this.DoExecuteTrigger(
                        this.lookup[combinedTrigger],
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
