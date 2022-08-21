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
    internal abstract class PressReleaseTriggerListener<TTrigger> : TriggerListener 
        where TTrigger : class, IPressState, IReturnInputHash
    {
        protected Dictionary<int, List<MappedOption>> lookupDown;
        protected Dictionary<int, List<MappedOption>> lookupRelease;

        protected PressReleaseTriggerListener()
        {
            lookupDown = new Dictionary<int, List<MappedOption>>();
            lookupRelease = new Dictionary<int, List<MappedOption>>();
        }

        internal override void AddMappedOption(MappedOption mappedOption)
        {
            var trigger = mappedOption.Trigger as TTrigger;
            var dictionary = (Dictionary<int, List<MappedOption>>)null;

            if (trigger.PressState == PressState.Press)
                dictionary = lookupDown;

            if (trigger.PressState == PressState.Release)
                dictionary = lookupRelease;

            if (dictionary == null)
                return;

            if (!dictionary.TryGetValue(trigger.GetInputHash(), out var mappedOptions))
                dictionary.Add(trigger.GetInputHash(), mappedOptions = new List<MappedOption>());

            mappedOptions.Add(mappedOption);
        }
    }
}
