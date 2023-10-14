using System.Collections.Generic;
using System.IO;
using System.Linq;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Mapping.Actions;
using Key2Joy.Plugins;

namespace Key2Joy.Tests.PluginHost;

public class PluginHost
{
    protected PluginHostProxy pluginHost;
    protected IReadOnlyList<MappingTypeFactory<AbstractAction>> pluginActionFactories;
    protected IReadOnlyList<MappingTypeFactory<AbstractAction>> pluginTriggerFactories;

    public virtual void Setup()
    {
        var pluginAssemblyPath = typeof(Stubs.TestPlugin.Plugin).Assembly.Location;
        var pluginAssemblyName = Path.GetFileName(pluginAssemblyPath).Replace(".dll", "");

        this.pluginHost = new(pluginAssemblyPath, pluginAssemblyName);

        this.pluginHost.LoadPlugin(out var _);
        this.pluginActionFactories = this.pluginHost.GetActionFactories();
        this.pluginTriggerFactories = this.pluginHost.GetActionFactories();
    }

    protected PluginActionProxy MakePluginAction<T>()
    {
        var pluginActionFactory = this.pluginActionFactories.First(a => a.FullTypeName == typeof(T).FullName);
        var action = CoreAction.MakeAction(pluginActionFactory);

        return action as PluginActionProxy;
    }

    public virtual void Cleanup() => this.pluginHost.Dispose();
}
