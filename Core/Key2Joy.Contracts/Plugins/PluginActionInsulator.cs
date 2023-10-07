using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
    }
}
