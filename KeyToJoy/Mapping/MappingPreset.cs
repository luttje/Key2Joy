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
        public List<MappedOption> Bindings { get; set; } = new List<MappedOption>();

        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public int Version { get; set; } = NO_VERSION; // Version is set on save

        public string Display => $"{Name} ({Path.GetFileName(filePath)})";

        private string filePath;
        private Dictionary<string, MappedOption> lookup = new Dictionary<string, MappedOption>();

        [JsonConstructor]
        internal MappingPreset(string name, List<MappedOption> bindings = null)
        {
            Name = name;

            var directory = GetSaveDirectory();

            if (filePath == null)
                filePath = Util.FileSystem.FindNonExistingFile(Path.Combine(directory, $"profile-%VERSION%.{EXTENSION}.json"));

            if (bindings != null)
            {
                foreach (var binding in bindings)
                {
                    Bindings.Add((MappedOption)binding.Clone());
                }
            }
        }

        internal static void Add(MappingPreset preset, bool blockSave = false)
        {
            // Ensure all actions in presets are loaded from the available actions in this app
            foreach (var bindableAction in BaseAction.All)
            {
                var binding = preset.Bindings.Where(b => b.Action == bindableAction).FirstOrDefault();

                if(binding == null) { 
                    preset.Bindings.Add(new MappedOption
                    {
                        Action = bindableAction,
                        Binding = null
                    });
                }
                else 
                    binding.Action = bindableAction;
            }

            All.Add(preset);

            if (!blockSave)
                preset.Save();
        }

        internal void AddOption(MappedOption bindingOption)
        {
            Bindings.Add(bindingOption);
            CacheLookup(bindingOption);
        }

        internal void PruneCacheKey(string oldBindingKey)
        {
            lookup.Remove(oldBindingKey);
        }

        internal void CacheLookup(MappedOption bindingOption)
        {
            if (bindingOption.Binding == null)
                return;

            lookup.Add(bindingOption.Binding.GetUniqueBindingKey(), bindingOption);
        }

        private void CacheAllLookup()
        {
            foreach (var bindingOption in Bindings)
            {
                CacheLookup(bindingOption);
            }
        }

        internal bool TryGetBinding(BaseTrigger binding, out MappedOption bindingOption)
        {
            return lookup.TryGetValue(binding.GetUniqueBindingKey(), out bindingOption);
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
