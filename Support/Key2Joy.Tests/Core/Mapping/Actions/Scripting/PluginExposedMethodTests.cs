using System.Linq;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Tests.PluginHost;
using Key2Joy.Tests.Stubs.TestPlugin;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Key2Joy.Tests.Core.Mapping.Actions.Scripting;

[TestClass]
public class PluginExposedMethodTests : PluginHostTestBase
{
    [TestInitialize]
    public override void Setup() => base.Setup();

    [TestCleanup]
    public override void Cleanup() => base.Cleanup();

    [TestMethod]
    public void Plugin_ScriptAction_CanGetParameterTypes()
    {
        var actionProxy = this.MakePluginAction<ExposedMethodsAction>();
        var exposedMethods = this.GetExposedMethodsForAction<ExposedMethodsAction>().ToList();
        var exposedMethod = exposedMethods[0];

        exposedMethod.Prepare(actionProxy);

        bool _;
        var parameterTypes = exposedMethod.GetParameterTypes(out _).ToList();

        CollectionAssert.AreEqual(parameterTypes, new[] { typeof(string), typeof(int[]) });
    }

    [TestMethod]
    public void Plugin_ScriptAction_CanInvoke()
    {
        var actionProxy = this.MakePluginAction<ExposedMethodsAction>();
        var exposedMethods = this.GetExposedMethodsForAction<ExposedMethodsAction>().ToList();
        var exposedMethod = exposedMethods.First(e => e.FunctionName == "simpleConcat");

        exposedMethod.Prepare(actionProxy);

        var result = (string)exposedMethod.TransformAndRedirect("test", new object[] { 1, 2, 3 });

        Assert.AreEqual(result, ExposedMethodsAction.Concat("test", new int[] { 1, 2, 3 }));
    }

    [TestMethod]
    public void Plugin_ScriptAction_CanInvokeEnum()
    {
        var actionProxy = this.MakePluginAction<ExposedMethodsAction>();
        var exposedMethods = this.GetExposedMethodsForAction<ExposedMethodsAction>().ToList();
        var exposedMethod = exposedMethods.First(e => e.FunctionName == "isUnlessTopLevel");

        exposedMethod.Prepare(actionProxy);

        var result = (bool)exposedMethod.TransformAndRedirect((int)MappingMenuVisibility.UnlessTopLevel);
        var result2 = (bool)exposedMethod.TransformAndRedirect(nameof(MappingMenuVisibility.UnlessTopLevel));

        var result3 = (bool)exposedMethod.TransformAndRedirect((int)MappingMenuVisibility.Always);
        var result4 = (bool)exposedMethod.TransformAndRedirect(nameof(MappingMenuVisibility.Always));

        Assert.IsTrue(result);
        Assert.IsTrue(result2);

        Assert.IsFalse(result3);
        Assert.IsFalse(result4);
    }

    [TestMethod]
    public void Plugin_ScriptAction_CanInvokeObjectArrayAndGetTransformedIntArray()
    {
        var actionProxy = this.MakePluginAction<ExposedMethodsAction>();
        var exposedMethods = this.GetExposedMethodsForAction<ExposedMethodsAction>().ToList();
        var exposedMethod = exposedMethods.First(e => e.FunctionName == "returnsNumbersInput");

        exposedMethod.Prepare(actionProxy);

        var result = (int[])exposedMethod.TransformAndRedirect(new object[] { 3, 2, 1 });

        Assert.IsInstanceOfType<int[]>(result);
        CollectionAssert.AreEqual(result, new[] { 3, 2, 1 });
    }
}
