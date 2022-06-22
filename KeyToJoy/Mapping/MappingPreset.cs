using KeyToJoy.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace KeyToJoy.Mapping
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class MappingPreset
    {
        const int NO_VERSION = 0;
        const int CURRENT_VERSION = 4;

        const string EXTENSION = "k2j";
        const string SAVE_DIR = "Key2Joy Presets";
        public static BindingList<MappingPreset> All { get; } = new BindingList<MappingPreset>();

        [JsonProperty]
        public BindingList<MappedOption> MappedOptions { get; set; } = new BindingList<MappedOption>();

        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public int Version { get; set; } = NO_VERSION; // Version is set on save

        public string Display => $"{Name} ({Path.GetFileName(filePath)})";

        private string filePath;

        [JsonConstructor]
        internal MappingPreset(string name, BindingList<MappedOption> mappedOptions = null)
        {
            Name = name;

            var directory = GetSaveDirectory();

            if (filePath == null)
                filePath = FileSystem.FindNonExistingFile(Path.Combine(directory, $"profile-%VERSION%.{EXTENSION}.json"));

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

        internal static void Add(MappingPreset preset, bool blockSave = false)
        {
            All.Add(preset);

            if (!blockSave)
                preset.Save();
        }

        internal static List<MappingPreset> LoadAll()
        {
            var presets = new List<MappingPreset>();
            var serializer = GetSerializer();

            foreach (var filePath in Directory.EnumerateFiles(GetSaveDirectory(), $"*.{EXTENSION}.json"))
            {
                MappingPreset preset;

                using (var sr = new StreamReader(filePath))
                using (var reader = new JsonTextReader(sr))
                {
                    preset = serializer.Deserialize<MappingPreset>(reader);
                }

                if (preset.PostLoad(filePath))
                    presets.Add(preset);
            }

            return presets;
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
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                SAVE_DIR);

            Directory.CreateDirectory(directory);

            return directory;
        }
    }
}
