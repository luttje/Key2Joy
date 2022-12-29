using Key2Joy.Config;
using Key2Joy.LowLevelInput;
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
    public class MappingProfile
    {
        const int NO_VERSION = 0;
        const int CURRENT_VERSION = 4;
        
        public const string DEFAULT_PROFILE_PATH = "default-profile";
        public const string EXTENSION = ".k2j.json";

        public const string LEGACY_SAVE_DIR = "Presets";
        public const string SAVE_DIR = "Profiles";

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
        public MappingProfile(string name, BindingList<MappedOption> mappedOptions = null)
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

        public void AddMapping(MappedOption mappedOption)
        {
            MappedOptions.Add(mappedOption);
        }

        public void AddMappingRange(IEnumerable<MappedOption> mappedOptions)
        {
            foreach (var mappedOption in mappedOptions)
                MappedOptions.Add((MappedOption)mappedOption.Clone());
        }

        public void RemoveMapping(MappedOption mappedOption)
        {
            MappedOptions.Remove(mappedOption);
        }

        public bool TryGetMappedOption(BaseTrigger trigger, out MappedOption mappedOption)
        {
            mappedOption = MappedOptions.FirstOrDefault(mo => mo.Trigger == trigger);
            return mappedOption != null;
        }

        public void Save()
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

            LegacyPressStateConverter.UpdateLegacyIfApplicable(this);

            return true;
        }

        public static void ExtractDefaultIfNotExists()
        {
            var defaultPath = Path.Combine(GetSaveDirectory(), $"{DEFAULT_PROFILE_PATH}{EXTENSION}");
            
            if (File.Exists(defaultPath))
                return;

            using (var file = new FileStream(defaultPath, FileMode.Create, FileAccess.Write))
            using (var writer = new BinaryWriter(file))
            {
                writer.Write(Properties.Resources.default_profile_k2j);
            }

            if(ConfigManager.Instance.LastLoadedProfile == null)
                ConfigManager.Instance.LastLoadedProfile = defaultPath;
        }

        public static MappingProfile Load(string filePath)
        {
            var serializer = GetSerializer();

            MappingProfile profile;

            if (!File.Exists(filePath))
            {
                var directory = GetSaveDirectory();
                filePath = Path.Combine(directory, filePath);

                if (!File.Exists(filePath))
                {
                    filePath += EXTENSION;

                    if (!File.Exists(filePath))
                        return null;
                }
            }

            using (var sr = new StreamReader(filePath))
            using (var reader = new JsonTextReader(sr))
                profile = serializer.Deserialize<MappingProfile>(reader);

            if (profile.PostLoad(filePath))
                return profile;

            return null;
        }

        public static MappingProfile RestoreLastLoaded()
        {
            var lastLoadedPath = ConfigManager.Instance.LastLoadedProfile;

            if (lastLoadedPath == null)
                return null;

            if (!File.Exists(lastLoadedPath))
                return null;

            return Load(lastLoadedPath);
        }

        private static JsonSerializer GetSerializer()
        {
            var serializer = new JsonSerializer();
            serializer.SerializationBinder = new MappingProfileSerializationBinder();
            serializer.Converters.Add(new StringEnumConverter());
            serializer.Formatting = Formatting.Indented;

            return serializer;
        }

        public static string GetSaveDirectory()
        {
            var legacyDirectory = Path.Combine(
                ConfigManager.GetAppDirectory(),
                LEGACY_SAVE_DIR);
            var directory = Path.Combine(
                ConfigManager.GetAppDirectory(),
                SAVE_DIR);

            if (Directory.Exists(legacyDirectory))
                Directory.Move(legacyDirectory, directory);
            else if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            return directory;
        }
    }
}
