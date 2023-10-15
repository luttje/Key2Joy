using System.IO;
using System.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Key2Joy.Tests.PluginHost;

[TestClass]
public class PluginHostFileAccessTests : PluginHostTestBase
{
    // We must go up one level, since the pluginDirectory is the same as for this test. (Because it's referenced)
    private string GetPathToRead() => Path.Combine(Directory.GetCurrentDirectory() + @"\..\Key2Joy.Tests.PluginHost.file.txt");

    [TestInitialize]
    public override void Setup()
    {
        base.Setup();

        File.WriteAllText(this.GetPathToRead(), "Hello World!");
    }

    [TestCleanup]
    public override void Cleanup() => base.Cleanup();

    [TestMethod]
    public void Plugin_Action_CanExecute_WriteToData()
    {
        var actionProxy = this.MakePluginAction<Stubs.TestPlugin.FileAccessAction>();
        var methodName = nameof(Stubs.TestPlugin.FileAccessAction.WriteToDataDirectory);

        actionProxy.InvokeScriptMethod(methodName, new object[0]);
    }

    [TestMethod]
    [ExpectedException(typeof(SecurityException))]
    public void Plugin_Action_CannotExecute_WriteOutsideData()
    {
        var actionProxy = this.MakePluginAction<Stubs.TestPlugin.FileAccessAction>();
        var methodName = nameof(Stubs.TestPlugin.FileAccessAction.WriteToDataDirectoryWithUnsafePath);

        actionProxy.InvokeScriptMethod(methodName, new object[] { "../" });

        Assert.Fail();
    }

    [TestMethod]
    [ExpectedException(typeof(SecurityException))]
    public void Plugin_Action_CannotExecute_WriteToAbsolutePath()
    {
        var actionProxy = this.MakePluginAction<Stubs.TestPlugin.FileAccessAction>();
        var methodName = nameof(Stubs.TestPlugin.FileAccessAction.WriteToAbsolutePath);

        actionProxy.InvokeScriptMethod(methodName, new object[] { @"C:\Users\Public\also-unsafe.txt" });

        Assert.Fail();
    }

    [TestMethod]
    [ExpectedException(typeof(SecurityException))]
    public void Plugin_Action_CannotExecute_ReadAbsolutePath()
    {
        var actionProxy = this.MakePluginAction<Stubs.TestPlugin.FileAccessAction>();
        var methodName = nameof(Stubs.TestPlugin.FileAccessAction.ReadAbsolutePath);

        var test = this.GetPathToRead();
        var contents = actionProxy.InvokeScriptMethod(methodName, new object[] { test });

        Assert.Fail();
    }
}
