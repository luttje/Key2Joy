using System;

namespace Key2Joy.Contracts.Plugins
{
    public class PluginTrigger : MarshalByRefObject
    {
        public PluginBase Plugin { get; set; }

    }
}
