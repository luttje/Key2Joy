using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Key2Joy.Tests.PluginHost;

[TestClass]
public class PluginHostCommonTests : PluginHostTestBase
{
    [TestInitialize]
    public override void Setup() => base.Setup();

    [TestCleanup]
    public override void Cleanup() => base.Cleanup();

    [TestMethod]
    public void Plugin_CanLoad()
    {
        Assert.IsNotNull(this.pluginHost);
        Assert.IsTrue(this.pluginActionFactories.Count() > 0);
        // TODO: Assert.IsTrue(pluginTriggerFactories.Count() > 0);
    }

    [TestMethod]
    public void Plugin_Action_CanExecute()
    {
        var actionProxy = this.MakePluginAction<Stubs.TestPlugin.CommonAction>();
        var methodName = nameof(Stubs.TestPlugin.CommonAction.CallWithoutParameters);

        actionProxy.InvokeScriptMethod(methodName, new object[0]);

        Assert.IsTrue((string)actionProxy.GetPublicPropertyValue(nameof(Stubs.TestPlugin.CommonAction.LastCalledName)) == methodName);
    }

    [TestMethod]
    public void Plugin_Action_CanExecute_WithParameters()
    {
        var actionProxy = this.MakePluginAction<Stubs.TestPlugin.CommonAction>();
        var methodName = nameof(Stubs.TestPlugin.CommonAction.CallWithParameter);

        actionProxy.InvokeScriptMethod(methodName, new object[] { "World" });

        Assert.IsTrue((string)actionProxy.GetPublicPropertyValue(nameof(Stubs.TestPlugin.CommonAction.LastCalledName)) == methodName);
    }

    [TestMethod]
    public void Plugin_Action_CanExecute_WithParametersAndReturn()
    {
        var actionProxy = this.MakePluginAction<Stubs.TestPlugin.CommonAction>();
        var methodName = nameof(Stubs.TestPlugin.CommonAction.CallWithParameterAndReturn_DoubleInput);

        var doubleOutput = (string)actionProxy.InvokeScriptMethod(methodName, new object[] { "World" });

        Assert.IsTrue((string)actionProxy.GetPublicPropertyValue(nameof(Stubs.TestPlugin.CommonAction.LastCalledName)) == methodName);
        Assert.IsTrue(doubleOutput == "WorldWorld");
    }
}
