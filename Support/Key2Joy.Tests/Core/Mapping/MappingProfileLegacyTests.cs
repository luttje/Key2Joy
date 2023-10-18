using System.IO;
using CommonServiceLocator;
using Key2Joy.Config;
using Key2Joy.Mapping;
using Key2Joy.Mapping.Actions;
using Key2Joy.Mapping.Triggers;
using Key2Joy.Tests.Core.Config;
using Key2Joy.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Key2Joy.Tests.Core.Mapping;

[TestClass]
public class MappingProfileLegacyTests
{
    private DependencyServiceLocator serviceLocator;

    [TestInitialize]
    public void Initialize()
    {
        ActionsRepository.Buffer();
        TriggersRepository.Buffer();
        ExposedEnumerationRepository.Buffer();

        this.serviceLocator = new DependencyServiceLocator();
        ServiceLocator.SetLocatorProvider(() => this.serviceLocator);
    }

    [TestCleanup]
    public void Cleanup() => MockConfigManager.RemoveMappingProfiles();

    [TestMethod]
    public void Load_WhenCurrentMappingProfile_ShouldLoadCurrentMappingProfile()
    {
        var mappingProfilePath = MockConfigManager.GetMockMappingProfilePath("default-profile.k2j.json");
        MockConfigManager.CopyStub("current-default-profile.k2j.json", mappingProfilePath);
        this.serviceLocator.Register<IConfigManager>(MockConfigManager.LoadOrCreateMock());

        var mappingProfile = MappingProfile.Load(mappingProfilePath);

        Assert.IsNotNull(mappingProfile);
        Assert.AreEqual(6, mappingProfile.Version);
    }

    [TestMethod]
    public void Load_WhenOutdatedMappingProfile_MakeBackupAndUpdate()
    {
        var mappingProfilePath = MockConfigManager.GetMockMappingProfilePath("default-profile.k2j.json");
        var oldProfileContents = MockConfigManager.CopyStub("old-default-profile.k2j.json", mappingProfilePath);
        this.serviceLocator.Register<IConfigManager>(MockConfigManager.LoadOrCreateMock());

        var mappingProfile = MappingProfile.Load(mappingProfilePath, suppressMessageBox: true);

        Assert.IsNotNull(mappingProfile);
        Assert.AreEqual(6, mappingProfile.Version);

        // A backup should have been created
        var backupFileName = $"{mappingProfilePath}{MappingProfile.BACKUP_EXTENSION}1";
        Assert.IsTrue(File.Exists(backupFileName));
        Assert.AreEqual(oldProfileContents, File.ReadAllText(backupFileName));
        Assert.AreNotEqual(oldProfileContents, File.ReadAllText(mappingProfilePath));
    }
}
