using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Mapping
{
    public class TriggerActivatedEventArgs : EventArgs
    {
        public List<MappedOption> MappedOptions { get; private set; }
        public TriggerListener Listener { get; private set; }
        public IInputBag InputBag { get; private set; }

        public TriggerActivatedEventArgs(TriggerListener listener, IInputBag inputBag, List<MappedOption> mappedOptions)
        {
            Listener = listener;
            MappedOptions = mappedOptions;
            InputBag = inputBag;
        }
    }
}
