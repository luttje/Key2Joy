using System;
using System.Collections.Generic;
using Key2Joy.Contracts.Mapping;

namespace Key2Joy.Plugins;

/// <summary>
/// Creates instances of types, simply using Activator.CreateInstance
/// </summary>
public class MappingTypeFactory
{
    public string FullTypeName { get; private set; }

    public MappingAttribute Attribute { get; private set; }
    public IEnumerable<ExposedMethod> ExposedMethods { get; private set; }

    public MappingTypeFactory(string fullTypeName, MappingAttribute attribute, IEnumerable<ExposedMethod> exposedMethods = null)
    {
        this.FullTypeName = fullTypeName;
        this.Attribute = attribute;
        this.ExposedMethods = exposedMethods ?? new List<ExposedMethod>();
    }

    public virtual T CreateInstance<T>(object[] constructorArguments) where T : AbstractMappingAspect => (T)Activator.CreateInstance(this.ToType(), constructorArguments);

    public virtual Type ToType() => Type.GetType(this.FullTypeName);
}

/// <summary>
/// Creates instances of types, simply using Activator.CreateInstance
/// </summary>
public class MappingTypeFactory<T> : MappingTypeFactory where T : AbstractMappingAspect
{
    public MappingTypeFactory(string fullTypeName, MappingAttribute attribute, IEnumerable<ExposedMethod> exposedMethods = null)
        : base(fullTypeName, attribute, exposedMethods)
    {
    }

    public virtual T CreateInstance(object[] constructorArguments) => base.CreateInstance<T>(constructorArguments);
}

/// <summary>
/// Creates the type by commanding the PluginHostProxy to create it.
/// </summary>
public class PluginMappingTypeFactory<T> : MappingTypeFactory<T> where T : AbstractMappingAspect
{
    private readonly PluginHostProxy pluginHost;
    private readonly Type hostBaseType;

    public PluginMappingTypeFactory(PluginHostProxy pluginHost, Type hostBaseType, string fullTypeName, MappingAttribute attribute, IReadOnlyList<ExposedMethod> exposedMethods = null)
        : base(fullTypeName, attribute, exposedMethods)
    {
        this.pluginHost = pluginHost;
        this.hostBaseType = hostBaseType;
    }

    public override T CreateInstance(object[] constructorArguments) => this.CreateInstance<T>(constructorArguments);

    public override T CreateInstance<T>(object[] constructorArguments) => this.pluginHost.CreateAspectInstance<T>(this.FullTypeName, constructorArguments);

    /// <summary>
    /// Since we can't get the Type in the other appdomain, we return the host/contract class it derives from instead.
    /// </summary>
    /// <returns></returns>
    public override Type ToType() => this.hostBaseType;
}
