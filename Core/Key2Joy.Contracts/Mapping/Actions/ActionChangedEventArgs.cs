using System;

namespace Key2Joy.Contracts.Mapping.Actions;

public class ActionChangedEventArgs : EventArgs
{
    public static new readonly ActionChangedEventArgs Empty = new();

    public AbstractAction Action { get; private set; }

    public ActionChangedEventArgs(AbstractAction action = null) => this.Action = action;
}
