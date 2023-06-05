using Key2Joy.Contracts.Mapping;
using Key2Joy.LowLevelInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
