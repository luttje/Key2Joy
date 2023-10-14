using System.IO;
using Key2Joy.Config;
using Key2Joy.Mapping;

namespace Key2Joy.Tests.Core.Config;

public class MockConfigManager : ConfigManager
{
    private const string APP_DIR = "TestConfigs";

    public static MockConfigManager LoadOrCreateMock()
        => (MockConfigManager)LoadOrCreate(new MockConfigManager());

    private static string GetMockAppDataDirectory()
    {
        var directory = APP_DIR;

        if (!Directory.Exists(directory))
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

    /// <summary>
    /// Copies the specified stub to the config or mapping profile path.
    /// </summary>
    /// <returns>The content of the file</returns>
    public static string CopyStub(string stubPath, string targetPath)
    {
        stubPath = Path.Combine(
            nameof(Core),
            nameof(Core.Config),
            "Stubs",
            stubPath
        );

        var directory = Path.GetDirectoryName(targetPath);

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var contents = File.ReadAllText(stubPath);
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
