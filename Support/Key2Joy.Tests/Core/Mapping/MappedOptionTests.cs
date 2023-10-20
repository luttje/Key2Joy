using Key2Joy.LowLevelInput;
using Key2Joy.Mapping;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Contracts.Mapping.Triggers;

namespace Key2Joy.Tests.Core.Mapping;

public class MockAction : AbstractAction
{
    public MockAction()
        : base("MockAction")
    { }

    public MockAction(string name)
        : base(name)
    { }
}

public class MockTrigger : AbstractTrigger
{
    public MockTrigger()
        : base("MockTrigger")
    { }

    public MockTrigger(string name)
        : base(name)
    { }

    public override AbstractTriggerListener GetTriggerListener() => throw new System.NotImplementedException();
}

public class MockPressStateAction : MockAction, IPressState
{
    public MockPressStateAction()
        : base("MockPressStateAction")
    { }

    public MockPressStateAction(string name)
        : base(name)
    { }

    public PressState PressState { get; set; }
}

public class MockPressStateTrigger : MockTrigger, IPressState
{
    public MockPressStateTrigger()
        : base("MockPressStateTrigger")
    { }

    public MockPressStateTrigger(string name)
        : base(name)
    { }

    public PressState PressState { get; set; }
}

[TestClass]
public class MappedOptionTests
{
    // Tests the GenerateOppositePressStateMappings for action with IPressState
    [TestMethod]
    public void GenerateOppositePressStateMappings_ActionWithPressState_ChangesPressState()
    {
        var option = new MappedOption
        {
            Action = new MockPressStateAction { PressState = PressState.Press },
            Trigger = new MockTrigger()
        };

        var mappings = MappedOption.GenerateOppositePressStateMappings(new List<MappedOption> { option });

        Assert.AreEqual(PressState.Release, ((IPressState)mappings[0].Action).PressState);
    }

    // Tests the GenerateOppositePressStateMappings for trigger with IPressState
    [TestMethod]
    public void GenerateOppositePressStateMappings_TriggerWithPressState_ChangesPressState()
    {
        var option = new MappedOption
        {
            Action = new MockAction(),
            Trigger = new MockPressStateTrigger { PressState = PressState.Press }
        };

        var mappings = MappedOption.GenerateOppositePressStateMappings(new List<MappedOption> { option });

        Assert.AreEqual(PressState.Release, ((IPressState)mappings[0].Trigger).PressState);
    }

    // Ensures that the new mappings are separate instances
    [TestMethod]
    public void GenerateOppositePressStateMappings_ReturnsNewInstance()
    {
        var option = new MappedOption
        {
            Action = new MockAction(),
            Trigger = new MockTrigger()
        };

        var mappings = MappedOption.GenerateOppositePressStateMappings(new List<MappedOption> { option });

        Assert.AreNotSame(option, mappings[0]);
        Assert.AreNotSame(option.Action, mappings[0].Action);
        Assert.AreNotSame(option.Trigger, mappings[0].Trigger);
    }

    // Tests the method with an empty list, expecting another empty list in return
    [TestMethod]
    public void GenerateOppositePressStateMappings_WithEmptyList_ReturnsEmptyList()
    {
        var mappings = MappedOption.GenerateOppositePressStateMappings(new List<MappedOption>());

        Assert.AreEqual(0, mappings.Count);
    }
}
