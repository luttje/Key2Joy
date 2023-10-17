using System.Collections.Generic;
using System.IO;
using System.Linq;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Mapping.Actions;
using Key2Joy.Mapping.Actions.Scripting;
using Key2Joy.Plugins;

namespace Key2Joy.Tests.PluginHost;

public class PluginHostTestBase
{
    protected PluginHostProxy pluginHost;
    protected IReadOnlyList<MappingTypeFactory<AbstractAction>> pluginActionFactories;
    protected IReadOnlyList<MappingTypeFactory<AbstractAction>> pluginTriggerFactories;
    protected IReadOnlyList<MappingControlFactory> mappingControlFactories;
    protected IReadOnlyList<ExposedEnumeration> exposedEnumerations;

    public virtual void Setup()
    {
        var pluginAssemblyPath = typeof(Stubs.TestPlugin.Plugin).Assembly.Location;
        var pluginAssemblyName = Path.GetFileName(pluginAssemblyPath).Replace(".dll", "");

        this.pluginHost = new(pluginAssemblyPath, pluginAssemblyName);

        this.pluginHost.LoadPlugin(out var _);
        this.pluginActionFactories = this.pluginHost.GetActionFactories();
        this.pluginTriggerFactories = this.pluginHost.GetActionFactories();
        this.mappingControlFactories = this.pluginHost.GetMappingControlFactories();
        this.exposedEnumerations = this.pluginHost.GetExposedEnumerations();
    }

    protected PluginActionProxy MakePluginAction<T>()
    {
        var pluginActionFactory = this.pluginActionFactories.First(a => a.FullTypeName == typeof(T).FullName);
        var action = CoreAction.MakeAction(pluginActionFactory);

        return action as PluginActionProxy;
    }

    protected IEnumerable<ExposedMethod> GetExposedMethodsForAction<T>()
    {
        var pluginActionFactory = this.pluginActionFactories.First(a => a.FullTypeName == typeof(T).FullName);

        return pluginActionFactory.ExposedMethods;
    }

    public virtual void Cleanup() => this.pluginHost.Dispose();
}
