using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Key2Joy.Contracts.Mapping.Actions
{
    /// <summary>
    /// Serves as a communication proxy across the AppDomain boundary. 
    /// Without this we would not be able to use GetType, since the type is not always 
    /// available in both Host and Client. Usually only client (plugin).
    /// </summary>
    public class ScriptProxy : MarshalByRefObject
    {
        private AbstractAction instance;
        private string methodName;

        /// <param name="instance"></param>
        /// <param name="methodName"></param>
        public ScriptProxy(AbstractAction instance, string methodName)
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

        /// <summary>
        /// Creates a proxy that scripts can call
        /// </summary>
        /// <seealso cref="ScriptProxy"/>
        /// <param name="otherDomain"></param>
        /// <param name="instance"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public static ScriptProxy Create(AppDomain otherDomain, AbstractAction instance, string methodName)
        {
            Debug.WriteLine(AppDomain.CurrentDomain.FriendlyName);
            var proxyAssemblyName = typeof(ScriptProxy).Assembly.FullName;
            var proxyTypeName = typeof(ScriptProxy).FullName;
            return (ScriptProxy)otherDomain.CreateInstanceAndUnwrap(
                proxyAssemblyName,
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
        
        /// <summary>
        /// MethodInfo that can be bound to scripts, proxyexecute will forward calls to the created proxy.
        /// </summary>
        /// <returns></returns>
        public MethodInfo GetExecutorMethodInfo()
        {
            return typeof(ScriptProxy).GetMethod(nameof(ProxyExecute));
        }
    }
}
