using System.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Key2Joy.Tests.PluginHost;

[TestClass]
public class PluginHostRegistryAccessTests : PluginHostTestBase
{
    private const string KeyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";

    [TestInitialize]
    public override void Setup() => base.Setup();

    [TestCleanup]
    public override void Cleanup() => base.Cleanup();

    [TestMethod]
    [ExpectedException(typeof(SecurityException))]
    public void Plugin_Action_CannotExecute_ReadPublicRegistry()
    {
        var actionProxy = this.MakePluginAction<Stubs.TestPlugin.RegistryAccessAction>();
        var subKeyNames = nameof(Stubs.TestPlugin.RegistryAccessAction.ReadRegistrySubKeyNames);

        actionProxy.InvokeScriptMethod(subKeyNames, new object[] { KeyPath });
    }
}
