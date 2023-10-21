using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonServiceLocator;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Contracts.Mapping.Triggers;
using Key2Joy.LowLevelInput;
using Key2Joy.LowLevelInput.SimulatedGamePad;
using SimWinInput;

namespace Key2Joy.Mapping.Actions.Input;

[Action(
    Description = "GamePad/Controller Simulation",
    NameFormat = "{1} {0} on GamePad #{2}",
    GroupName = "GamePad Simulation",
    GroupImage = "joystick"
)]
public class GamePadButtonAction : CoreAction, IPressState
{
    public GamePadControl Control { get; set; }
    public PressState PressState { get; set; }
    public int GamePadIndex { get; set; }

    public GamePadButtonAction(string name)
        : base(name)
    { }

    public static List<MappedOption> GetAllButtonActions(PressState pressState)
    {
        var actionFactory = ActionsRepository.GetAction(typeof(GamePadButtonAction));

        List<MappedOption> actions = new();
        foreach (var control in GetAllButtons())
        {
            var action = (GamePadButtonAction)MakeAction(actionFactory);
            action.Control = control;
            action.PressState = pressState;

            actions.Add(new MappedOption
            {
                Action = action
            });
        }
        return actions;
    }

    public static GamePadControl[] GetAllButtons()
    {
        var allEnums = Enum.GetValues(typeof(GamePadControl));

        var skip = new GamePadControl[]
        {
            GamePadControl.None,

            // Handled separately in GamePadTriggerAction
            GamePadControl.LeftTrigger,
            GamePadControl.RightTrigger,

            // Handled separately in GamePadStickAction
            GamePadControl.LeftStickLeft,
            GamePadControl.LeftStickRight,
            GamePadControl.LeftStickUp,
            GamePadControl.LeftStickDown,
            GamePadControl.RightStickLeft,
            GamePadControl.RightStickRight,
            GamePadControl.RightStickUp,
            GamePadControl.RightStickDown,
        };

        var buttons = allEnums
            .Cast<GamePadControl>()
            .Where(x => !skip.Contains(x))
            .ToArray();

        return buttons;
    }

    public override void OnStartListening(AbstractTriggerListener listener, ref IList<AbstractAction> otherActions)
    {
        base.OnStartListening(listener, ref otherActions);

        var gamePadService = ServiceLocator.Current.GetInstance<ISimulatedGamePadService>();
        gamePadService.EnsurePluggedIn(this.GamePadIndex);
    }

    /// <markdown-doc>
    /// <parent-name>Input</parent-name>
    /// <path>Api/Input</path>
    /// </markdown-doc>
    /// <summary>
    /// Simulate pressing or releasing (or both) gamepad buttons.
    /// </summary>
    /// <markdown-example>
    /// Shows how to press "A" on the gamepad for 500ms, then release it.
    /// <code language="js">
    /// <![CDATA[
    /// GamePad.Simulate(GamePadControl.A, PressState.Press);
    /// setTimeout(function () {
    ///     GamePad.Simulate(GamePadControl.A, PressState.Release);
    /// }, 500);
    /// ]]>
    /// </code>
    /// </markdown-example>
    /// <param name="control">Button to simulate</param>
    /// <param name="pressState">Action to simulate</param>
    /// <param name="gamepadIndex">Which of 4 possible gamepads to simulate: 0 (default), 1, 2 or 3</param>
    /// <name>GamePad.Simulate</name>
    [ExposesScriptingMethod("GamePad.Simulate")]
    public void ExecuteForScript(GamePadControl control, PressState pressState, int gamepadIndex = 0)
    {
        this.Control = control;
        this.PressState = pressState;
        this.GamePadIndex = gamepadIndex;

        var gamePadService = ServiceLocator.Current.GetInstance<ISimulatedGamePadService>();
        var gamePad = gamePadService.GetGamePad(this.GamePadIndex);

        if (!gamePad.GetIsPluggedIn())
        {
            gamePad.PlugIn();
        }

        if (this.PressState == PressState.Press)
        {
            gamePad.SetControl(this.Control);
        }

        if (this.PressState == PressState.Release)
        {
            gamePad.ReleaseControl(this.Control);
        }
    }

    public override async Task Execute(AbstractInputBag inputBag = null)
    {
        var gamePadService = ServiceLocator.Current.GetInstance<ISimulatedGamePadService>();
        var gamePad = gamePadService.GetGamePad(this.GamePadIndex);

        if (this.PressState == PressState.Press)
        {
            gamePad.SetControl(this.Control);
        }
        else if (this.PressState == PressState.Release)
        {
            gamePad.ReleaseControl(this.Control);
        }
    }

    public override string GetNameDisplay() => this.Name.Replace("{0}", this.Control.ToString())
            .Replace("{1}", Enum.GetName(typeof(PressState), this.PressState))
            .Replace("{2}", this.GamePadIndex.ToString());

    public override bool Equals(object obj)
    {
        if (obj is not GamePadButtonAction action)
        {
            return false;
        }

        return action.Control == this.Control
            && action.PressState == this.PressState;
    }
}
