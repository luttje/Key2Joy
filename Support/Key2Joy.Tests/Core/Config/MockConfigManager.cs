using System.IO;
using Key2Joy.Config;

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

    protected override string GetAppDataDirectory() => GetMockAppDataDirectory();

    /// <summary>
    /// Copies the specified config stub to the config path.
    /// </summary>
    /// <returns>The content of the file</returns>
    public static string CopyConfigStub(string stubPath)
    {
        var configPath = GetMockConfigPath();

        stubPath = Path.Combine(
            nameof(Core),
            nameof(Core.Config),
            "Stubs",
            stubPath
        );

        File.Copy(stubPath, configPath, true);

        return File.ReadAllText(configPath);
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
}
