using Key2Joy.Config;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Plugins;
using Key2Joy.Mapping;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Key2Joy.Plugins
{
    public class PluginSet
    {
        private readonly List<PluginBase> loadedPlugins = new();
        public IReadOnlyList<PluginBase> LoadedPlugins => loadedPlugins;

        private readonly Dictionary<string, PluginLoadState> pluginLoadStates = new();
        public IReadOnlyDictionary<string, PluginLoadState> AllPluginLoadStates => pluginLoadStates;

        private readonly string[] pluginDirectoriesPaths;
        public IReadOnlyList<string> PluginAssemblyPaths => pluginDirectoriesPaths;
        private IList<PluginHostProxy> proxiesToDispose = new List<PluginHostProxy>();

        /**
         * Plugin customizations
         */
        private readonly List<MappingTypeFactory<AbstractAction>> actionFactories = new();
        private readonly List<MappingTypeFactory<AbstractTrigger>> triggerFactories = new();
        private readonly List<MappingControlFactory> mappingControlFactories = new();

        public string PluginsFolder { get; private set; }

        /// <summary>
        /// Loads plugins from the specified directory
        /// </summary>
        /// <param name="pluginDirectoriesPaths">The path to the directory containing the plugins</param>
        /// <returns></returns>
        internal PluginSet(string pluginDirectoriesPaths)
        {
            PluginsFolder = pluginDirectoriesPaths;
            this.pluginDirectoriesPaths = Directory.GetDirectories(pluginDirectoriesPaths);
        }

        internal void LoadAll()
        {
            var enabledPlugins = ConfigManager.Config.EnabledPlugins;

            foreach (var pluginDirectoryPath in pluginDirectoriesPaths)
            {
                var pluginAssemblyName = Path.GetFileName(pluginDirectoryPath);
                var pluginAssemblyFileName = $"{pluginAssemblyName}.dll";
                var pluginAssemblyPath = Path.Combine(pluginDirectoryPath, pluginAssemblyFileName);
                var expectedChecksum = enabledPlugins.ContainsKey(pluginAssemblyPath) ? enabledPlugins[pluginAssemblyPath] : null;

                try
                {
                    var plugin = LoadPlugin(pluginAssemblyPath, expectedChecksum);
                    AddPluginState(PluginLoadStates.Loaded, pluginAssemblyPath, null, plugin);
                }
                catch (PluginLoadException ex)
                {
                    AddPluginState(
                        PluginLoadStates.NotLoaded,
                        pluginAssemblyPath,
                        "Plugin disabled. Enable it if you trust the author."
                    );
                }
            }

            ActionsRepository.Buffer(actionFactories);
            TriggersRepository.Buffer(triggerFactories);
            MappingControlRepository.Buffer(mappingControlFactories);
        }

        public PluginHostProxy LoadPlugin(string pluginAssemblyPath, string expectedChecksum = null)
        {
            var pluginDirectoryPath = Path.GetDirectoryName(pluginAssemblyPath);
            var pluginAssemblyName = Path.GetFileName(pluginAssemblyPath).Replace(".dll", "");
            var pluginLoadState = new PluginLoadState(pluginAssemblyPath);

            var pluginHost = new PluginHostProxy(pluginAssemblyPath, pluginAssemblyName);
            string loadedChecksum;

            try
            {
                pluginHost.LoadPlugin(out loadedChecksum, expectedChecksum);

                actionFactories.AddRange(pluginHost.GetActionFactories());
                triggerFactories.AddRange(pluginHost.GetTriggerFactories());
                mappingControlFactories.AddRange(pluginHost.GetMappingControlFactories());
            }
            catch (PluginLoadException ex)
            {
                pluginHost.Dispose();

                AddPluginState(
                    PluginLoadStates.FailedToLoad,
                    pluginAssemblyPath,
                    ex.Message
                );

                return null;
            }

            AddPluginState(PluginLoadStates.Loaded, pluginAssemblyPath, null, pluginHost);

            ConfigManager.Instance.SetPluginEnabled(pluginAssemblyPath, loadedChecksum);

            pluginHost.Disposing += PluginHost_Disposing;
            proxiesToDispose.Add(pluginHost);

            return pluginHost;
        }

        public void DisablePlugin(string pluginAssemblyPath)
        {
            ConfigManager.Instance.SetPluginEnabled(pluginAssemblyPath, null);
            MessageBox.Show("When disabling loaded plugins you have to restart the application for these changes to take effect.");
        }

        internal void AddPluginState(PluginLoadStates state, string pluginAssemblyPath, string errorMessage, PluginHostProxy loadedPlugin = null)
        {
            if (state == PluginLoadStates.FailedToLoad)
            {
                MessageBox.Show(
                    $"One of your plugins located at {pluginAssemblyPath} failed to load. This was the error: " +
                    errorMessage,
                    "Failed to load plugin!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }

            if (pluginLoadStates.TryGetValue(pluginAssemblyPath, out var loadState))
            {
                loadState.LoadState = state;
                loadState.SetPluginHost(loadedPlugin);
                loadState.LoadErrorMessage = errorMessage ?? string.Empty;
                return;
            }

            loadState = new PluginLoadState(pluginAssemblyPath)
            {
                LoadState = state,
                LoadErrorMessage = errorMessage
            };
            loadState.SetPluginHost(loadedPlugin);

            pluginLoadStates.Add(
                pluginAssemblyPath,
                loadState
            );
        }

        private void PluginHost_Disposing(object sender, EventArgs e)
        {
            //if (this.Disposing || this.IsDisposed)
            //    return;

            //var pluginHost = (PluginHostProxy)sender;

            //if (proxiesToDispose.Contains(pluginHost))
            //    proxiesToDispose.Remove(pluginHost);

            //this.Invoke((MethodInvoker)delegate
            //{
            //    if (this.Controls.Contains(tbcPlugins))
            //        this.Controls.Remove(tbcPlugins);

            //    // This wont work, since the tabpage being removed causes a WndProc which triggers other dipsosed plugin controls to get called (which causes an error)
            //    //if (tbcPlugins.TabPages.Contains(tabPage))
            //    //    tbcPlugins.TabPages.Remove(tabPage);

            //    // Trigger a rebuild for the tab control
            //    LoadAllPlugins();

            //    MessageBox.Show(this, $"A plugin unloaded unexpectedly, it may have crashed or been forcefully shut down. All plugins have been reloaded.", "Plugin unloaded unexpectedly!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //});
        }
    }
}
