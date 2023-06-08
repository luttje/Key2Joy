using Key2Joy.Contracts.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Contracts.Mapping
{
    public class TriggerActivatedEventArgs : EventArgs
    {
        public IList<AbstractMappedOption> MappedOptions { get; private set; }
        public AbstractTriggerListener Listener { get; private set; }
        public AbstractInputBag InputBag { get; private set; }

        public TriggerActivatedEventArgs(AbstractTriggerListener listener, AbstractInputBag inputBag, IList<AbstractMappedOption> mappedOptions)
        {
            Listener = listener;
            MappedOptions = mappedOptions;
            InputBag = inputBag;
        }
    }
}
