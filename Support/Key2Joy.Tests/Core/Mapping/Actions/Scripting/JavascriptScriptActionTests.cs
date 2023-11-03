using Jint;
using Key2Joy.Contracts.Plugins;
using Key2Joy.Mapping.Actions;
using Key2Joy.Mapping;
using Key2Joy.Mapping.Actions.Scripting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Key2Joy.Contracts.Mapping.Actions;

namespace Key2Joy.Tests.Core.Mapping.Actions.Scripting;

public class JavascriptMockType : AbstractAction
{
    public JavascriptMockType() : base(string.Empty)
    { }

    public void MethodWithFunctionParameter(CallbackAction callback, params object[] args)
        => callback(args);
}

internal class TestJavascriptScriptAction : JavascriptAction
{
    public TestJavascriptScriptAction(string name) : base(name)
    {
    }

    public Engine GetEnvironment() => this.Environment;
}

[TestClass]
public class JavascriptScriptActionTests
{
    [TestMethod]
    public void Test_TypeExposedMethod_CanReceiveJavascriptFunction()
    {
        ActionsRepository.Buffer();
        ExposedEnumerationRepository.Buffer();

        var javascriptScriptAction = new TestJavascriptScriptAction("TestAction");
        var javascript = javascriptScriptAction.SetupEnvironment();

        var exposedMethod = new TypeExposedMethod("functionName", nameof(JavascriptMockType.MethodWithFunctionParameter), typeof(JavascriptMockType));

        var instance = new JavascriptMockType();
        exposedMethod.Prepare(instance);
        javascriptScriptAction.RegisterScriptingMethod(
                    exposedMethod,
                    instance);

        javascript.Execute("var funcResult; functionName(function(){ funcResult = 1337 });");

        var actual = javascript.GetValue("funcResult");
        Assert.AreEqual((double)1337, actual);
    }

    [TestMethod]
    public void Test_TypeExposedMethod_CanReceiveJavascriptFunctionWithParams()
    {
        ActionsRepository.Buffer();
        ExposedEnumerationRepository.Buffer();

        var javascriptScriptAction = new TestJavascriptScriptAction("TestAction");
        var javascript = javascriptScriptAction.SetupEnvironment();

        var exposedMethod = new TypeExposedMethod("functionName", nameof(JavascriptMockType.MethodWithFunctionParameter), typeof(JavascriptMockType));

        var instance = new JavascriptMockType();
        exposedMethod.Prepare(instance);
        javascriptScriptAction.RegisterScriptingMethod(
                    exposedMethod,
                    instance);

        javascript.Execute("var funcResult; functionName(function(a,b,c,d){ funcResult = `${a}${b}${c}${d}` }, 1, 3, 3, 7)");

        var actual = javascript.GetValue("funcResult");
        Assert.AreEqual("1337", actual);
    }
}
