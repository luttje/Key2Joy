using System;
using System.IO;
using System.Text.Json;

namespace Key2Joy.Config
{
    public class ConfigManager
    {
        const string APP_DIR = "Key2Joy";
        const string CONFIG_PATH = "config.json";

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
                GetAppDirectory(),
                CONFIG_PATH);

            File.WriteAllText(configPath, JsonSerializer.Serialize(configState, options));
        }
        
        private static ConfigManager LoadOrCreate()
        {
            var configPath = Path.Combine(
                GetAppDirectory(),
                CONFIG_PATH);

            if (!File.Exists(configPath))
            {
                instance = new ConfigManager();
                instance.configState = new ConfigState();
                instance.IsInitialized = true;
                instance.Save();
                return instance;
            }
            
            var options = GetSerializerOptions();
            instance = new ConfigManager();
            instance.configState = JsonSerializer.Deserialize<ConfigState>(File.ReadAllText(configPath), options);

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
            var options = new JsonSerializerOptions();
            options.WriteIndented = true;

            return options;
        }

        public static string GetAppDirectory()
        {
            var directory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                APP_DIR);

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            return directory;
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
            if (permissionsChecksumOrNull != null)
            {
                if (!configState.EnabledPlugins.ContainsKey(pluginAssemblyPath))
                {
                    configState.EnabledPlugins.Add(pluginAssemblyPath, permissionsChecksumOrNull);
                }
                else
                {
                    configState.EnabledPlugins[pluginAssemblyPath] = permissionsChecksumOrNull;
                }
            } 
            else
            {
                if (configState.EnabledPlugins.ContainsKey(pluginAssemblyPath))
                {
                    configState.EnabledPlugins.Remove(pluginAssemblyPath);
                }
            }
            
            Save();
        }
    }
}
