using System;
using System.Collections.Generic;

namespace Key2Joy.Contracts.Mapping
{
    public class TriggerActivatingEventArgs : EventArgs
    {
        public IList<AbstractMappedOption> MappedOptionCandidates { get; private set; }
        public AbstractTriggerListener Listener { get; private set; }
        public AbstractInputBag InputBag { get; private set; }

        private Func<AbstractTrigger, bool> optionCandidateFilter;

        public TriggerActivatingEventArgs(
            AbstractTriggerListener listener,
            AbstractInputBag inputBag,
            IList<AbstractMappedOption> mappedOptions,
            Func<AbstractTrigger, bool> optionCandidateFilter = null)
        {
            Listener = listener;
            MappedOptionCandidates = mappedOptions;
            InputBag = inputBag;
            this.optionCandidateFilter = optionCandidateFilter;
        }

        public bool GetIsMappedOptionCandidate(AbstractTrigger trigger)
        {
            if (trigger?.GetTriggerListener() != Listener)
                return false;

            if (optionCandidateFilter != null)
                return optionCandidateFilter.Invoke(trigger);

            return true;
        }
    }
}
