using Key2Joy.Contracts.Mapping;
using Key2Joy.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Key2Joy.Plugins
{
    public abstract class ExposedMethod
    {
        public string FunctionName { get; protected set; }
        public string MethodName { get; protected set; }

        public ExposedMethod(string functionName, string methodName)
        {
            FunctionName = functionName;
            MethodName = methodName;
        }

        public abstract Delegate CreateDelegate(AbstractAction instance);
    }

    public class TypeExposedMethod : ExposedMethod
    {
        public Type Type { get; protected set; }

        public TypeExposedMethod(string functionName, string methodName, Type type) 
            : base(functionName, methodName)
        {
            Type = type;
        }

        public override Delegate CreateDelegate(AbstractAction instance)
        {
            var method = Type.GetMethod(MethodName);

            return method.CreateDelegate(instance);
        }
    }

    public class PluginExposedMethod : ExposedMethod
    {
        public string TypeName { get; protected set; }
        
        private PluginHostProxy pluginHost;
        private Dictionary<Type, Func<object, object>> parameterTransformers = new();
        private PluginActionProxy currentInstance;
        
        public PluginExposedMethod(PluginHostProxy pluginHost, string typeName, string functionName, string methodName)
            : base(functionName, methodName)
        {
            this.pluginHost = pluginHost;
            TypeName = typeName;
        }

        public override Delegate CreateDelegate(AbstractAction instance)
        {
            var methodInfo = GetExecutorMethodInfo((PluginActionProxy)instance);
            var parameters = methodInfo.GetParameters();
            var parameterTypes = parameters.Select(p => p.ParameterType).ToArray();
            var delegateType = ExpressionUtil.GetDelegateType(parameterTypes, methodInfo.ReturnType);
            var executor = methodInfo.CreateDelegate(delegateType, this);

            return executor;
        }

        public void RegisterParameterTransformer<T>(Func<T, object> transformer)
        {
            parameterTransformers.Add(typeof(T), o =>
            {
                return transformer((T)o);
            });
        }

        public object TransformAndRedirect(params object[] parameters)
        {
            // Check if any of the parameters are not serializable/MarshalByRefObject and need to be wrapped.
            var transformedParameters = parameters.Select(p =>
            {
                if (parameterTransformers.TryGetValue(p.GetType(), out var transformer))
                {
                    return transformer(p);
                }

                if (p is MarshalByRefObject || p is ISerializable)
                {
                    return p;
                }

                if (p.GetType().IsSerializable)
                {
                    return p;
                }

                throw new NotImplementedException("Parameter type not supported to cross AppDomain boundary: " + p.GetType().FullName);
            }).ToArray();

            return this.currentInstance.InvokeScriptMethod(MethodName, transformedParameters);
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
}
