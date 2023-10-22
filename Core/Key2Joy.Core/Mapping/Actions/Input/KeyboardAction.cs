using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Contracts.Mapping.Triggers;
using Key2Joy.LowLevelInput;

namespace Key2Joy.Mapping.Actions.Input;

[Action(
    Description = "Keyboard Simulation",
    NameFormat = "{1} {0} on Keyboard",
    GroupName = "Keyboard Simulation",
    GroupImage = "keyboard"
)]
public class KeyboardAction : CoreAction, IPressState, IProvideReverseAspect
{
    public KeyboardKey Key { get; set; }
    public PressState PressState { get; set; }

    public KeyboardAction(string name)
        : base(name)
    {
    }

    /// <inheritdoc/>
    public void MakeReverse(AbstractMappingAspect aspect)
        => CommonReverseAspect.MakeReversePressState(this, aspect);

    public static KeyboardKey[] GetAllKeys()
    {
        var allEnums = Enum.GetValues(typeof(KeyboardKey));
        List<KeyboardKey> keys = new();

        // Skip the enumerations that are zero
        foreach (var keyEnum in allEnums)
        {
            if ((short)keyEnum == 0)
            {
                continue;
            }

            keys.Add((KeyboardKey)keyEnum);
        }

        return keys.ToArray();
    }

    public static List<MappedOption> GetAllButtonActions(PressState pressState)
    {
        var actionFactory = ActionsRepository.GetAction(typeof(KeyboardAction));

        List<MappedOption> actions = new();
        foreach (var key in GetAllKeys())
        {
            var action = (KeyboardAction)MakeAction(actionFactory);
            action.Key = key;
            action.PressState = pressState;

            actions.Add(new MappedOption
            {
                Action = action
            });
        }
        return actions;
    }

    /// <markdown-doc>
    /// <parent-name>Input</parent-name>
    /// <path>Api/Input</path>
    /// </markdown-doc>
    /// <summary>
    /// Simulate pressing or releasing (or both) keyboard keys.
    /// </summary>
    /// <param name="key">Key to simulate</param>
    /// <param name="pressState">Action to simulate</param>
    /// <name>Keyboard.Simulate</name>
    [ExposesScriptingMethod("Keyboard.Simulate")]
    public async void ExecuteForScript(KeyboardKey key, PressState pressState)
    {
        this.Key = key;
        this.PressState = pressState;

        if (this.PressState == PressState.Press)
        {
            SimulatedKeyboard.PressKey(this.Key);
        }

        if (this.PressState == PressState.Release)
        {
            SimulatedKeyboard.ReleaseKey(this.Key);
        }
    }

    public override async Task Execute(AbstractInputBag inputBag = null)
    {
        if (this.PressState == PressState.Press)
        {
            SimulatedKeyboard.PressKey(this.Key);
        }
        else if (this.PressState == PressState.Release)
        {
            SimulatedKeyboard.ReleaseKey(this.Key);
        }
    }

    public override string GetNameDisplay() => this.Name.Replace("{0}", Enum.GetName(typeof(KeyboardKey), this.Key))
            .Replace("{1}", Enum.GetName(typeof(PressState), this.PressState));

    public override bool Equals(object obj)
    {
        if (obj is not KeyboardAction action)
        {
            return false;
        }

        return action.Key == this.Key
            && action.PressState == this.PressState;
    }
}
