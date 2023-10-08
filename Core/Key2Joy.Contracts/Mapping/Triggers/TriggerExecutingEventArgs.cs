using System;

namespace Key2Joy.Contracts.Mapping.Triggers;

public class TriggerExecutingEventArgs : EventArgs
{
    public bool Handled { get; set; }

    public TriggerExecutingEventArgs()
    {
    }
}
