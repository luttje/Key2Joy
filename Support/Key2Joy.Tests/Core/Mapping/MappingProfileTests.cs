using System.IO;
using CommonServiceLocator;
using Key2Joy.Config;
using Key2Joy.Mapping;
using Key2Joy.Tests.Core.Config;
using Key2Joy.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Key2Joy.Tests.Core.Mapping;

[TestClass]
public class MappingProfileTests
{
    private const string TestExtension = ".k2j-test.json";
    private MockConfigManager configManager;

    [TestInitialize]
    public void Initialize()
    {
        // Setup dependency injection and config manager service locator
        var serviceLocator = new DependencyServiceLocator();
        ServiceLocator.SetLocatorProvider(() => serviceLocator);

        this.configManager = MockConfigManager.LoadOrCreateMock();
        serviceLocator.Register<IConfigManager>(this.configManager);
    }

    [TestCleanup]
    public void Cleanup() => MockConfigManager.RemoveConfigStub();

    [TestMethod]
    public void ResolveLastLoadedProfilePath_WhenLastLoadedProfileIsNull_ShouldReturnDefaultPath()
    {
        this.configManager.GetConfigState().LastLoadedProfile = null;

        var result = MappingProfile.ResolveLastLoadedProfilePath();

        Assert.AreEqual(MappingProfile.GetDefaultPath(), result);
    }

    [TestMethod]
    public void ResolveLastLoadedProfilePath_WhenLastLoadedProfileDoesNotExist_ShouldExtractDefaultAndReturnDefaultPath()
    {
        this.configManager.GetConfigState().LastLoadedProfile = "NonExistentPath";

        var result = MappingProfile.ResolveLastLoadedProfilePath();

        Assert.AreEqual(MappingProfile.GetDefaultPath(), result);
        Assert.IsTrue(File.Exists(MappingProfile.GetDefaultPath())); // Ensure default path is extracted
    }

    [TestMethod]
    public void ResolveLastLoadedProfilePath_WhenLastLoadedProfileExists_ShouldReturnLastLoadedPath()
    {
        var fakePath = "FakePath";
        this.configManager.GetConfigState().LastLoadedProfile = fakePath;
        File.Create(fakePath).Dispose(); // Create a fake file

        var result = MappingProfile.ResolveLastLoadedProfilePath();

        Assert.AreEqual(fakePath, result);

        File.Delete(fakePath);
    }

    [TestMethod]
    public void ResolveProfilePath_FileExists_ReturnsFilePath()
    {
        var fakePath = $"existingFile.{TestExtension}";
        File.Create(fakePath).Dispose(); // Create a fake file

        var resolvedPath = MappingProfile.ResolveProfilePath(fakePath);

        Assert.AreEqual(fakePath, resolvedPath);

        File.Delete(fakePath);
    }

    [TestMethod]
    public void ResolveProfilePath_FileDoesNotExist_ReturnsNull()
    {
        var fakePath = $"existingFile.{TestExtension}";
        var resolvedPath = MappingProfile.ResolveProfilePath(fakePath);

        Assert.IsNull(resolvedPath);
    }
}
