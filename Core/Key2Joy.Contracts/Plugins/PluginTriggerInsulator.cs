using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Contracts.Plugins
{
    public class PluginTriggerInsulator : MarshalByRefObject
    {
        private PluginTrigger source;

        public PluginTriggerInsulator(PluginTrigger source)
        {
            this.source = source;
        }

        public PluginTrigger GetPluginTrigger
        {
            get
            {
                return source;
            }
        }
    }
}
