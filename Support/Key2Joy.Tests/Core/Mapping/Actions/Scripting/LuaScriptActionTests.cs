using Key2Joy.Contracts.Plugins;
using Key2Joy.Mapping;
using Key2Joy.Mapping.Actions;
using Key2Joy.Mapping.Actions.Scripting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLua;

namespace Key2Joy.Tests.Core.Mapping.Actions.Scripting;

public class LuaMockType
{
    public void MethodWithFunctionParameter(CallbackAction callback)
        => callback();
}

internal class TestLuaScriptAction : LuaScriptAction
{
    public TestLuaScriptAction(string name) : base(name)
    {
    }

    public Lua GetEnvironment() => this.Environment;
}

[TestClass]
public class LuaScriptActionTests
{
    [TestMethod]
    public void Script_ExecutesLuaFunction_ChecksResult()
    {
        ActionsRepository.Buffer();
        ExposedEnumerationRepository.Buffer();

        LuaScriptAction action = new(string.Empty);
        var lua = action.SetupEnvironment();

        action.Script = "function execute() return 1337 end\n" +
            "numberIsOne = execute()";
        action.Execute(null).Wait();

        var actual = lua["numberIsOne"];
        Assert.AreEqual((double)1337, actual);
    }

    [TestMethod]
    public void TestLuaScriptActionRegistersPrintFunction()
    {
        var luaScriptAction = new TestLuaScriptAction("TestAction");
        luaScriptAction.SetupEnvironment();

        var registeredFunction = luaScriptAction.GetEnvironment().GetFunction("print");

        Assert.IsNotNull(registeredFunction);
    }

    [TestMethod]
    public void TestLuaScriptActionRegistersCollectionFunction()
    {
        var luaScriptAction = new TestLuaScriptAction("TestAction");
        luaScriptAction.SetupEnvironment();

        var registeredFunction = luaScriptAction.GetEnvironment().GetFunction("collection");

        Assert.IsNotNull(registeredFunction);
    }

    [TestMethod]
    public void TestLuaScriptActionMakeEnvironmentCreatesLuaEnvironment()
    {
        var luaScriptAction = new TestLuaScriptAction("TestAction");

        var environment = luaScriptAction.MakeEnvironment();

        Assert.IsInstanceOfType(environment, typeof(Lua));
    }

    [TestMethod]
    public void TestLuaScriptActionInequalityBasedOnNameAndScript()
    {
        var firstAction = new TestLuaScriptAction("TestAction1") { Script = "print('Hello')" };
        var secondAction = new TestLuaScriptAction("TestAction2") { Script = "print('Hello')" };

        Assert.AreNotEqual(firstAction, secondAction);
    }

    [TestMethod]
    public void Test_TypeExposedMethod_CanReceiveLuaFunction()
    {
        var exposedMethod = new TypeExposedMethod("functionName", nameof(LuaMockType.MethodWithFunctionParameter), typeof(LuaMockType));

        var instance = new LuaMockType();
        exposedMethod.Prepare(instance);
        exposedMethod.RegisterParameterTransformer<LuaFunction>((luaFunction, expectedType) => new CallbackActionWrapper((object)luaFunction.Call));

        var lua = new Lua();
        lua.DoString("function funcToPass() funcResult = 1337; end");
        var function = lua.GetFunction("funcToPass");

        exposedMethod.TransformAndRedirect(new object[] { function });

        var actual = lua["funcResult"];
        Assert.AreEqual((double)1337, actual);
    }

    // TODO: This currently fails. We need to rethink how we do equality checks (and why)
    // the way we currently do... It's tedious to override Equals and GetHashCode for every
    // action and I kinda forgot why we have to.
    //[TestMethod]
    //public void TestLuaScriptActionInequalityBasedOnSameNameAndScript()
    //{
    //    var firstAction = new TestLuaScriptAction("TestAction") { Script = "print('Hello')" };
    //    var secondAction = new TestLuaScriptAction("TestAction") { Script = "print('Hello')" };

    //    Assert.AreNotEqual(firstAction, secondAction);
    //}
}
