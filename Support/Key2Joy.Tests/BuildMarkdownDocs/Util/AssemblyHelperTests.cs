using System.IO;
using BuildMarkdownDocs.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Key2Joy.Tests.BuildMarkdownDocs.Util;

[TestClass]
public class AssemblyHelperTests
{
    private const string TestDirectoryPath = "mockDirectory";
    private static readonly string TestPluginDirectoryPath = Path.Combine(TestDirectoryPath, "Plugins", "mockPluginAssembly");

    [TestInitialize]
    public void Initialize()
    {
        if (!Directory.Exists(TestDirectoryPath))
        {
            Directory.CreateDirectory(TestDirectoryPath);
        }

        if (!Directory.Exists(TestPluginDirectoryPath))
        {
            Directory.CreateDirectory(TestPluginDirectoryPath);
        }

        File.WriteAllText(Path.Combine(TestDirectoryPath, "mockAssembly.dll"), "this content doesn't matter, since we wont load it");
        File.WriteAllText(Path.Combine(TestPluginDirectoryPath, "mockPluginAssembly.dll"), "this content doesn't matter, since we wont load it");
    }

    [TestCleanup]
    public void Cleanup()
    {
        if (Directory.Exists(TestDirectoryPath))
        {
            Directory.Delete(TestDirectoryPath, true);
        }
    }

    [TestMethod]
    public void DetermineAssemblyPath_MainDirectory_ReturnsMainPath()
    {
        var helper = new AssemblyHelper(TestDirectoryPath, "mockAssembly");
        var result = helper.DetermineAssemblyPath();
        Assert.AreEqual(Path.Combine(TestDirectoryPath, "mockAssembly.dll"), result);
    }

    [TestMethod]
    public void DetermineAssemblyPath_PluginsDirectory_ReturnsPluginPath()
    {
        var helper = new AssemblyHelper(TestDirectoryPath, "mockPluginAssembly");
        var result = helper.DetermineAssemblyPath();
        Assert.AreEqual(Path.Combine(TestPluginDirectoryPath, "mockPluginAssembly.dll"), result);
        Assert.IsTrue(helper.IsPlugin);
    }

    [TestMethod]
    [ExpectedException(typeof(FileNotFoundException))]
    public void DetermineAssemblyPath_NotFound_ThrowsException()
    {
        var helper = new AssemblyHelper(TestDirectoryPath, "nonExistentAssembly");
        helper.DetermineAssemblyPath();
    }
}
