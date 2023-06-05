using Key2Joy.Contracts.Plugins;
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
        public string AssemblyPath { get; set; }

        public PluginLoadStates LoadState { get; set; } = PluginLoadStates.NotLoaded;
        public string LoadErrorMessage { get; set; } = null;

        public Type PluginType { get; set; } = null;
        public AbstractPlugin Plugin { get; set; } = null;

        public PluginLoadState(string assemblyPath, Type pluginType = null)
        {
            AssemblyPath = assemblyPath;
            PluginType = pluginType;
        }
    }
}
