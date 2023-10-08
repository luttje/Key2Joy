using Key2Joy.Contracts.Mapping;
using System;

namespace Key2Joy.Contracts.Plugins
{
    public class PluginAction : MarshalByRefObject
    {
        public PluginBase Plugin { get; set; }

        public virtual MappingAspectOptions BuildSaveOptions(MappingAspectOptions options) => options;
        public virtual void LoadOptions(MappingAspectOptions options) { }

        public virtual string GetNameDisplay(string nameFormat)
        {
            return nameFormat;

        }

        internal object InvokeScriptMethod(string methodName, object[] parameters)
        {
            var method = this.GetType().GetMethod(methodName);
            return method.Invoke(this, parameters);
        }
    }
}
