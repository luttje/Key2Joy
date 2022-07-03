using Key2Joy.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Key2Joy.Mapping
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class MappingPreset
    {
        const int NO_VERSION = 0;
        const int CURRENT_VERSION = 4;
        
        public const string DEFAULT_PRESET_PATH = "default-profile";
        public const string EXTENSION = ".k2j.json";

        public const string SAVE_DIR = "Presets";

        [JsonProperty]
        public BindingList<MappedOption> MappedOptions { get; set; } = new BindingList<MappedOption>();

        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public int Version { get; set; } = NO_VERSION; // Version is set on save
        
        public string FilePath => filePath;

        public string Display => $"{Name} ({Path.GetFileName(filePath)})";

        private string filePath;

        [JsonConstructor]
        internal MappingPreset(string name, BindingList<MappedOption> mappedOptions = null)
        {
            Name = name;

            var directory = GetSaveDirectory();

            if (filePath == null)
                filePath = FileSystem.FindNonExistingFile(Path.Combine(directory, $"profile-%VERSION%{EXTENSION}"));

            if (mappedOptions != null)
            {
                foreach (var mappedOption in mappedOptions)
                {
                    MappedOptions.Add((MappedOption)mappedOption.Clone());
                }
            }
        }

        internal void AddMapping(MappedOption mappedOption)
        {
            MappedOptions.Add(mappedOption);
        }

        internal void AddMappingRange(IEnumerable<MappedOption> mappedOptions)
        {
            foreach (var mappedOption in mappedOptions)
                MappedOptions.Add((MappedOption)mappedOption.Clone());
        }

        internal void RemoveMapping(MappedOption mappedOption)
        {
            MappedOptions.Remove(mappedOption);
        }

        internal bool TryGetMappedOption(BaseTrigger trigger, out MappedOption mappedOption)
        {
            mappedOption = MappedOptions.FirstOrDefault(mo => mo.Trigger == trigger);
            return mappedOption != null;
        }

        internal void Save()
        {
            var serializer = GetSerializer();

            using (var sw = new StreamWriter(filePath))
            using (var writer = new JsonTextWriter(sw))
            {
                this.Version = CURRENT_VERSION;
                serializer.Serialize(writer, this);
            }
        }

        private bool PostLoad(string filePath)
        {
            this.filePath = filePath;

            return true;
        }

        internal static void ExtractDefaultIfNotExists()
        {
            var defaultPath = Path.Combine(GetSaveDirectory(), $"{DEFAULT_PRESET_PATH}{EXTENSION}");
            
            if (File.Exists(defaultPath))
                return;

            using (var file = new FileStream(defaultPath, FileMode.Create, FileAccess.Write))
            using (var writer = new BinaryWriter(file))
            {
                writer.Write(Properties.Resources.default_profile_k2j);
            }

            if(Config.Instance.LastLoadedPreset == null)
                Config.Instance.LastLoadedPreset = defaultPath;
        }

        internal static MappingPreset Load(string filePath)
        {
            var serializer = GetSerializer();

            MappingPreset preset;

            using (var sr = new StreamReader(filePath))
            using (var reader = new JsonTextReader(sr))
                preset = serializer.Deserialize<MappingPreset>(reader);

            if (preset.PostLoad(filePath))
                return preset;

            return null;
        }

        internal static MappingPreset RestoreLastLoaded()
        {
            var lastLoadedPath = Config.Instance.LastLoadedPreset;

            if (lastLoadedPath == null)
                return null;

            if (!File.Exists(lastLoadedPath))
                return null;

            return Load(lastLoadedPath);
        }

        private static JsonSerializer GetSerializer()
        {
            var serializer = new JsonSerializer();
            serializer.SerializationBinder = new MappingPresetSerializationBinder();
            serializer.Converters.Add(new StringEnumConverter());
            serializer.Formatting = Formatting.Indented;

            return serializer;
        }

        internal static string GetSaveDirectory()
        {
            var directory = Path.Combine(
                Program.GetAppDirectory(),
                SAVE_DIR);

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            return directory;
        }
    }
}
