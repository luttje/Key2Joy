using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Plugins
{
    public enum PluginLoadStates
    {
        NotLoaded,
        Loaded,
        FailedToLoad
    }
    
    public class PluginLoadState
    {
        public Assembly Assembly { get; set; }

        public PluginLoadStates LoadState { get; set; } = PluginLoadStates.NotLoaded;
        public string LoadErrorMessage { get; set; } = null;

        public Type PluginType { get; set; } = null;
        public Plugin Plugin { get; set; } = null;

        public PluginLoadState(Assembly assembly, Type pluginType = null)
        {
            Assembly = assembly;
            PluginType = pluginType;
        }
    }
}
