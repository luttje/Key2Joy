using Key2Joy.Contracts.Mapping;
using Key2Joy.LowLevelInput;
using System.Collections.Generic;

namespace Key2Joy.Mapping
{
    public abstract class PressReleaseTriggerListener<TTrigger> : CoreTriggerListener
        where TTrigger : class, IPressState, IReturnInputHash
    {
        protected Dictionary<int, List<AbstractMappedOption>> lookupDown;
        protected Dictionary<int, List<AbstractMappedOption>> lookupRelease;

        protected PressReleaseTriggerListener()
        {
            lookupDown = new Dictionary<int, List<AbstractMappedOption>>();
            lookupRelease = new Dictionary<int, List<AbstractMappedOption>>();
        }

        public override void AddMappedOption(AbstractMappedOption mappedOption)
        {
            var trigger = mappedOption.Trigger as TTrigger;
            var dictionary = (Dictionary<int, List<AbstractMappedOption>>)null;

            if (trigger.PressState == PressState.Press)
                dictionary = lookupDown;

            if (trigger.PressState == PressState.Release)
                dictionary = lookupRelease;

            if (dictionary == null)
                return;

            if (!dictionary.TryGetValue(trigger.GetInputHash(), out var mappedOptions))
                dictionary.Add(trigger.GetInputHash(), mappedOptions = new List<AbstractMappedOption>());

            mappedOptions.Add(mappedOption);
        }
    }
}
