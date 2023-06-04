using Key2Joy.Mapping;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Plugins
{
    public class PluginSet
    {
        private readonly List<Plugin> loadedPlugins = new();
        public IReadOnlyList<Plugin> LoadedPlugins => loadedPlugins;

        private readonly List<PluginLoadState> pluginLoadStates = new();
        public IReadOnlyList<PluginLoadState> AllPluginLoadStates => pluginLoadStates;

        private readonly string[] pluginAssemblyPaths;
        public IReadOnlyList<string> PluginAssemblyPaths => pluginAssemblyPaths;

        private readonly List<Type> actionTypes = new();
        public IReadOnlyList<Type> ActionTypes => actionTypes;

        private readonly List<Type> triggerTypes = new();
        public IReadOnlyList<Type> TriggerTypes => triggerTypes;

        public string Folder { get; private set; }

        /// <summary>
        /// Loads plugins from the specified directory
        /// </summary>
        /// <param name="pluginsPath">The path to the directory containing the plugins</param>
        /// <returns></returns>
        internal PluginSet(string pluginsDirectoryPath)
        {
            Folder = pluginsDirectoryPath;
            pluginAssemblyPaths = Directory.GetFiles(pluginsDirectoryPath, "*.dll");

            Start();
        }

        private void Start()
        {
            // Preload assemblies
            foreach (Assembly assembly in GetQualifyingPluginAssemblies())
            {
                try
                {
                    foreach (Type type in assembly.GetTypes())
                    {
                        if (!typeof(Plugin).IsAssignableFrom(type))
                        {
                            continue;
                        }

                        QueueLoad(assembly, type);

                        // Only look for a single type to implement IPlugin.
                        break;
                    }
                }
                catch (Exception e)
                {
                    AddFailedLoad(assembly, e.Message);
                }
            }

            // Load the plugins from the assemblies
            LoadQueued();
        }

        internal List<Assembly> GetQualifyingPluginAssemblies()
        {
            var allAssemblies = new List<Assembly>();

            foreach (var dll in pluginAssemblyPaths)
            {
                allAssemblies.Add(Assembly.LoadFile(dll));
            }

            return allAssemblies;
        }

        internal void QueueLoad(Assembly assembly, Type pluginType)
        {
            pluginLoadStates.Add(new PluginLoadState(assembly, pluginType));
        }
        
        internal void AddFailedLoad(Assembly assembly, string errorMessage)
        {
            pluginLoadStates.Add(new PluginLoadState(assembly)
            {
                LoadState = PluginLoadStates.FailedToLoad,
                LoadErrorMessage = errorMessage
            });
        }

        internal void LoadQueued()
        {
            foreach (var pluginLoadState in pluginLoadStates)
            {
                if (pluginLoadState.LoadState == PluginLoadStates.FailedToLoad)
                {
                    continue;
                } else if (pluginLoadState.LoadState == PluginLoadStates.Loaded)
                {
                    continue;
                }

                try
                {
                    var plugin = (Plugin)Activator.CreateInstance(pluginLoadState.PluginType);

                    var assembly = pluginLoadState.Assembly;

                    foreach (var type in assembly.GetTypes())
                    {
                        if (type.IsAbstract)
                        {
                            continue;
                        }

                        if (type.IsSubclassOf(typeof(BaseAction)))
                        {
                            actionTypes.Add(type);
                            plugin.AddActionType(type);
                        }
                        else if (type.IsSubclassOf(typeof(BaseTrigger)))
                        {
                            triggerTypes.Add(type);
                            plugin.AddTriggerType(type);
                        }
                    }

                    pluginLoadState.Plugin = plugin;
                    pluginLoadState.LoadState = PluginLoadStates.Loaded;

                    loadedPlugins.Add(plugin);
                }
                catch (Exception e)
                {
                    pluginLoadState.LoadErrorMessage = e.Message;
                    pluginLoadState.LoadState = PluginLoadStates.FailedToLoad;
                }
            }
        }
    }
}
