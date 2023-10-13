using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Key2Joy.Contracts;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Triggers;
using Key2Joy.Contracts.Plugins;
using Key2Joy.Mapping.Actions;

namespace Key2Joy.Plugins;

public class PluginActionProxy : CoreAction, IGetRealObject<PluginAction>
{
    private readonly PluginActionInsulator source;

    public PluginActionProxy(string name, PluginActionInsulator source)
        : base(name) => this.source = source;

    public PluginAction GetRealObject() => this.source.PluginAction;

    public override MappingAspectOptions SaveOptions()
    {
        var options = base.SaveOptions();

        options = this.source.BuildSaveOptions(options);

        return options;
    }

    public override void LoadOptions(MappingAspectOptions options)
    {
        base.LoadOptions(options);

        this.source.LoadOptions(options);
    }

    public override string GetNameDisplay() => this.source.GetNameDisplay(this.Name);

    internal IList<Type> GetMethodParameterTypes(string methodName)
        => this.source.GetMethodParameterTypes(methodName);

    public object InvokeScriptMethod(string methodName, object[] parameters)
    {
        try
        {
            return this.source.InvokeScriptMethod(methodName, parameters);
        }
        catch (Exception ex)
        {
            var exception = ex.InnerException ?? ex;
            Output.WriteLine(exception.ToString());
            throw exception;
        }
    }

    public override Task Execute(AbstractInputBag inputBag = null)
    {
        try
        {
            return this.source.Execute(inputBag);
        }
        catch (Exception ex)
        {
            var exception = ex.InnerException ?? ex;
            Output.WriteLine(exception.ToString());
            throw exception;
        }
    }

    public object GetPublicPropertyValue(string propertyName)
    {
        try
        {
            return this.source.GetPublicPropertyValue(propertyName);
        }
        catch (Exception ex)
        {
            var exception = ex.InnerException ?? ex;
            Output.WriteLine(exception.ToString());
            throw exception;
        }
    }
}
