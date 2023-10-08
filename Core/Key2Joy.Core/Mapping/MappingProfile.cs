using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Key2Joy.Config;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Contracts.Mapping.Triggers;
using Key2Joy.Util;

namespace Key2Joy.Mapping;

public class MappingProfile
{
    private const int NO_VERSION = 0;
    private const int CURRENT_VERSION = 5;

    public const string DEFAULT_PROFILE_PATH = "default-profile";
    public const string EXTENSION = ".k2j.json";

    public const string LEGACY_SAVE_DIR = "Presets";
    public const string SAVE_DIR = "Profiles";

    public BindingList<MappedOption> MappedOptions { get; set; } = new BindingList<MappedOption>();

    public string Name { get; set; }

    public int Version { get; set; } = NO_VERSION; // Version is set on save

    [JsonIgnore]
    public string FilePath { get; private set; }

    [JsonIgnore]
    public string Display => $"{this.Name} ({Path.GetFileName(this.FilePath)})";

    [JsonConstructor]
    public MappingProfile(string name, BindingList<MappedOption> mappedOptions = null)
    {
        this.Name = name;

        var directory = GetSaveDirectory();

        this.FilePath ??= FileSystem.FindNonExistingFile(Path.Combine(directory, $"profile-%VERSION%{EXTENSION}"));

        if (mappedOptions != null)
        {
            foreach (var mappedOption in mappedOptions)
            {
                this.MappedOptions.Add((MappedOption)mappedOption.Clone());
            }
        }
    }

    public void AddMapping(MappedOption mappedOption) => this.MappedOptions.Add(mappedOption);

    public void AddMappingRange(IEnumerable<MappedOption> mappedOptions)
    {
        foreach (var mappedOption in mappedOptions)
        {
            this.MappedOptions.Add((MappedOption)mappedOption.Clone());
        }
    }

    public void RemoveMapping(MappedOption mappedOption) => this.MappedOptions.Remove(mappedOption);

    public bool TryGetMappedOption(AbstractTrigger trigger, out MappedOption mappedOption)
    {
        mappedOption = this.MappedOptions.FirstOrDefault(mo => mo.Trigger == trigger);
        return mappedOption != null;
    }

    public void Save()
    {
        var options = GetSerializerOptions();

        this.Version = CURRENT_VERSION;
        File.WriteAllText(this.FilePath, JsonSerializer.Serialize(this, options));
    }

    private bool PostLoad(string filePath)
    {
        this.FilePath = filePath;

        return true;
    }

    private static string GetDefaultPath() => Path.Combine(GetSaveDirectory(), $"{DEFAULT_PROFILE_PATH}{EXTENSION}");

    public static void ExtractDefaultIfNotExists()
    {
        var defaultPath = GetDefaultPath();

        if (File.Exists(defaultPath))
        {
            return;
        }

        using (FileStream file = new(defaultPath, FileMode.Create, FileAccess.Write))
        using (BinaryWriter writer = new(file))
        {
            writer.Write(Properties.Resources.default_profile_k2j);
        }

        if (ConfigManager.Config.LastLoadedProfile == null)
        {
            ConfigManager.Config.LastLoadedProfile = defaultPath;
        }
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
                {
                    return null;
                }
            }
        }

        profile = JsonSerializer.Deserialize<MappingProfile>(File.ReadAllText(filePath), options);

        if (profile.PostLoad(filePath))
        {
            return profile;
        }

        return null;
    }

    public static MappingProfile RestoreLastLoaded()
    {
        var lastLoadedPath = ConfigManager.Config.LastLoadedProfile ?? GetDefaultPath();
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

        JsonSerializerOptions options = new();
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
