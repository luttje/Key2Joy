using System;

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
