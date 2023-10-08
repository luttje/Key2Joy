using Key2Joy.Contracts.Mapping;
using System;

namespace Key2Joy.Contracts.Plugins
{
    public class PluginActionInsulator : MarshalByRefObject
    {
        private PluginAction source;

        public PluginActionInsulator(PluginAction source)
        {
            this.source = source;
        }

        public PluginAction GetPluginAction
        {
            get
            {
                return source;
            }
        }

        public MappingAspectOptions BuildSaveOptions(MappingAspectOptions options) => source.BuildSaveOptions(options);
        public void LoadOptions(MappingAspectOptions options) => source.LoadOptions(options);
        public string GetNameDisplay(string name) => source.GetNameDisplay(name);

        public object InvokeScriptMethod(string methodName, object[] parameters) => source.InvokeScriptMethod(methodName, parameters);
    }
}
