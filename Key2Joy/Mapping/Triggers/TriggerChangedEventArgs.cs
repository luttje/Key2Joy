using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Mapping
{
    public class TriggerChangedEventArgs : EventArgs
    {
        public static readonly TriggerChangedEventArgs Empty = new TriggerChangedEventArgs();

        public BaseTrigger Trigger { get; private set; }

        public TriggerChangedEventArgs(BaseTrigger trigger = null)
        {
            Trigger = trigger;
        }
    }
}
