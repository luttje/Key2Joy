using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Forms;
using CommonServiceLocator;
using Key2Joy.Config;
using Key2Joy.Contracts;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Contracts.Mapping.Triggers;
using Key2Joy.Util;

namespace Key2Joy.Mapping;

public class MappingProfile
{
    private const int NO_VERSION = 0;
    private const int CURRENT_VERSION = 6;

    public const string DEFAULT_PROFILE_PATH = "default-profile";
    public const string EXTENSION = ".k2j.json";

    public const string BACKUP_EXTENSION = ".bak";
    public const string SAVE_DIR = "Profiles";

    public BindingList<MappedOption> MappedOptions { get; set; } = new();
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

            foreach (var mappedOption in this.MappedOptions)
            {
                mappedOption.Initialize(this.MappedOptions);
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

    public void RemoveMapping(MappedOption mappedOption)
        => this.MappedOptions.Remove(mappedOption);

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

    private bool PostLoad(string filePath, bool suppressMessageBox = false)
    {
        this.FilePath = filePath;

        if (this.Version != CURRENT_VERSION)
        {
            // This is an old profile, so we'll make a back-up and save the current one
            var pathFormat = $"{this.FilePath}{BACKUP_EXTENSION}%VERSION%";
            var availablePath = FileSystem.FindNonExistingFile(pathFormat);
            File.Copy(this.FilePath, availablePath, true);

            this.Version = CURRENT_VERSION;
            this.Save();

            if (!suppressMessageBox)
            {
                MessageBox.Show(
                    $"The profile \"{this.Name}\" was an old version and has been updated. The old version has been backed up to \"{availablePath}\".",
                    "Profile Updated",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        return true;
    }

    public static string GetDefaultPath() => Path.Combine(GetSaveDirectory(), $"{DEFAULT_PROFILE_PATH}{EXTENSION}");

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
            writer.Write(GetDefaultProfileContents());
        }

        var configState = ServiceLocator.Current
            .GetInstance<IConfigManager>()
            .GetConfigState();
        if (configState.LastLoadedProfile == null)
        {
            configState.LastLoadedProfile = defaultPath;
        }
    }

    public static byte[] GetDefaultProfileContents()
        => Properties.Resources.default_profile_k2j;

    public static string ResolveProfilePath(string filePath)
    {
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

        return filePath;
    }

    public static string ResolveLastLoadedProfilePath()
    {
        var configState = ServiceLocator.Current
            .GetInstance<IConfigManager>()
            .GetConfigState();
        var lastLoadedPath = configState.LastLoadedProfile ?? GetDefaultPath();
        if (!File.Exists(lastLoadedPath))
        {
            ExtractDefaultIfNotExists();
            lastLoadedPath = GetDefaultPath();
        }

        return lastLoadedPath;
    }

    public static MappingProfile Load(string filePath, bool suppressMessageBox = false)
    {
        var options = GetSerializerOptions();
        MappingProfile profile;

        filePath = ResolveProfilePath(filePath);

        profile = JsonSerializer.Deserialize<MappingProfile>(File.ReadAllText(filePath), options);

        if (profile.PostLoad(filePath, suppressMessageBox))
        {
            return profile;
        }

        return null;
    }

    public static MappingProfile RestoreLastLoaded()
    {
        var lastLoadedPath = ResolveLastLoadedProfilePath();

        return Load(lastLoadedPath);
    }

    private static JsonSerializerOptions GetSerializerOptions()
    {
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
        var directory = Path.Combine(
            Output.GetAppDataDirectory(),
            SAVE_DIR);

        if (!string.IsNullOrWhiteSpace(directory)
            && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        return directory;
    }
}
