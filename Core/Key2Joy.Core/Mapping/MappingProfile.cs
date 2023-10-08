using Key2Joy.Config;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Util;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Key2Joy.Mapping
{
    public class MappingProfile
    {
        const int NO_VERSION = 0;
        const int CURRENT_VERSION = 5;

        public const string DEFAULT_PROFILE_PATH = "default-profile";
        public const string EXTENSION = ".k2j.json";

        public const string LEGACY_SAVE_DIR = "Presets";
        public const string SAVE_DIR = "Profiles";

        public BindingList<MappedOption> MappedOptions { get; set; } = new BindingList<MappedOption>();

        public string Name { get; set; }

        public int Version { get; set; } = NO_VERSION; // Version is set on save

        [JsonIgnore]
        public string FilePath => filePath;

        [JsonIgnore]
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

        public bool TryGetMappedOption(AbstractTrigger trigger, out MappedOption mappedOption)
        {
            mappedOption = MappedOptions.FirstOrDefault(mo => mo.Trigger == trigger);
            return mappedOption != null;
        }

        public void Save()
        {
            var options = GetSerializerOptions();

            this.Version = CURRENT_VERSION;
            File.WriteAllText(filePath, JsonSerializer.Serialize(this, options));
        }

        private bool PostLoad(string filePath)
        {
            this.filePath = filePath;

            return true;
        }

        private static string GetDefaultPath() => Path.Combine(GetSaveDirectory(), $"{DEFAULT_PROFILE_PATH}{EXTENSION}");

        public static void ExtractDefaultIfNotExists()
        {
            var defaultPath = GetDefaultPath();

            if (File.Exists(defaultPath))
                return;

            using (var file = new FileStream(defaultPath, FileMode.Create, FileAccess.Write))
            using (var writer = new BinaryWriter(file))
            {
                writer.Write(Properties.Resources.default_profile_k2j);
            }

            if (ConfigManager.Config.LastLoadedProfile == null)
                ConfigManager.Config.LastLoadedProfile = defaultPath;
        }

        public static MappingProfile Load(string filePath)
        {
            var options = GetSerializerOptions();

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

            profile = JsonSerializer.Deserialize<MappingProfile>(File.ReadAllText(filePath), options);

            if (profile.PostLoad(filePath))
                return profile;

            return null;
        }

        public static MappingProfile RestoreLastLoaded()
        {
            var lastLoadedPath = ConfigManager.Config.LastLoadedProfile;

            if (lastLoadedPath == null)
                lastLoadedPath = GetDefaultPath();

            if (!File.Exists(lastLoadedPath))
            {
                ExtractDefaultIfNotExists();
                lastLoadedPath = GetDefaultPath();
            }

            return Load(lastLoadedPath);
        }

        private static JsonSerializerOptions GetSerializerOptions()
        {
            // TODO: serializer.SerializationBinder = new MappingProfileSerializationBinder();

            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter());
            options.Converters.Add(new JsonMappingAspectConverter<AbstractAction>());
            options.Converters.Add(new JsonMappingAspectConverter<AbstractTrigger>());
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.WriteIndented = true;

            return options;
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
            {
                if (Directory.Exists(directory))
                {
                    Directory.Move(legacyDirectory, FileSystem.FindNonExistingFile(legacyDirectory + "-%VERSION%"));
                }
                else
                {
                    Directory.Move(legacyDirectory, directory);
                }
            }
            else if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            return directory;
        }
    }
}
