using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Key2Joy.Tests.Core.Config;

[TestClass]
public class ConfigManagerTest
{
    [TestCleanup]
    public void Cleanup() => MockConfigManager.RemoveConfigStub();

    [TestMethod]
    public void LoadOrCreate_WhenConfigFileDoesNotExist_CreatesConfigFile()
    {
        var configManager = MockConfigManager.LoadOrCreateMock();

        Assert.IsTrue(configManager.IsInitialized);
        Assert.IsTrue(File.Exists(MockConfigManager.GetMockConfigPath()));
    }

    [TestMethod]
    public void LoadOrCreate_WhenConfigFileExists_LoadsConfigFile()
    {
        var configContents = MockConfigManager.CopyStub("current-config.json", MockConfigManager.GetMockConfigPath());
        var configManager = MockConfigManager.LoadOrCreateMock();

        Assert.IsTrue(configManager.IsInitialized);
        Assert.IsTrue(File.Exists(MockConfigManager.GetMockConfigPath()));
        Assert.AreEqual(configContents, File.ReadAllText(MockConfigManager.GetMockConfigPath()));
    }

    [TestMethod]
    public void LoadOrCreate_WhenOldConfigFileExists_GracefullyHandles()
    {
        var configContents = MockConfigManager.CopyStub("old-config.json", MockConfigManager.GetMockConfigPath());
        var configManager = MockConfigManager.LoadOrCreateMock();

        Assert.IsTrue(configManager.IsInitialized);
        Assert.IsTrue(File.Exists(MockConfigManager.GetMockConfigPath()));
        Assert.AreNotEqual(configContents, File.ReadAllText(MockConfigManager.GetMockConfigPath()));
    }
}
