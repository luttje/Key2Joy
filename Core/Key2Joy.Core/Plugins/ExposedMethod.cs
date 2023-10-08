using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Util;

namespace Key2Joy.Plugins;

public abstract class ExposedMethod
{
    public string FunctionName { get; protected set; }
    public string MethodName { get; protected set; }

    public ExposedMethod(string functionName, string methodName)
    {
        this.FunctionName = functionName;
        this.MethodName = methodName;
    }

    public abstract Delegate CreateDelegate(AbstractAction instance);
}

public class TypeExposedMethod : ExposedMethod
{
    public Type Type { get; protected set; }

    public TypeExposedMethod(string functionName, string methodName, Type type)
        : base(functionName, methodName) => this.Type = type;

    public override Delegate CreateDelegate(AbstractAction instance)
    {
        var method = this.Type.GetMethod(this.MethodName);

        return method.CreateDelegate(instance);
    }
}

public class PluginExposedMethod : ExposedMethod
{
    public string TypeName { get; protected set; }

    private readonly PluginHostProxy pluginHost;
    private readonly Dictionary<Type, Func<object, object>> parameterTransformers = new();
    private PluginActionProxy currentInstance;

    public PluginExposedMethod(PluginHostProxy pluginHost, string typeName, string functionName, string methodName)
        : base(functionName, methodName)
    {
        this.pluginHost = pluginHost;
        this.TypeName = typeName;
    }

    public override Delegate CreateDelegate(AbstractAction instance)
    {
        var methodInfo = this.GetExecutorMethodInfo((PluginActionProxy)instance);
        var parameters = methodInfo.GetParameters();
        var parameterTypes = parameters.Select(p => p.ParameterType).ToArray();
        var delegateType = ExpressionUtil.GetDelegateType(parameterTypes, methodInfo.ReturnType);
        var executor = methodInfo.CreateDelegate(delegateType, this);

        return executor;
    }

    public void RegisterParameterTransformer<T>(Func<T, object> transformer)
    {
        var key = typeof(T);

        if (this.parameterTransformers.ContainsKey(key))
        {
            this.parameterTransformers.Remove(key);
        }

        this.parameterTransformers.Add(key, o => transformer((T)o));
    }

    public object TransformAndRedirect(params object[] parameters)
    {
        // Check if any of the parameters are not serializable/MarshalByRefObject and need to be wrapped.
        var transformedParameters = parameters.Select(p =>
        {
            if (this.parameterTransformers.TryGetValue(p.GetType(), out var transformer))
            {
                return transformer(p);
            }

            if (p is MarshalByRefObject or ISerializable)
            {
                return p;
            }

            if (p.GetType().IsSerializable)
            {
                return p;
            }

            throw new NotImplementedException("Parameter type not supported to cross AppDomain boundary: " + p.GetType().FullName);
        }).ToArray();

        return this.currentInstance.InvokeScriptMethod(this.MethodName, transformedParameters);
    }

    /// <summary>
    /// MethodInfo that can be bound to scripts
    /// </summary>
    /// <returns></returns>
    public MethodInfo GetExecutorMethodInfo(PluginActionProxy instance)
    {
        this.currentInstance = instance;
        return typeof(PluginExposedMethod).GetMethod(nameof(TransformAndRedirect));
    }
}
