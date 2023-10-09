using System;
using Key2Joy.Contracts.Mapping;

namespace Key2Joy.Contracts.Plugins;

public class PluginActionInsulator : MarshalByRefObject
{
    public PluginActionInsulator(PluginAction source) => this.PluginAction = source;

    public PluginAction PluginAction { get; }

    public MappingAspectOptions BuildSaveOptions(MappingAspectOptions options) => this.PluginAction.BuildSaveOptions(options);
    public void LoadOptions(MappingAspectOptions options) => this.PluginAction.LoadOptions(options);
    public string GetNameDisplay(string name) => this.PluginAction.GetNameDisplay(name);

    public object InvokeScriptMethod(string methodName, object[] parameters) => this.PluginAction.InvokeScriptMethod(methodName, parameters);
    public object GetPublicPropertyValue(string propertyName) => this.PluginAction.GetPublicPropertyValue(propertyName);
}
