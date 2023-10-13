using System.IO;
using System.Text.Json;
using Key2Joy.Contracts;

namespace Key2Joy.Config;

public class ConfigManager
{
    private const string CONFIG_PATH = "config.json";

    private static ConfigManager instance;

    internal static ConfigManager Instance
    {
        get
        {
            instance ??= LoadOrCreate();

            return instance;
        }
    }

    public static ConfigState Config => Instance.configState;

    internal bool IsInitialized { get; private set; }
    private ConfigState configState;

    private ConfigManager()
    { }

    internal void Save()
    {
        var options = GetSerializerOptions();
        var configPath = Path.Combine(
            Output.GetAppDataDirectory(),
            CONFIG_PATH);

        File.WriteAllText(configPath, JsonSerializer.Serialize(this.configState, options));
    }

    private static ConfigManager LoadOrCreate()
    {
        var configPath = Path.Combine(
            Output.GetAppDataDirectory(),
            CONFIG_PATH);

        if (!File.Exists(configPath))
        {
            instance = new ConfigManager();
#pragma warning disable IDE0017 // Simplify object initialization (would break since ConfigState checks IsInitialized)
            instance.configState = new ConfigState();
            instance.IsInitialized = true;
#pragma warning restore IDE0017 // Simplify object initialization
            instance.Save();
            return instance;
        }

        var options = GetSerializerOptions();
        instance = new ConfigManager();
#pragma warning disable IDE0017 // Simplify object initialization (would break since ConfigState checks IsInitialized)
        instance.configState = JsonSerializer.Deserialize<ConfigState>(File.ReadAllText(configPath), options);
#pragma warning restore IDE0017 // Simplify object initialization

        var assembly = System.Reflection.Assembly.GetEntryAssembly();

        // If the assembly is null then we are running in a unit test
        if (assembly == null)
        {
            instance.IsInitialized = true;
            return instance;
        }

        var executablePath = assembly.Location;
        if (executablePath.EndsWith("Key2Joy.exe")
            && instance.configState.LastInstallPath != executablePath)
        {
            instance.configState.LastInstallPath = executablePath;
            instance.Save();
        }

        instance.IsInitialized = true;

        return instance;
    }

    private static JsonSerializerOptions GetSerializerOptions()
    {
        JsonSerializerOptions options = new()
        {
            WriteIndented = true
        };

        return options;
    }

    /// <summary>
    /// Turns the plugin path into a relative one from the app directory
    /// </summary>
    /// <param name="pluginAssemblyPath"></param>
    /// <returns></returns>
    private string NormalizePluginPath(string pluginAssemblyPath)
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
    internal void SetPluginEnabled(string pluginAssemblyPath, string permissionsChecksumOrNull)
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

    internal bool IsPluginEnabled(string pluginAssemblyPath)
        => this.configState.EnabledPlugins.ContainsKey(this.NormalizePluginPath(pluginAssemblyPath));

    internal string GetExpectedChecksum(string pluginAssemblyPath)
        => this.configState.EnabledPlugins.ContainsKey(this.NormalizePluginPath(pluginAssemblyPath))
            ? this.configState.EnabledPlugins[this.NormalizePluginPath(pluginAssemblyPath)]
            : null;
}
