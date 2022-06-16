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
                filePath = Util.FileSystem.FindNonExistingFile(Path.Combine(directory, $"profile-%VERSION%.key2joy.json"));

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
            // Ensure all actions in presets are loaded from the available actions in this app
            foreach (var bindableAction in BindableAction.All)
            {
                var binding = preset.Bindings.Where(b => b.Action == bindableAction).FirstOrDefault();

                if(binding == null) { 
                    preset.Bindings.Add(new BindingOption
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
                BindingPreset preset;

                using (var sr = new StreamReader(filePath))
                using (var reader = new JsonTextReader(sr))
                {
                    preset = serializer.Deserialize<BindingPreset>(reader);
                }

                if (preset.PostLoad(filePath))
                    presets.Add(preset);
            }

            return presets;
        }

        private bool PostLoad(string filePath)
        {
            this.filePath = filePath;

            if(this.Version == 2)
            {
                this.filePath = Util.FileSystem.FindNonExistingFile($"{filePath}.%VERSION%.bak");
                Save();
                File.Delete(filePath);
                MessageBox.Show($"Preset @ {filePath} was version {this.Version} whilst current application version is {CURRENT_VERSION}! \n\nThis old version is no longer supported. You will have to create a new preset. \n\nA backup of this outdated preset has been made in the Documents/Key2Joy Presets folder.", "Outdated preset failed to load!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (this.Version != CURRENT_VERSION)
                MessageBox.Show($"Preset @ {filePath} was version {this.Version} whilst current application version is {CURRENT_VERSION}! \n\nSome features may be missing because of this. \n\nIt's best to just remove the preset and create a new one.", "Outdated preset loaded!", MessageBoxButtons.OK, MessageBoxIcon.Warning);

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
