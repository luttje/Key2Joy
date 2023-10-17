using System;
using System.Collections.Generic;
using System.IO;
using CommonServiceLocator;
using Key2Joy.Config;
using Key2Joy.Contracts;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Contracts.Mapping.Triggers;
using Key2Joy.Contracts.Plugins;
using Key2Joy.Mapping;
using Key2Joy.Mapping.Actions;
using Key2Joy.Mapping.Triggers;
using static System.Windows.Forms.AxHost;

namespace Key2Joy.Plugins;

[ExposesEnumeration(typeof(LowLevelInput.Mouse.MoveType))]
[ExposesEnumeration(typeof(LowLevelInput.Mouse.Buttons))]
[ExposesEnumeration(typeof(SimWinInput.GamePadControl))]
[ExposesEnumeration(typeof(LowLevelInput.PressState))]
[ExposesEnumeration(typeof(LowLevelInput.Simulator.GamePadStick))]
[ExposesEnumeration(typeof(Mapping.Actions.Logic.AppCommand))]
[ExposesEnumeration(typeof(LowLevelInput.KeyboardKey))]
public class PluginSet : IDisposable
{
    public string PluginsFolder { get; private set; }

    private readonly Dictionary<string, PluginLoadState> pluginLoadStates = new();
    public IReadOnlyDictionary<string, PluginLoadState> AllPluginLoadStates => this.pluginLoadStates;

    private readonly string[] pluginDirectoriesPaths;
    public IReadOnlyList<string> PluginAssemblyPaths => this.pluginDirectoriesPaths;
    private readonly IList<PluginHostProxy> proxiesToDispose = new List<PluginHostProxy>();

    /**
     * Plugin customizations
     */
    private readonly List<MappingTypeFactory<AbstractAction>> actionFactories = new();
    private readonly List<MappingTypeFactory<AbstractTrigger>> triggerFactories = new();
    private readonly List<MappingControlFactory> mappingControlFactories = new();
    private readonly List<ExposedEnumeration> exposedEnumerations = new();

    private readonly IConfigManager configManager;

    /// <summary>
    /// Loads plugins from the specified directory
    /// </summary>
    /// <param name="pluginDirectoriesPaths">The absolute path to the directory containing the plugins</param>
    /// <returns></returns>
    public PluginSet(string pluginDirectoriesPaths)
    {
        if (!Path.IsPathRooted(pluginDirectoriesPaths))
        {
            throw new ArgumentException("Plugin directory path must be absolute", nameof(pluginDirectoriesPaths));
        }

        this.configManager = ServiceLocator.Current.GetInstance<IConfigManager>();

        this.PluginsFolder = pluginDirectoriesPaths;

        if (!string.IsNullOrWhiteSpace(pluginDirectoriesPaths)
            && !Directory.Exists(pluginDirectoriesPaths))
        {
            Directory.CreateDirectory(pluginDirectoriesPaths);
        }

        this.pluginDirectoriesPaths = Directory.GetDirectories(pluginDirectoriesPaths);
    }

    public void LoadAll()
    {
        foreach (var pluginDirectoryPath in this.pluginDirectoriesPaths)
        {
            var pluginAssemblyName = Path.GetFileName(pluginDirectoryPath);
            var pluginAssemblyFileName = $"{pluginAssemblyName}.dll";
            var pluginAssemblyPath = Path.Combine(pluginDirectoryPath, pluginAssemblyFileName);
            var expectedChecksum = this.configManager.GetExpectedPluginChecksum(pluginAssemblyPath);

            if (this.configManager.IsPluginEnabled(pluginAssemblyPath))
            {
                var plugin = this.LoadPlugin(pluginAssemblyPath, expectedChecksum);

                if (plugin != null)
                {
                    this.AddPluginState(PluginLoadStates.Loaded, pluginAssemblyPath, null, plugin);
                }
            }
            else
            {
                this.AddPluginState(
                    PluginLoadStates.NotLoaded,
                    pluginAssemblyPath,
                    "Plugin disabled. Enable it if you trust the author."
                );
            }
        }
    }

    public void RefreshPluginTypes()
    {
        ActionsRepository.Buffer(this.actionFactories);
        TriggersRepository.Buffer(this.triggerFactories);
        MappingControlRepository.Buffer(this.mappingControlFactories);
        ExposedEnumerationRepository.Buffer(this.exposedEnumerations);
    }

    public PluginHostProxy LoadPlugin(string pluginAssemblyPath, string expectedChecksum = null)
    {
        var pluginAssemblyName = Path.GetFileName(pluginAssemblyPath).Replace(".dll", "");
        PluginHostProxy pluginHost = new(pluginAssemblyPath, pluginAssemblyName);
        string loadedChecksum;

        try
        {
            pluginHost.LoadPlugin(out loadedChecksum, expectedChecksum);

            this.actionFactories.AddRange(pluginHost.GetActionFactories());
            this.triggerFactories.AddRange(pluginHost.GetTriggerFactories());
            this.mappingControlFactories.AddRange(pluginHost.GetMappingControlFactories());
            this.exposedEnumerations.AddRange(pluginHost.GetExposedEnumerations());
        }
        catch (PluginLoadException ex)
        {
            Output.WriteLine(ex);
            pluginHost.Dispose();

            this.AddPluginState(
                PluginLoadStates.FailedToLoad,
                pluginAssemblyPath,
                ex.Message
            );

            return null;
        }

        this.AddPluginState(PluginLoadStates.Loaded, pluginAssemblyPath, null, pluginHost);

        this.configManager.SetPluginEnabled(pluginAssemblyPath, loadedChecksum);

        this.proxiesToDispose.Add(pluginHost);

        return pluginHost;
    }

    /// <summary>
    /// Disables the plugin for next load. Note that this doesnt unload resources already
    /// started by the plugin
    /// TODO: Fully unload plugin
    /// </summary>
    /// <param name="pluginAssemblyPath"></param>
    public void DisablePlugin(string pluginAssemblyPath)
        => this.configManager.SetPluginEnabled(pluginAssemblyPath, null);

    internal void AddPluginState(PluginLoadStates state, string pluginAssemblyPath, string errorMessage, PluginHostProxy loadedPlugin = null)
    {
        if (this.pluginLoadStates.TryGetValue(pluginAssemblyPath, out var loadState))
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

        this.pluginLoadStates.Add(
            pluginAssemblyPath,
            loadState
        );
    }

    public void Dispose()
    {
        foreach (var keyValuePair in this.pluginLoadStates)
        {
            var pluginLoadState = keyValuePair.Value;
            pluginLoadState.PluginHost?.Dispose();
        }
    }
}
