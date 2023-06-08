using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Contracts.Mapping
{
    public class TriggerChangedEventArgs : EventArgs
    {
        public static readonly TriggerChangedEventArgs Empty = new TriggerChangedEventArgs();

        public AbstractTrigger Trigger { get; private set; }

        public TriggerChangedEventArgs(AbstractTrigger trigger = null)
        {
            Trigger = trigger;
        }
    }
}
