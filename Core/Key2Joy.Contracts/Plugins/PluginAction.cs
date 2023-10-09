using System;
using Key2Joy.Contracts.Mapping;

namespace Key2Joy.Contracts.Plugins;

public class PluginAction : MarshalByRefObject
{
    public PluginBase Plugin { get; set; }

    public virtual MappingAspectOptions BuildSaveOptions(MappingAspectOptions options) => options;
    public virtual void LoadOptions(MappingAspectOptions options) { }

    public virtual string GetNameDisplay(string nameFormat) => nameFormat;

    internal object InvokeScriptMethod(string methodName, object[] parameters)
    {
        var method = this.GetType().GetMethod(methodName);
        return method.Invoke(this, parameters);
    }

    internal object GetPublicPropertyValue(string propertyName)
    {
        var property = this.GetType().GetProperty(propertyName);
        return property.GetValue(this);
    }
}
