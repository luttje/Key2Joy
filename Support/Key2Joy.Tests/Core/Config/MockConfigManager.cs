using System.IO;
using System.Text;
using Key2Joy.Config;
using Key2Joy.Mapping;

namespace Key2Joy.Tests.Core.Config;

public class MockConfigManager : ConfigManager
{
    private const string APP_DIR = "TestConfigs";

    public static MockConfigManager LoadOrCreateMock()
        => new();

    public MockConfigManager() : base()
    { }

    private static string GetMockAppDataDirectory()
    {
        var directory = APP_DIR;

        if (!string.IsNullOrWhiteSpace(directory)
            && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        return directory;
    }

    internal static string GetMockConfigPath()
        => Path.Combine(GetMockAppDataDirectory(), CONFIG_PATH);

    internal static string GetMockMappingProfilePath(string profileFileName = "")
        => Path.Combine(GetMockAppDataDirectory(), MappingProfile.SAVE_DIR, profileFileName);

    protected override string GetAppDataDirectory() => GetMockAppDataDirectory();

    private static string AdjustAndWriteContents(string contents, string targetPath)
    {
        var assemblyPath = Path.GetDirectoryName(typeof(Gui.Program).Assembly.Location);

        static string EscapePath(string path) => path.Replace("\\", "\\\\");

        contents = contents.Replace("%TEST_ASSEMBLY_PATH%", EscapePath(assemblyPath));
        contents = contents.Replace("%TEST_APP_DATA_PATH%", EscapePath(Path.Combine(assemblyPath, GetMockAppDataDirectory())));

        // We must trim the newline off the end, or else the Json Serializer will Save and not match the stub.
        contents = contents.TrimEnd('\r', '\n');

        File.WriteAllText(targetPath, contents);

        return contents;
    }

    /// <summary>
    /// Copies the specified stub to the config or mapping profile path.
    /// </summary>
    /// <returns>The content of the file</returns>
    public static string CopyStub(string stubPath, string targetPath)
    {
        stubPath = Path.Combine(
            nameof(Core),
            nameof(Config),
            "Stubs",
            stubPath
        );

        var directory = Path.GetDirectoryName(targetPath);

        if (!string.IsNullOrWhiteSpace(directory)
            && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var contents = File.ReadAllText(stubPath);

        return AdjustAndWriteContents(contents, targetPath);
    }

    /// <summary>
    /// Copies the current default profile to the target path.
    /// Gets it from the assembly's resources.
    /// </summary>
    /// <param name="targetPath"></param>
    /// <returns></returns>
    public static string CopyStubCurrentDefaultProfile(string targetPath)
    {
        var directory = Path.GetDirectoryName(targetPath);

        if (!string.IsNullOrWhiteSpace(directory)
            && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var fileContents = MappingProfile.GetDefaultProfileContents();
        var contents = Encoding.UTF8.GetString(fileContents);

        return AdjustAndWriteContents(contents, targetPath);
    }

    /// <summary>
    /// Removes the config file
    /// </summary>
    public static void RemoveConfigStub()
    {
        var configPath = GetMockConfigPath();

        if (File.Exists(configPath))
        {
            File.Delete(configPath);
        }
    }

    /// <summary>
    /// Removes the mapping profiles directory
    /// </summary>
    public static void RemoveMappingProfiles()
    {
        var mappingProfilePath = GetMockMappingProfilePath();

        if (Directory.Exists(mappingProfilePath))
        {
            Directory.Delete(mappingProfilePath, true);
        }
    }
}
