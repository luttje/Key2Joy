using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Key2Joy.Contracts.Mapping;

namespace Key2Joy.Contracts.Plugins
{
    /// <summary>
    /// Serves as the place where the actual type instances are stored (only on the host). A reference
    /// number is returned for later use.
    /// </summary>
    public static class WrappedPluginTypesStore
    {
        private static Dictionary<int, object> instances = new Dictionary<int, object>();
        private static int nextId = 0;

        public static int Store(object instance)
        {
            instances.Add(nextId, instance);
            return nextId++;
        }

        public static object Get(int id)
        {
            return instances[id];
        }

        public static T Get<T>(int id)
        {
            return (T)instances[id];
        }
    }

    /// <summary>
    /// Type to be used as a parameter to a scripting action. When invoked, the
    /// host will be the one to execute the action.
    /// </summary>
    public class WrappedPluginType : MarshalByRefObject
    {
        private int id;

        public WrappedPluginType(object action)
        {
            id = WrappedPluginTypesStore.Store(action);
        }

        public T GetInstance<T>() => WrappedPluginTypesStore.Get<T>(id);

        public void AsDelegateInvoke(params object[] args)
        {
            var action = WrappedPluginTypesStore.Get<Delegate>(id);
            action.DynamicInvoke(new object[] { args }); // Extra wrap needed for params: http://deploytonenyures.blogspot.com/2011/11/c-params-and-methodinfoinvoke.html
        }
    }
}
