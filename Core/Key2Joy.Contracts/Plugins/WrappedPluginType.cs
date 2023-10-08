using System;

namespace Key2Joy.Contracts.Plugins
{
    /// <summary>
    /// Type to be used as a parameter to a scripting action. When invoked, the
    /// host will be the one to execute the action.
    /// </summary>
    public class WrappedPluginType : MarshalByRefObject
    {
        private readonly object instance;

        public WrappedPluginType(object instance)
        {
            this.instance = instance;
        }

        public T GetInstance<T>() => (T)this.instance;

        public void AsDelegateInvoke(params object[] args)
        {
            var action = this.GetInstance<Delegate>();
            action.DynamicInvoke(new object[] { args }); // Extra wrap needed for params: http://deploytonenyures.blogspot.com/2011/11/c-params-and-methodinfoinvoke.html
        }
    }
}
