using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SimWinInput;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace KeyToJoy.Input
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class BindingPreset
    {
        const string SAVE_DIR = "Key2Joy Presets";
        public static BindingList<BindingPreset> All { get; } = new BindingList<BindingPreset>();

        [JsonProperty]
        public List<BindingOption> Bindings { get; set; } = new List<BindingOption>();

        [JsonProperty]
        public string Name { get; set; }

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
                int version = 1;
                do
                {
                    filePath = Path.Combine(directory, $"profile-{version}.key2joy.json");
                    version++;
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
            All.Add(preset);

            if (!blockSave)
                preset.Save();
        }

        internal void AddOption(BindingOption bindingOption)
        {
            Bindings.Add(bindingOption);
            CacheLookup(bindingOption);
        }

        private void CacheLookup(BindingOption bindingOption)
        {
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

        #region Default Binding Preset
        internal static BindingPreset Default
        {
            get
            {
                var defaultPreset = new BindingPreset("Default");

                // Top of controller
                defaultPreset.AddOption(new BindingOption
                {
                    Control = GamePadControl.LeftShoulder,
                    Binding = new KeyboardBinding(Keys.Q)
                });
                defaultPreset.AddOption(new BindingOption
                {
                    Control = GamePadControl.LeftTrigger,
                    Binding = new KeyboardBinding(Keys.D1)
                });
                defaultPreset.AddOption(new BindingOption
                {
                    Control = GamePadControl.RightShoulder,
                    Binding = new KeyboardBinding(Keys.E)
                });
                defaultPreset.AddOption(new BindingOption
                {
                    Control = GamePadControl.RightTrigger,
                    Binding = new KeyboardBinding(Keys.D2)
                });

                // Left half of controller
                defaultPreset.AddOption(new BindingOption
                {
                    Control = GamePadControl.LeftStickUp,
                    Binding = new KeyboardBinding(Keys.W)
                });
                defaultPreset.AddOption(new BindingOption
                {
                    Control = GamePadControl.LeftStickRight,
                    Binding = new KeyboardBinding(Keys.D)
                });
                defaultPreset.AddOption(new BindingOption
                {
                    Control = GamePadControl.LeftStickDown,
                    Binding = new KeyboardBinding(Keys.S)
                });
                defaultPreset.AddOption(new BindingOption
                {
                    Control = GamePadControl.LeftStickLeft,
                    Binding = new KeyboardBinding(Keys.A)
                });
                defaultPreset.AddOption(new BindingOption
                {
                    Control = GamePadControl.LeftStickClick,
                    Binding = new KeyboardBinding(Keys.LControlKey)
                });

                defaultPreset.AddOption(new BindingOption
                {
                    Control = GamePadControl.DPadUp,
                    Binding = new KeyboardBinding(Keys.Up)
                });
                defaultPreset.AddOption(new BindingOption
                {
                    Control = GamePadControl.DPadRight,
                    Binding = new KeyboardBinding(Keys.Right)
                });
                defaultPreset.AddOption(new BindingOption
                {
                    Control = GamePadControl.DPadDown,
                    Binding = new KeyboardBinding(Keys.Down)
                });
                defaultPreset.AddOption(new BindingOption
                {
                    Control = GamePadControl.DPadLeft,
                    Binding = new KeyboardBinding(Keys.Left)
                });

                // Right half of controller
                defaultPreset.AddOption(new BindingOption
                {
                    Control = GamePadControl.RightStickUp,
                    Binding = new MouseAxisBinding(AxisDirection.Up)
                });
                defaultPreset.AddOption(new BindingOption
                {
                    Control = GamePadControl.RightStickRight,
                    Binding = new MouseAxisBinding(AxisDirection.Right)
                });
                defaultPreset.AddOption(new BindingOption
                {
                    Control = GamePadControl.RightStickDown,
                    Binding = new MouseAxisBinding(AxisDirection.Down)
                });
                defaultPreset.AddOption(new BindingOption
                {
                    Control = GamePadControl.RightStickLeft,
                    Binding = new MouseAxisBinding(AxisDirection.Left)
                });
                defaultPreset.AddOption(new BindingOption
                {
                    Control = GamePadControl.RightStickClick,
                    Binding = new KeyboardBinding(Keys.RControlKey)
                });

                defaultPreset.AddOption(new BindingOption
                {
                    Control = GamePadControl.X,
                    Binding = new KeyboardBinding(Keys.X)
                });
                defaultPreset.AddOption(new BindingOption
                {
                    Control = GamePadControl.Y,
                    Binding = new KeyboardBinding(Keys.Y)
                });
                defaultPreset.AddOption(new BindingOption
                {
                    Control = GamePadControl.A,
                    Binding = new KeyboardBinding(Keys.F)
                });
                defaultPreset.AddOption(new BindingOption
                {
                    Control = GamePadControl.B,
                    Binding = new KeyboardBinding(Keys.Z)
                });

                return defaultPreset;
            }
        }
        #endregion

    }
}
