using System.IO;
using System.Text.Json;
using Key2Joy.Contracts;
using Key2Joy.Util;

namespace Key2Joy.Config;

/// <summary>
/// Manages user configurations being loaded from and saved to disk.
/// </summary>
public class ConfigManager : IConfigManager
{
    protected const string CONFIG_PATH = "config.json";

    public bool IsInitialized { get; private set; }

    private ConfigState configState;

    public ConfigManager()
        => this.LoadOrCreate();

    /// <returns>The path to where the config file is located.</returns>
    protected virtual string GetAppDataDirectory() => Output.GetAppDataDirectory();

    public ConfigState GetConfigState()
        => this.configState;

    public void Save()
    {
        var options = GetSerializerOptions();
        var configPath = Path.Combine(
            this.GetAppDataDirectory(),
            CONFIG_PATH);

        File.WriteAllText(configPath, JsonSerializer.Serialize(this.configState, options));
    }

    /// <summary>
    /// Loads the configuration or creates a default one on disk.
    /// </summary>
    public void LoadOrCreate()
    {
        var configPath = Path.Combine(
            this.GetAppDataDirectory(),
            CONFIG_PATH);

        this.configState = new ConfigState(this);

        if (File.Exists(configPath))
        {
            var options = GetSerializerOptions();
            // Merge the loaded config state with the default config state
            JsonUtilities.PopulateObject(
                File.ReadAllText(configPath),
                this.configState,
                options
            );
        }

        var assembly = System.Reflection.Assembly.GetEntryAssembly();

        // If the assembly is null then we are running in a unit test
        if (assembly == null)
        {
            this.CompleteInitialization();
            return;
        }

        var executablePath = assembly.Location;
        if (executablePath.EndsWith("Key2Joy.exe")
            && this.configState.LastInstallPath != executablePath)
        {
            this.configState.LastInstallPath = executablePath;
        }

        this.CompleteInitialization();
        return;
    }

    private void CompleteInitialization()
    {
        this.IsInitialized = true;

        // We save so old properties are removed and new ones are added to the config file immediately
        this.Save();
    }

    protected static JsonSerializerOptions GetSerializerOptions()
    {
        JsonSerializerOptions options = new()
        {
            WriteIndented = true,
        };

        return options;
    }

    /// <summary>
    /// Turns the plugin path into a relative one from the app directory
    /// </summary>
    /// <param name="pluginAssemblyPath"></param>
    /// <returns></returns>
    protected string NormalizePluginPath(string pluginAssemblyPath)
    {
        var appDirectory = Path.GetDirectoryName(this.configState.LastInstallPath);
        var pluginPath = Path.GetFullPath(pluginAssemblyPath);
        var relativePath = pluginPath.Replace(appDirectory, string.Empty).TrimStart('\\');

        return relativePath;
    }

    /// <summary>
    /// Sets a plugin as enabled, the permissions checksum is stored so no changes to the permissions
    /// are accepted when loading the plugin later.
    ///
    /// Set permissionsChecksumOrNull to null to disable the plugin.
    /// </summary>
    /// <param name="pluginAssemblyPath"></param>
    /// <param name="permissionsChecksumOrNull"></param>
    public void SetPluginEnabled(string pluginAssemblyPath, string permissionsChecksumOrNull)
    {
        pluginAssemblyPath = this.NormalizePluginPath(pluginAssemblyPath);

        if (permissionsChecksumOrNull != null)
        {
            if (!this.configState.EnabledPlugins.ContainsKey(pluginAssemblyPath))
            {
                this.configState.EnabledPlugins.Add(pluginAssemblyPath, permissionsChecksumOrNull);
            }
            else
            {
                this.configState.EnabledPlugins[pluginAssemblyPath] = permissionsChecksumOrNull;
            }
        }
        else
        {
            if (this.configState.EnabledPlugins.ContainsKey(pluginAssemblyPath))
            {
                this.configState.EnabledPlugins.Remove(pluginAssemblyPath);
            }
        }

        this.Save();
    }

    public bool IsPluginEnabled(string pluginAssemblyPath)
        => this.configState.EnabledPlugins.ContainsKey(this.NormalizePluginPath(pluginAssemblyPath));

    public string GetExpectedPluginChecksum(string pluginAssemblyPath)
        => this.configState.EnabledPlugins.ContainsKey(this.NormalizePluginPath(pluginAssemblyPath))
            ? this.configState.EnabledPlugins[this.NormalizePluginPath(pluginAssemblyPath)]
            : null;
}
