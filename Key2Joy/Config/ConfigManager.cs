using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Config
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ConfigManager
    {
        const string APP_DIR = "Key2Joy";
        const string CONFIG_PATH = "config.json";

        public static ConfigManager instance;
        public static ConfigManager Instance
        {
            get
            {
                if (instance == null)
                    instance = LoadOrCreate();

                return instance;
            }
        }
        private bool isInitialized;

        [BooleanConfigControl(
            Text = "Mute informative message about this app minimizing by default"
        )]
        [JsonProperty]
        public bool MuteCloseExitMessage
        {
            get => muteCloseExitMessage;
            set => SaveIfInitialized(muteCloseExitMessage = value);
        }
        private bool muteCloseExitMessage;

        [BooleanConfigControl(
            Text = "Override default behaviour when trigger action is executed"
        )]
        [JsonProperty]
        public bool OverrideDefaultTriggerBehaviour
        {
            get => overrideDefaultTriggerBehaviour;
            set => SaveIfInitialized(overrideDefaultTriggerBehaviour = value);
        }
        private bool overrideDefaultTriggerBehaviour = true;

        [TextConfigControl(
            Text = "Last loaded mapping profile file location"
        )]
        [JsonProperty]
        public string LastLoadedProfile
        {
            get => lastLoadedProfile;
            set => SaveIfInitialized(lastLoadedProfile = value);
        }
        private string lastLoadedProfile;

        [TextConfigControl(
            Text = "Path to directory where logs are saved"
        )]
        [JsonProperty]
        public string LogOutputPath
        {
            get => logOutputPath;
            set => SaveIfInitialized(logOutputPath = value);
        }
        private string logOutputPath = Path.Combine(GetAppDirectory(), "Logs");

        private ConfigManager() { }
        private void SaveIfInitialized(object changedValue = null)
        {
            if (isInitialized)
                Save();
        }

        private void Save()
        {
            var serializer = GetSerializer();
            var configPath = Path.Combine(
                GetAppDirectory(),
                CONFIG_PATH);

            using (var sw = new StreamWriter(configPath))
            using (var writer = new JsonTextWriter(sw))
                serializer.Serialize(writer, this);
        }
        
        private static ConfigManager LoadOrCreate()
        {
            var configPath = Path.Combine(
                GetAppDirectory(),
                CONFIG_PATH);

            if (!File.Exists(configPath))
            {
                instance = new ConfigManager();
                instance.isInitialized = true;
                instance.Save();
                return instance;
            }
            
            var serializer = GetSerializer();
                
            using (var sr = new StreamReader(configPath))
            using (var reader = new JsonTextReader(sr))
                instance = serializer.Deserialize<ConfigManager>(reader);

            instance.isInitialized = true;

            return instance;
        }

        private static JsonSerializer GetSerializer()
        {
            var serializer = new JsonSerializer();
            serializer.Formatting = Formatting.Indented;

            return serializer;
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

    }
}
