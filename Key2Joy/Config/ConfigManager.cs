﻿using Newtonsoft.Json;
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

        public static ConfigManager Instance { get; private set; }
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
            Text = "Last loaded mapping preset file location"
        )]
        [JsonProperty]
        public string LastLoadedPreset
        {
            get => lastLoadedPreset;
            set => SaveIfInitialized(lastLoadedPreset = value);
        }
        private string lastLoadedPreset;

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

        public static void Load()
        {
            var configPath = Path.Combine(
                GetAppDirectory(),
                CONFIG_PATH);

            if (!File.Exists(configPath))
            {
                Instance = new ConfigManager();
                Instance.isInitialized = true;
                Instance.Save();
                return;
            }
            
            var serializer = GetSerializer();
                
            using (var sr = new StreamReader(configPath))
            using (var reader = new JsonTextReader(sr))
                Instance = serializer.Deserialize<ConfigManager>(reader);

            Instance.isInitialized = true;
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
