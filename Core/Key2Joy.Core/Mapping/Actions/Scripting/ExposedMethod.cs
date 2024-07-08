using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Key2Joy.Contracts.Util;
using Key2Joy.Plugins;

namespace Key2Joy.Mapping.Actions.Scripting;

public delegate object ParameterTransformerDelegate(object parameter, Type methodParameterType);

public delegate object ParameterTransformerDelegate<T>(T parameter, Type methodParameterType);

public abstract class ExposedMethod
{
    public string FunctionName { get; protected set; }
    public string MethodName { get; protected set; }
    public bool IsPrepared => this.Instance != null;

    protected object Instance { get; private set; }

    private readonly Dictionary<Type, ParameterTransformerDelegate> parameterTransformers = new();
    protected IList<Type> ParameterTypes { get; private set; } = new List<Type>();
    protected IList<object> ParameterDefaultValues { get; private set; } = new List<object>();
    protected bool IsLastParameterParams { get; private set; } = false;

    public ExposedMethod(string functionName, string methodName)
    {
        this.FunctionName = functionName;
        this.MethodName = methodName;
    }

    public void Prepare(object instance)
    {
        this.Instance = instance;
        this.ParameterTypes = this.GetParameterTypes(out var parameterDefaultValues, out var isLastParameterParams);
        this.ParameterDefaultValues = parameterDefaultValues;
        this.IsLastParameterParams = isLastParameterParams;
    }

    public abstract IList<Type> GetParameterTypes(out IList<object> parameterDefaultValues, out bool isLastParameterParams);

    public abstract object InvokeMethod(object[] transformedParameters);

    /// <summary>
    /// Register a transformer for certain types coming from scripts.
    /// The transformer will get the parameter value and the type of the method parameter.
    /// The transformer must return an object that will be passed to the method.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="transformer"></param>
    public void RegisterParameterTransformer<T>(ParameterTransformerDelegate<T> transformer)
    {
        if (!this.IsPrepared)
        {
            throw new InvalidOperationException("Cannot register parameter transformer before preparing the method. Call .Prepare on the exposed method first.");
        }

        var key = typeof(T);

        if (this.parameterTransformers.ContainsKey(key))
        {
            this.parameterTransformers.Remove(key);
        }

        this.parameterTransformers.Add(key, (p, t) => transformer((T)p, t));
    }

    /// <summary>
    /// Will try to transform the parameter to the type of the method parameter.
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public object TransformAndRedirect(params object[] parameters)
    {
        if (!this.IsPrepared)
        {
            throw new InvalidOperationException("Cannot TransformAndRedirect before preparing the method. Call .Prepare on the exposed method first.");
        }

        // When we pass TransformAndRedirect any array as the only parameter, C# treats that
        // array as the parameters array. Whilst we likely want to send through the array as is.
        // To workaround this we'll check that if we get more parameters than we expect, we'll
        // wrap it in an object[]
        if (this.ParameterTypes.Count == 1
            && parameters.Length > this.ParameterTypes.Count)
        {
            parameters = new object[] { parameters };
        }

        // If there's more arguments than parameters, and the last parameter is params,
        // we'll wrap the rest of the arguments in an object[] and pass that through.
        if (this.IsLastParameterParams)
        {
            var surplusParameters = parameters.Skip(this.ParameterTypes.Count - 1).ToArray();
            var parametersToPass = parameters.Take(this.ParameterTypes.Count - 1).ToList();
            parametersToPass.Add(surplusParameters);
            parameters = parametersToPass.ToArray();
        }

        var transformedParameters = parameters
            .Select((parameter, parameterIndex) =>
            {
                var parameterType = this.ParameterTypes[parameterIndex];

                if (this.parameterTransformers.TryGetValue(parameter.GetType(), out var transformer))
                {
                    parameter = transformer(parameter, parameterType);
                }

                return TypeConverter.ConvertToType(parameter, parameterType);
            })
            .ToList();

        // Ensure the transformedParameters list has the same number of items as this.ParameterTypes
        while (transformedParameters.Count < this.ParameterTypes.Count)
        {
            transformedParameters.Add(this.GetDefaultParameterValue(transformedParameters.Count));
        }

        return this.InvokeMethod(transformedParameters.ToArray());
    }

    /// <summary>
    /// Method to get default parameter value or create an empty array for params
    /// </summary>
    /// <param name="parameterIndex"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    private object GetDefaultParameterValue(int parameterIndex)
    {
        if (this.IsLastParameterParams
            && parameterIndex == this.ParameterTypes.Count - 1)
        {
            var lastParameterType = this.ParameterTypes.Last();
            var elementType = lastParameterType.GetElementType();
            return Array.CreateInstance(elementType ?? typeof(object), 0);
        }
        else if (parameterIndex < this.ParameterDefaultValues.Count
            && this.ParameterDefaultValues[parameterIndex] != DBNull.Value)
        {
            return this.ParameterDefaultValues[parameterIndex];
        }

        throw new ArgumentException("No default value available for parameter at index " + parameterIndex);
    }

    /// <summary>
    /// MethodInfo that can be bound to scripts
    /// </summary>
    /// <returns></returns>
    public virtual MethodInfo GetExecutorMethodInfo() => typeof(ExposedMethod).GetMethod(nameof(TransformAndRedirect));
}

public class TypeExposedMethod : ExposedMethod
{
    public Type Type { get; protected set; }

    private readonly MethodInfo cachedMethodInfo;

    public TypeExposedMethod(string functionName, string methodName, Type type)
        : base(functionName, methodName)
    {
        this.Type = type;
        this.cachedMethodInfo = this.Type.GetMethod(this.MethodName);
    }

    public override IList<Type> GetParameterTypes(out IList<object> parameterDefaultValues, out bool isLastParameterParams)
    {
        var parameterInfos = this.cachedMethodInfo.GetParameters();

        isLastParameterParams = parameterInfos.Length > 0
            && parameterInfos.Last().IsDefined(typeof(ParamArrayAttribute), false);

        parameterDefaultValues = parameterInfos.Select(p => p.DefaultValue).ToList();

        return parameterInfos.Select(p => p.ParameterType).ToList();
    }

    public override object InvokeMethod(object[] transformedParameters)
        => this.cachedMethodInfo.Invoke(this.Instance, transformedParameters);
}

public class PluginExposedMethod : ExposedMethod
{
    public string TypeName { get; protected set; }

    public PluginExposedMethod(string typeName, string functionName, string methodName)
        : base(functionName, methodName)
        => this.TypeName = typeName;

    public override IList<Type> GetParameterTypes(out IList<object> parameterDefaultValues, out bool isLastParameterParams)
    {
        var instance = (PluginActionProxy)this.Instance;
        return instance.GetMethodParameterTypes(this.MethodName, out parameterDefaultValues, out isLastParameterParams);
    }

    public override object InvokeMethod(object[] transformedParameters)
    {
        var instance = (PluginActionProxy)this.Instance;
        return instance.InvokeScriptMethod(this.MethodName, transformedParameters);
    }
}
