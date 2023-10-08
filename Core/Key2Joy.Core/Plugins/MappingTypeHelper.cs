using System;
using System.Collections.Generic;
using System.Runtime.Remoting;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Plugins;

namespace Key2Joy.Plugins;

public class MappingTypeHelper
{
    /// <summary>
    /// Ensures the typename is valid, splitting the long variant into its short form
    /// </summary>
    /// <param name="typeInfoTypeName"></param>
    /// <returns></returns>
    public static string EnsureSimpleTypeName(string typeInfoTypeName) => typeInfoTypeName.Split(',')[0];

    /// <summary>
    /// Gets the typename, even if the object is a proxy
    /// </summary>
    /// <param name="typeFactories"></param>
    /// <param name="instance"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static string GetTypeFullName<T>(IDictionary<string, MappingTypeFactory<T>> typeFactories, AbstractMappingAspect instance)
        where T : AbstractMappingAspect
    {
        var realObject = GetRealObject(instance);
        var realTypeName = GetRealTypeName(realObject);

        if (!typeFactories.ContainsKey(realTypeName))
        {
            throw new ArgumentException("Only allowed types may be (de)serialized");
        }

        return realTypeName;
    }

    /// <summary>
    /// Gets the typename, even if the object is a proxy
    /// </summary>
    /// <param name="typeFactories"></param>
    /// <param name="instance"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static string GetTypeFullName(IDictionary<string, MappingTypeFactory> typeFactories, AbstractMappingAspect instance)
    {
        var realObject = GetRealObject(instance);
        var realTypeName = GetRealTypeName(realObject);

        if (!typeFactories.ContainsKey(realTypeName))
        {
            throw new ArgumentException("Only allowed types may be (de)serialized");
        }

        return realTypeName;
    }

    private static string GetRealTypeName(MarshalByRefObject realObject)
    {
        if (RemotingServices.IsTransparentProxy(realObject))
        {
            var objRef = RemotingServices.GetObjRefForProxy(realObject);
            return EnsureSimpleTypeName(objRef.TypeInfo.TypeName);
        }

        return realObject.GetType().FullName;
    }

    private static MarshalByRefObject GetRealObject(AbstractMappingAspect instance)
    {
        if (instance is IGetRealObject<PluginAction> pluginAction)
        {
            return pluginAction.GetRealObject();
        }
        else if (instance is IGetRealObject<PluginTrigger> pluginTrigger)
        {
            return pluginTrigger.GetRealObject();
        }

        return instance;
    }
}
