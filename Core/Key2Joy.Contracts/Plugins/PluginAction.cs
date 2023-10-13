using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Triggers;

namespace Key2Joy.Contracts.Plugins;

public class PluginAction : MarshalByRefObject
{
    public PluginBase Plugin { get; set; }

    public virtual MappingAspectOptions BuildSaveOptions(MappingAspectOptions options) => options;

    public virtual void LoadOptions(MappingAspectOptions options)
    { }

    public virtual string GetNameDisplay(string nameFormat) => nameFormat;

    /// <summary>
    /// Returns a list of types for the parameters of the specified method.
    ///
    /// The types returned must be simple types that can be marshalled across
    /// </summary>
    /// <param name="methodName"></param>
    /// <returns></returns>
    internal IList<Type> GetMethodParameterTypes(string methodName)
    {
        var method = this.GetType().GetMethod(methodName);
        var parameters = method.GetParameters();
        //var types = new List<Type>();
        //foreach (var parameter in parameters)
        //{
        //    var type = parameter.ParameterType;
        //    if (type.IsPrimitive || type == typeof(string))
        //    {
        //        types.Add(type);
        //    }
        //    else
        //    {
        //        throw new NotSupportedException($"Parameter type {type} is not supported");
        //    }
        //}
        //return types;
        return parameters.Select(p => p.ParameterType).ToList();
    }

    /// <summary>
    /// Invokes the specified method on the plugin.
    /// </summary>
    /// <param name="methodName"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    internal object InvokeScriptMethod(string methodName, object[] parameters)
    {
        var method = this.GetType().GetMethod(methodName);
        return method.Invoke(this, parameters);
    }

    /// <summary>
    /// Executes the plugin action.
    /// </summary>
    /// <param name="inputBag"></param>
    /// <returns></returns>
    public virtual async Task Execute(AbstractInputBag inputBag = null)
    { }

    /// <summary>
    /// Returns the value of the specified public property.
    /// </summary>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    internal object GetPublicPropertyValue(string propertyName)
    {
        var property = this.GetType().GetProperty(propertyName);
        return property.GetValue(this);
    }
}
