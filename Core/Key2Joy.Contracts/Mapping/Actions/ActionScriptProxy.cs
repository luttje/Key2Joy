using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Key2Joy.Contracts.Mapping
{
    /// <summary>
    /// Serves as a communication proxy across the AppDomain boundary. 
    /// Without this we would not be able to use GetType, since the type is not always 
    /// available in both Host and Client. Usually only client (plugin).
    /// </summary>
    public class ActionScriptProxy : MarshalByRefObject
    {
        private AbstractAction instance;
        private string methodName;

        /// <param name="instance"></param>
        /// <param name="methodName"></param>
        public ActionScriptProxy(AbstractAction instance, string methodName)
        {
            this.instance = instance;
            this.methodName = methodName;
        }

        /// <summary>
        /// Forwards the parameters to the correct instance and method
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public object ProxyExecute(params object[] parameters)
        {
            var type = instance.GetType();
            var types = parameters.Select(p => p.GetType()).ToArray();
            var method = type.GetMethod(methodName, types);
            
            return method.Invoke(instance, parameters);
        }
    }

    /// <summary>
    /// Hosts the ActionScriptProxy. This host is the one to be called and it will transform the parameters before forwarding them.
    /// </summary>
    public class ActionScriptProxyHost
    {
        private ActionScriptProxy proxy;
        private Dictionary<Type, Func<object, object>> parameterTransformers = new();

        /// <summary>
        /// Creates a proxy that scripts can call through this host.
        /// </summary>
        /// <param name="otherDomain"></param>
        /// <param name="assemblyPath"></param>
        /// <param name="instance"></param>
        /// <param name="methodName"></param>
        /// <seealso cref="ActionScriptProxy"/>
        public ActionScriptProxyHost(AppDomain otherDomain, string assemblyPath, AbstractAction instance, string methodName)
        {
            Debug.WriteLine(AppDomain.CurrentDomain.FriendlyName);

            var proxyTypeAssembly = typeof(ActionScriptProxy).Assembly.Location;
            var proxyTypeName = typeof(ActionScriptProxy).FullName;

            // Use the assemblyPath of the plugin to find the proxy type assembly next to it
            var proxyTypeSearchPath = Path.Combine(Path.GetDirectoryName(assemblyPath), Path.GetFileName(proxyTypeAssembly));
            
            proxy = (ActionScriptProxy)otherDomain.CreateInstanceFromAndUnwrap(
                proxyTypeSearchPath,
                proxyTypeName,
                false,
                BindingFlags.Default,
                null,
                // Constructor parameters that provide the instance (already a remote proxy) and method.
                new object[]
                {
                    instance,
                    methodName,
                },
                null,
                null);
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

                throw new NotImplementedException("Parameter type not supported to cross AppDomain boundary: " + p.GetType().FullName);
            }).ToArray();

            return proxy.ProxyExecute(transformedParameters);
        }
        
        /// <summary>
        /// MethodInfo that can be bound to scripts, proxyexecute will forward calls to the created proxy.
        /// </summary>
        /// <returns></returns>
        public MethodInfo GetExecutorMethodInfo()
        {
            return typeof(ActionScriptProxyHost).GetMethod(nameof(TransformAndRedirect));
        }

    }
}
