using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Mapping
{
    internal class CombinedTriggerListener : TriggerListener
    {
        internal static CombinedTriggerListener instance;
        internal static CombinedTriggerListener Instance
        {
            get
            {
                if (instance == null)
                    instance = new CombinedTriggerListener();

                return instance;
            }
        }

        protected Dictionary<CombinedTrigger, List<MappedOption>> lookup = new Dictionary<CombinedTrigger, List<MappedOption>>();
        private Dictionary<MappedOption, CombinedTrigger> optionsToExecute;

        internal override void AddMappedOption(MappedOption mappedOption)
        {
            var trigger = mappedOption.Trigger as CombinedTrigger;
            List<MappedOption> mappedOptions;

            if (!lookup.TryGetValue(trigger, out mappedOptions))
                lookup.Add(trigger, mappedOptions = new List<MappedOption>());

            foreach (var realTrigger in trigger.Triggers)
            {
                // Disable these options from ever executing since we will do that manually in Listener_TriggerActivated
                realTrigger.Executing += (s, e) => e.Handled = true;
            }

            mappedOptions.Add(mappedOption);
        }
        
        internal override bool GetIsTriggered(BaseTrigger trigger) => false;

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
                        break;
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
            optionsToExecute = new Dictionary<MappedOption, CombinedTrigger>();

            foreach (var combinedTrigger in lookup.Keys)
            {
                bool found = false;

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
                    break;
            }

            e.MappedOptionCandidates.AddRange(optionsToExecute.Keys);
        }

        private void Listener_TriggerActivated(object sender, TriggerActivatedEventArgs e)
        {
            foreach (var mappedOption in e.MappedOptions)
            {
                // Skip every mapped option that we didn't add as candidates ourselves
                if (!optionsToExecute.TryGetValue(mappedOption, out var combinedTrigger))
                    continue;

                optionsToExecute.Remove(mappedOption);

                // Check if all triggers for this mapped option are matched. Only then Executes the actions.
                var allTriggered = true;

                foreach (var trigger in combinedTrigger.Triggers)
                {
                    if(!trigger.GetTriggerListener().GetIsTriggered(trigger))
                    { 
                        allTriggered = false;
                        break;
                    }
                }

                if (allTriggered)
                {
                    var inputBag = new CombinedInputBag()
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
