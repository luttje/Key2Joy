using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SimWinInput;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace KeyToJoy.Input
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class BindingPreset
    {
        const int NO_VERSION = 0;
        const int CURRENT_VERSION = 3;

        const string SAVE_DIR = "Key2Joy Presets";
        public static BindingList<BindingPreset> All { get; } = new BindingList<BindingPreset>();

        [JsonProperty]
        public List<BindingOption> Bindings { get; set; } = new List<BindingOption>();

        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public int Version { get; set; } = NO_VERSION; // Version is set on save

        public string Display => $"{Name} ({Path.GetFileName(filePath)})";

        private string filePath;
        private Dictionary<string, BindingOption> lookup = new Dictionary<string, BindingOption>();

        [JsonConstructor]
        internal BindingPreset(string name, List<BindingOption> bindings = null)
        {
            Name = name;

            var directory = GetSaveDirectory();

            if (filePath == null)
            {
                int alt = 1;
                do
                {
                    filePath = Path.Combine(directory, $"profile-{alt}.key2joy.json");
                    alt++;
                } while (File.Exists(filePath));
            }

            if (bindings != null)
            {
                foreach (var binding in bindings)
                {
                    Bindings.Add((BindingOption)binding.Clone());
                }
            }
        }

        internal static void Add(BindingPreset preset, bool blockSave = false)
        {
            if (preset.Bindings.Count < BindableAction.All.Count)
            {
                // Find missing binding options and list them as unbound in this preset
                var missingBindables = BindableAction.All.Where(l2 => !preset.Bindings.Any(l1 => l1.Action == l2));

                foreach (var bindableAction in missingBindables)
                {
                    preset.Bindings.Add(new BindingOption
                    {
                        Action = bindableAction,
                        Binding = null
                    });
                }
            }

            All.Add(preset);

            if (!blockSave)
                preset.Save();
        }

        internal void AddOption(BindingOption bindingOption)
        {
            Bindings.Add(bindingOption);
            CacheLookup(bindingOption);
        }

        internal void PruneCacheKey(string oldBindingKey)
        {
            lookup.Remove(oldBindingKey);
        }

        internal void CacheLookup(BindingOption bindingOption)
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

        internal bool TryGetBinding(Binding binding, out BindingOption bindingOption)
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

        internal static List<BindingPreset> LoadAll()
        {
            var presets = new List<BindingPreset>();
            var serializer = GetSerializer();

            foreach (var filePath in Directory.EnumerateFiles(GetSaveDirectory(), "*.key2joy.json"))
            {
                using (var sr = new StreamReader(filePath))
                using (var reader = new JsonTextReader(sr))
                {
                    var preset = serializer.Deserialize<BindingPreset>(reader);
                    preset.PostLoad(filePath);
                    presets.Add(preset);
                }
            }

            return presets;
        }

        private void PostLoad(string filePath)
        {
            this.filePath = filePath;

            if (this.Version != CURRENT_VERSION)
                MessageBox.Show($"Preset @ {filePath} was version {this.Version} whilst current application version is {CURRENT_VERSION}! Some features may be missing because of this. It's best to just remove the preset and create a new one.", "Outdated preset loaded!", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            CacheAllLookup();
        }

        private static JsonSerializer GetSerializer()
        {
            var serializer = new JsonSerializer();
            serializer.Converters.Add(new StringEnumConverter());
            serializer.Formatting = Formatting.Indented;

            return serializer;
        }

        private static string GetSaveDirectory()
        {
            var directory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                SAVE_DIR);

            Directory.CreateDirectory(directory);

            return directory;
        }
    }
}
