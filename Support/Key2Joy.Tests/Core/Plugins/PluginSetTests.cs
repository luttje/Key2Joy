using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Key2Joy.Plugins;
using CommonServiceLocator;
using Key2Joy.Config;
using Key2Joy.Tests.Core.Config;
using Key2Joy.Util;

namespace Key2Joy.Tests.Core.Plugins;

[TestClass]
public class PluginSetTests
{
    private string testPluginDirectory;
    private MockConfigManager configManager;

    [TestInitialize]
    public void TestInitialize()
    {
        // Setup dependency injection and config manager service locator
        var serviceLocator = new DependencyServiceLocator();
        ServiceLocator.SetLocatorProvider(() => serviceLocator);

        var configContents = MockConfigManager.CopyStub("current-config.json", MockConfigManager.GetMockConfigPath());
        this.configManager = MockConfigManager.LoadOrCreateMock();
        serviceLocator.Register<IConfigManager>(this.configManager);

        this.testPluginDirectory = Path.Combine(
            Path.GetDirectoryName(this.configManager.GetConfigState().LastInstallPath),
            "Plugins"
        );
    }

    /// <summary>
    /// Enable the stub plugin, optionally with a custom checksum
    /// </summary>
    /// <param name="checksum"></param>
    private string EnableStubPlugin(string checksum = null)
    {
        // Enable the stub plugin
        var pluginAssemblyPath = Path.Combine(
            "Plugins",
            "Key2Joy.Tests.Stubs.TestPlugin",
            "Key2Joy.Tests.Stubs.TestPlugin.dll"
        );
        var fullPluginAssemblyPath = Path.Combine(
            this.testPluginDirectory,
            "Key2Joy.Tests.Stubs.TestPlugin",
            "Key2Joy.Tests.Stubs.TestPlugin.dll"
        );

        if (checksum == null)
        {
            var permissionsXml = Key2Joy.PluginHost.PluginHost.GetAdditionalPermissionsXml(pluginAssemblyPath);
            checksum = Key2Joy.PluginHost.PluginHost.CalculatePermissionsChecksum(permissionsXml);
        }

        this.configManager.SetPluginEnabled(
            pluginAssemblyPath,
            checksum
        );

        return fullPluginAssemblyPath;
    }

    [TestMethod]
    public void Constructor_SetsPluginFolderProperty()
    {
        var pluginSet = new PluginSet(this.testPluginDirectory);

        Assert.AreEqual(this.testPluginDirectory, pluginSet.PluginsFolder);
    }

    [TestMethod]
    public void LoadAll_LoadsEnabledPlugins()
    {
        var fullPluginAssemblyPath = this.EnableStubPlugin();

        var pluginSet = new PluginSet(this.testPluginDirectory);
        pluginSet.LoadAll();

        var loadedPlugins = pluginSet.AllPluginLoadStates;
        var loadedPlugin = loadedPlugins[fullPluginAssemblyPath];

        Assert.AreEqual(PluginLoadStates.Loaded, loadedPlugin.LoadState);
    }

    // TODO:
    //[TestMethod]
    //public void LoadAll_SkipsDisabledPlugins()
    //{ }

    [TestMethod]
    public void LoadAll_FailsLoadingPluginWithDifferentPermissionsChecksum()
    {
        var fullPluginAssemblyPath = this.EnableStubPlugin("invalid-checksum");

        var pluginSet = new PluginSet(this.testPluginDirectory);
        pluginSet.LoadAll();

        var loadedPlugins = pluginSet.AllPluginLoadStates;
        var loadedPlugin = loadedPlugins[fullPluginAssemblyPath];

        Assert.AreEqual(PluginLoadStates.FailedToLoad, loadedPlugin.LoadState);
    }

    [TestMethod]
    public void DisablePlugin_DisablesSpecifiedPlugin()
    {
        var fullPluginAssemblyPath = this.EnableStubPlugin();

        var pluginSet = new PluginSet(this.testPluginDirectory);
        pluginSet.LoadAll();

        var loadedPlugins = pluginSet.AllPluginLoadStates;
        var loadedPlugin = loadedPlugins[fullPluginAssemblyPath];

        pluginSet.DisablePlugin(fullPluginAssemblyPath);

        Assert.IsFalse(this.configManager.IsPluginEnabled(fullPluginAssemblyPath));

        // TODO: Add assertions to verify that the specified plugin was fully unloaded (not implemented yet)
        //Assert.AreEqual(PluginLoadStates.NotLoaded, loadedPlugin.LoadState);
    }

    // TODO:
    //[TestMethod]
    //public void Dispose_DisposesLoadedPlugins()
    //{
    //    var pluginSet = new PluginSet(this.testPluginDirectory);

    //    // TODO: Load some plugins to test disposing.

    //    pluginSet.Dispose();

    //    // TODO: Add assertions to verify that the plugins have been disposed.
    //}
}
