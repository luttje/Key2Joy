using System;
using System.Collections.Generic;

namespace Key2Joy.Contracts.Mapping.Triggers
{
    public class TriggerActivatedEventArgs : EventArgs
    {
        public IList<AbstractMappedOption> MappedOptions { get; private set; }
        public AbstractTriggerListener Listener { get; private set; }
        public AbstractInputBag InputBag { get; private set; }

        public TriggerActivatedEventArgs(AbstractTriggerListener listener, AbstractInputBag inputBag, IList<AbstractMappedOption> mappedOptions)
        {
            this.Listener = listener;
            this.MappedOptions = mappedOptions;
            this.InputBag = inputBag;
        }
    }
}
