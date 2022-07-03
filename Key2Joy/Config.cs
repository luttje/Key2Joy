using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class Config
    {
        const string CONFIG_PATH = "config.json";

        public static Config Instance { get; private set; }
        private bool isInitialized;

        [ConfigControl(
            Text = "Mute informative message about this app minimizing by default",
            ControlType = typeof(System.Windows.Forms.CheckBox)
        )]
        [JsonProperty]
        public bool MuteCloseExitMessage
        {
            get => muteCloseExitMessage;
            set => SaveIfInitialized(muteCloseExitMessage = value);
        }
        private bool muteCloseExitMessage;

        [ConfigControl(
            Text = "Last loaded mapping preset file location",
            ControlType = typeof(System.Windows.Forms.TextBox)
        )]
        [JsonProperty]
        public string LastLoadedPreset
        {
            get => lastLoadedPreset;
            set => SaveIfInitialized(lastLoadedPreset = value);
        }
        private string lastLoadedPreset;

        [ConfigControl(
            Text = "Release button after how many milliseconds after pressing it with PressAndRelease",
            ControlType = typeof(System.Windows.Forms.NumericUpDown)
        )]
        [JsonProperty]
        public int PressReleaseWaitTime
        {
            get => pressReleaseWaitTime;
            set => SaveIfInitialized(pressReleaseWaitTime = value);
        }
        private int pressReleaseWaitTime = 50;

        [ConfigControl(
            Text = "Path to directory where logs are saved",
            ControlType = typeof(System.Windows.Forms.TextBox)
        )]
        [JsonProperty]
        public string LogOutputPath
        {
            get => logOutputPath;
            set => SaveIfInitialized(logOutputPath = value);
        }
        private string logOutputPath = Path.Combine(Program.GetAppDirectory(), "Logs");

        private Config() { }
        private void SaveIfInitialized(object changedValue = null)
        {
            if (isInitialized)
                Save();
        }

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
