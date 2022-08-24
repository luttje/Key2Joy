using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Mapping
{
    public class TriggerActivatingEventArgs : EventArgs
    {
        public List<MappedOption> MappedOptionCandidates { get; private set; }
        public TriggerListener Listener { get; private set; }
        public IInputBag InputBag { get; private set; }

        private Func<BaseTrigger, bool> optionCandidateFilter;

        public TriggerActivatingEventArgs(
            TriggerListener listener, 
            IInputBag inputBag, 
            List<MappedOption> mappedOptions, 
            Func<BaseTrigger, bool> optionCandidateFilter = null)
        {
            Listener = listener;
            MappedOptionCandidates = mappedOptions;
            InputBag = inputBag;
            this.optionCandidateFilter = optionCandidateFilter;
        }

        internal bool GetIsMappedOptionCandidate(BaseTrigger trigger)
        {
            if (trigger?.GetTriggerListener() != Listener)
                return false;

            if(optionCandidateFilter != null)
                return optionCandidateFilter.Invoke(trigger);

            return true;
        }
    }
}
