using System;
using Key2Joy.Contracts.Mapping.Triggers;
using Key2Joy.Contracts.Plugins;
using Key2Joy.Mapping.Triggers;

namespace Key2Joy.Plugins;

public class PluginTriggerProxy : CoreTrigger, IGetRealObject<PluginTrigger>
{
    private readonly PluginTriggerInsulator source;

    public PluginTriggerProxy(string name, PluginTriggerInsulator source)
        : base(name) => this.source = source;

    public PluginTrigger GetRealObject() => this.source.PluginTrigger;

    public override AbstractTriggerListener GetTriggerListener() => throw new NotImplementedException();

    public override string GetUniqueKey() => throw new NotImplementedException();
}
