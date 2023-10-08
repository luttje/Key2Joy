using System;
using Key2Joy.Contracts.Mapping;

namespace Key2Joy.Contracts.Plugins
{
    public class PluginActionInsulator : MarshalByRefObject
    {
        private readonly PluginAction source;

        public PluginActionInsulator(PluginAction source)
        {
            this.source = source;
        }

        public PluginAction GetPluginAction
        {
            get
            {
                return this.source;
            }
        }

        public MappingAspectOptions BuildSaveOptions(MappingAspectOptions options) => this.source.BuildSaveOptions(options);
        public void LoadOptions(MappingAspectOptions options) => this.source.LoadOptions(options);
        public string GetNameDisplay(string name) => this.source.GetNameDisplay(name);

        public object InvokeScriptMethod(string methodName, object[] parameters) => this.source.InvokeScriptMethod(methodName, parameters);
    }
}
