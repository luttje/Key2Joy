using System;
using System.Collections.Generic;

namespace Key2Joy.Contracts.Mapping.Triggers
{
    public class TriggerActivatingEventArgs : EventArgs
    {
        public IList<AbstractMappedOption> MappedOptionCandidates { get; private set; }
        public AbstractTriggerListener Listener { get; private set; }
        public AbstractInputBag InputBag { get; private set; }

        private readonly Func<AbstractTrigger, bool> optionCandidateFilter;

        public TriggerActivatingEventArgs(
            AbstractTriggerListener listener,
            AbstractInputBag inputBag,
            IList<AbstractMappedOption> mappedOptions,
            Func<AbstractTrigger, bool> optionCandidateFilter = null)
        {
            this.Listener = listener;
            this.MappedOptionCandidates = mappedOptions;
            this.InputBag = inputBag;
            this.optionCandidateFilter = optionCandidateFilter;
        }

        public bool GetIsMappedOptionCandidate(AbstractTrigger trigger)
        {
            if (trigger?.GetTriggerListener() != this.Listener)
            {
                return false;
            }

            if (this.optionCandidateFilter != null)
            {
                return this.optionCandidateFilter.Invoke(trigger);
            }

            return true;
        }
    }
}
