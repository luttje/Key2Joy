using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Contracts.Mapping.Triggers;
using Key2Joy.Mapping;
using Key2Joy.Mapping.Actions;
using Key2Joy.Mapping.Actions.Scripting;
using Key2Joy.Mapping.Triggers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Key2Joy.Tests.Core.Mapping.Actions.Scripting;

internal class TestInputBag : AbstractInputBag
{
}

internal class TestTriggerListener : AbstractTriggerListener
{
    public override void AddMappedOption(AbstractMappedOption mappedOption) => throw new NotImplementedException();

    public override bool GetIsTriggered(AbstractTrigger trigger) => throw new NotImplementedException();

    public override void StartListening(ref IList<AbstractTriggerListener> allListeners) => throw new NotImplementedException();

    public override void StopListening() => throw new NotImplementedException();
}

public struct ExposedMethodWithAction
{
    public ExposedMethod ExposedMethod { get; set; }
    public AbstractAction Action { get; set; }
}

public class MockEnvironment : IDisposable
{
    public List<ExposedEnumeration> ExposedEnumerations = new();
    public List<ExposedMethodWithAction> ExposedMethodsWithActions = new();

    public void Dispose()
    { }
}

public class TestScriptActionWithEnvironment : BaseScriptActionWithEnvironment<MockEnvironment>
{
    public TestScriptActionWithEnvironment()
        : base("TestAction")
    { }

    public TestScriptActionWithEnvironment(string name, string script)
        : base(name)
        => this.Script = script;

    public override MockEnvironment MakeEnvironment()
        => new();

    public override void RegisterScriptingEnum(ExposedEnumeration enumeration)
        => this.Environment.ExposedEnumerations.Add(enumeration);

    public override void RegisterScriptingMethod(ExposedMethod exposedMethod, AbstractAction scriptActionInstance)
        => this.Environment.ExposedMethodsWithActions.Add(new ExposedMethodWithAction()
        {
            ExposedMethod = exposedMethod,
            Action = scriptActionInstance
        });

    public MockEnvironment GetEnvironment()
            => this.Environment;
}

[TestClass]
public class BaseScriptActionWithEnvironmentTests
{
    [TestInitialize]
    public void Initialize()
    {
        ActionsRepository.Buffer();
        TriggersRepository.Buffer();
        ExposedEnumerationRepository.Buffer();
    }

    [TestMethod]
    public async Task SetupEnvironment_ReturnsEnvironment()
    {
        var mockAction = new Mock<TestScriptActionWithEnvironment>
        {
            CallBase = true
        };

        var environment = mockAction.Object.SetupEnvironment();

        Assert.IsNotNull(environment);
    }

    [TestMethod]
    public async Task RetireEnvironment_SetsIsRetiredToTrue()
    {
        var mockAction = new Mock<TestScriptActionWithEnvironment>
        {
            CallBase = true
        };

        mockAction.Object.RetireEnvironment();

        Assert.IsTrue(mockAction.Object.IsRetired);
    }

    [TestMethod]
    public void MakeEnvironment_ReusesExistingEnvironments()
    {
        var mockAction = new Mock<TestScriptActionWithEnvironment>() { CallBase = true };
        mockAction.Setup(a => a.MakeEnvironment()).Returns(new MockEnvironment());

        var previousEnvironment = mockAction.Object.SetupEnvironment();
        var nextEnvironment = mockAction.Object.SetupEnvironment();

        Assert.AreEqual(previousEnvironment, nextEnvironment);
    }

    [TestMethod]
    public async Task Execute_ReactivatesRetiredEnvironment()
    {
        var mockAction = new Mock<TestScriptActionWithEnvironment>
        {
            CallBase = true
        };
        mockAction.Object.RetireEnvironment();

        await mockAction.Object.Execute(new TestInputBag());

        Assert.IsFalse(mockAction.Object.IsRetired);
    }

    [TestMethod]
    public void OnStartListening_SetsEnvironmentFromExistingScriptAction()
    {
        var firstAction = new Mock<TestScriptActionWithEnvironment>(
            "FirstAction",
            @"var first = 1;"
        );
        var secondAction = new Mock<TestScriptActionWithEnvironment>(
            "SecondAction",
            @"var second = 2;"
        );

        IList<AbstractAction> allActions = new List<AbstractAction> {
            firstAction.Object,
            secondAction.Object
        };

        secondAction.Object.OnStartListening(new TestTriggerListener(), ref allActions);

        Assert.AreEqual(
            firstAction.Object.GetEnvironment(),
            secondAction.Object.GetEnvironment()
        );
        // Even though the environment is the same, the script action scripts remain separate.
        Assert.AreNotEqual(
            firstAction.Object.Script,
            secondAction.Object.Script
        );
    }
}
