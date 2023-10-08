using Key2Joy.Contracts.Plugins;
using Key2Joy.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Plugins
{
    public class PluginActionProxy : CoreAction, IGetRealObject<PluginAction>
    {
        private PluginAction source;
        
        public PluginActionProxy(string fullTypeName, PluginAction source)
            : base(fullTypeName)
        {
            this.source = source;
        }

        public PluginAction GetRealObject()
        {
            return source;
        }
    }
}
