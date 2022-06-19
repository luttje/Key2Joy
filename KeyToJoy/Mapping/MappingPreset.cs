using KeyToJoy.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SimWinInput;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;

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
        public List<MappedOption> MappedOptions { get; set; } = new List<MappedOption>();

        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public int Version { get; set; } = NO_VERSION; // Version is set on save

        public string Display => $"{Name} ({Path.GetFileName(filePath)})";

        private string filePath;
        private Dictionary<string, MappedOption> lookup = new Dictionary<string, MappedOption>();

        [JsonConstructor]
        internal MappingPreset(string name, List<MappedOption> mappedOptions = null)
        {
            Name = name;

            var directory = GetSaveDirectory();

            if (filePath == null)
                filePath = Util.FileSystem.FindNonExistingFile(Path.Combine(directory, $"profile-%VERSION%.{EXTENSION}.json"));

            if (mappedOptions != null)
            {
                foreach (var mappedOption in mappedOptions)
                {
                    MappedOptions.Add((MappedOption)mappedOption.Clone());
                }
            }
        }

        internal static void Add(MappingPreset preset, bool blockSave = false)
        {
            // Ensure all actions in presets are loaded from the available actions in this app
            foreach (var action in BaseAction.All)
            {
                var mappedOption = preset.MappedOptions.Where(b => b.Action == action).FirstOrDefault();

                if(mappedOption == null) { 
                    preset.MappedOptions.Add(new MappedOption
                    {
                        Action = action,
                        Trigger = null
                    });
                }
                else 
                    mappedOption.Action = action;
            }

            All.Add(preset);

            if (!blockSave)
                preset.Save();
        }

        internal void AddMapping(MappedOption mappedOption)
        {
            MappedOptions.Add(mappedOption);
            CacheLookup(mappedOption);
        }

        internal void PruneCacheKey(string oldMappingKey)
        {
            lookup.Remove(oldMappingKey);
        }

        internal void CacheLookup(MappedOption mappedOption)
        {
            if (mappedOption.Trigger == null)
                return;

            lookup.Add(mappedOption.Trigger.GetUniqueKey(), mappedOption);
        }

        private void CacheAllLookup()
        {
            foreach (var mappedOption in MappedOptions)
            {
                CacheLookup(mappedOption);
            }
        }

        internal bool TryGetMappedOption(BaseTrigger trigger, out MappedOption mappedOption)
        {
            return lookup.TryGetValue(trigger.GetUniqueKey(), out mappedOption);
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

        private bool PostLoad(string filePath)
        {
            this.filePath = filePath;

            CacheAllLookup();

            return true;
        }

        private static JsonSerializer GetSerializer()
        {
            var serializer = new JsonSerializer();
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
