using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyToJoy
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class Config
    {
        const string CONFIG_PATH = "config.json";

        public static Config Instance { get; private set; }

        [JsonProperty]
        public bool MuteCloseExitMessage
        {
            get => muteCloseExitMessage;
            set
            {
                muteCloseExitMessage = value;

                if (isInitialized)
                    Save();
            }
        }
        private bool muteCloseExitMessage;

        [JsonProperty]
        public string LastLoadedPreset
        {
            get => lastLoadedPreset;
            set
            {
                lastLoadedPreset = value;

                if (isInitialized)
                    Save();
            }
        }
        private string lastLoadedPreset;

        private bool isInitialized;

        private Config() { }

        private void Save()
        {
            var serializer = GetSerializer();
            var configPath = Path.Combine(
                Program.GetAppDirectory(),
                CONFIG_PATH);

            using (var sw = new StreamWriter(configPath))
            using (var writer = new JsonTextWriter(sw))
                serializer.Serialize(writer, this);
        }

        public static void Load()
        {
            var configPath = Path.Combine(
                Program.GetAppDirectory(),
                CONFIG_PATH);

            if (!File.Exists(configPath))
            {
                Instance = new Config();
                Instance.isInitialized = true;
                Instance.Save();
                return;
            }
            
            var serializer = GetSerializer();
                
            using (var sr = new StreamReader(configPath))
            using (var reader = new JsonTextReader(sr))
                Instance = serializer.Deserialize<Config>(reader);

            Instance.isInitialized = true;
        }

        private static JsonSerializer GetSerializer()
        {
            var serializer = new JsonSerializer();
            serializer.Formatting = Formatting.Indented;

            return serializer;
        }
    }
}
