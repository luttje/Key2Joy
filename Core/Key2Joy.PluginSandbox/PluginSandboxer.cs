using Key2Joy.Contracts.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.PluginSandbox
{
    public class PluginSandboxer : MarshalByRefObject
    {
        public AbstractPlugin LoadPlugin(string pluginAssembly, string pluginType, string pluginDataDirectory)
        {
            var pluginWrapped = Activator.CreateInstance(
                pluginAssembly,
                pluginType
            );
            var plugin = pluginWrapped.Unwrap() as AbstractPlugin;
            plugin.PluginDataDirectory = pluginDataDirectory;
            
            plugin.Load();

            return plugin;
        }
    }
}
