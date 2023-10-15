using System.IO;
using System.Text.Json;
using Key2Joy.Contracts;

namespace Key2Joy.Config;

/// <summary>
/// Manages user configurations being loaded from and saved to disk.
/// </summary>
public class ConfigManager
{
    protected const string CONFIG_PATH = "config.json";

    private static ConfigManager instance;

    public static ConfigManager Instance
    {
        get
        {
            instance ??= LoadOrCreate();

            return instance;
        }
    }

    public static ConfigState Config => Instance.configState;

    public bool IsInitialized { get; private set; }
    private ConfigState configState;

    protected ConfigManager()
    { }

    /// <returns>The path to where the config file is located.</returns>
    protected virtual string GetAppDataDirectory() => Output.GetAppDataDirectory();

    internal void Save()
    {
        var options = GetSerializerOptions();
        var configPath = Path.Combine(
            this.GetAppDataDirectory(),
            CONFIG_PATH);

        File.WriteAllText(configPath, JsonSerializer.Serialize(this.configState, options));
    }

    protected static ConfigManager LoadOrCreate(ConfigManager customInstance = null)
    {
        customInstance ??= new ConfigManager();
        instance = customInstance;

        var configPath = Path.Combine(
            instance.GetAppDataDirectory(),
            CONFIG_PATH);

        if (!File.Exists(configPath))
        {
            instance.configState = new ConfigState();
            instance.IsInitialized = true;
            instance.Save();
            return instance;
        }

        var options = GetSerializerOptions();
        instance.configState = JsonSerializer.Deserialize<ConfigState>(File.ReadAllText(configPath), options);

        var assembly = System.Reflection.Assembly.GetEntryAssembly();

        // If the assembly is null then we are running in a unit test
        if (assembly == null)
        {
            instance.CompleteInitialization();
            return instance;
        }

        var executablePath = assembly.Location;
        if (executablePath.EndsWith("Key2Joy.exe")
            && instance.configState.LastInstallPath != executablePath)
        {
            instance.configState.LastInstallPath = executablePath;
        }

        instance.CompleteInitialization();
        return instance;
    }

    private void CompleteInitialization()
    {
        instance.IsInitialized = true;

        // We save so old properties are removed and new ones are added to the config file immediately
        instance.Save();
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
