using System;

namespace Key2Joy.Contracts.Mapping.Triggers
{
    public class TriggerChangedEventArgs : EventArgs
    {
        public static new readonly TriggerChangedEventArgs Empty = new();

        public AbstractTrigger Trigger { get; private set; }

        public TriggerChangedEventArgs(AbstractTrigger trigger = null) => this.Trigger = trigger;
    }
}
