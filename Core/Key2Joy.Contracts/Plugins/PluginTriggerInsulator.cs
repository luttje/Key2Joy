using System;

namespace Key2Joy.Contracts.Plugins;

public class PluginTriggerInsulator : MarshalByRefObject
{
    public PluginTriggerInsulator(PluginTrigger source) => this.PluginTrigger = source;

    public PluginTrigger PluginTrigger { get; }
}
