using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Plugins;
using Key2Joy.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Plugins
{
    public class PluginTriggerProxy : CoreTrigger, IGetRealObject<PluginTrigger>
    {
        private PluginTrigger source;
        
        public PluginTriggerProxy(string fullTypeName, PluginTrigger source)
            : base(fullTypeName)
        {
            this.source = source;
        }

        public PluginTrigger GetRealObject()
        {
            return source;
        }
        
        public override AbstractTriggerListener GetTriggerListener()
        {
            throw new NotImplementedException();
        }

        public override string GetUniqueKey()
        {
            throw new NotImplementedException();
        }
    }
}
