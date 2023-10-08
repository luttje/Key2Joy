using System;

namespace Key2Joy.Contracts.Mapping
{
    public class TriggerChangedEventArgs : EventArgs
    {
        public static new readonly TriggerChangedEventArgs Empty = new TriggerChangedEventArgs();

        public AbstractTrigger Trigger { get; private set; }

        public TriggerChangedEventArgs(AbstractTrigger trigger = null)
        {
            Trigger = trigger;
        }
    }
}
