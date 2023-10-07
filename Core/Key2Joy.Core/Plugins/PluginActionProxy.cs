using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Plugins;
using Key2Joy.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Key2Joy.Plugins
{
    public class PluginActionProxy : CoreAction, IGetRealObject<PluginAction>
    {
        private PluginActionInsulator source;
        
        public PluginActionProxy(string fullTypeName, PluginActionInsulator source)
            : base(fullTypeName)
        {
            this.source = source;
        }

        public PluginAction GetRealObject()
        {
            return source.GetPluginAction;
        }

        public override MappingAspectOptions SaveOptions()
        {
            var options = base.SaveOptions();

            options = source.BuildSaveOptions(options);

            return options;
        }
        
        public override void LoadOptions(MappingAspectOptions options)
        {
            base.LoadOptions(options);

            source.LoadOptions(options);
        }
    }
}
