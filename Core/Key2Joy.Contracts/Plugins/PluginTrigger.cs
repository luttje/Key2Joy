using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Contracts.Plugins
{
    public class PluginTrigger : MarshalByRefObject
    {
        public PluginBase Plugin { get; set; }

    }
}
