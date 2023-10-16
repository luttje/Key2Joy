using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommonServiceLocator;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Contracts.Mapping.Triggers;
using Key2Joy.LowLevelInput;
using Key2Joy.LowLevelInput.GamePad;
using Key2Joy.Mapping.Triggers.Mouse;
using SimWinInput;

namespace Key2Joy.Mapping.Actions.Input;

[Action(
    Description = "GamePad/Controller Simulation",
    NameFormat = "{1} {0} on GamePad #{2}",
    GroupName = "GamePad Simulation",
    GroupImage = "joystick"
)]
public class GamePadAction : CoreAction, IPressState
{
    public GamePadControl Control { get; set; }
    public PressState PressState { get; set; }
    public int GamePadIndex { get; set; }

    private readonly IGamePadService gamePadService;

    public GamePadAction(string name) : base(name)
        => this.gamePadService = ServiceLocator.Current.GetInstance<IGamePadService>();

    public static List<MappedOption> GetAllButtonActions(PressState pressState)
    {
        var actionFactory = ActionsRepository.GetAction(typeof(GamePadAction));

        List<MappedOption> actions = new();
        foreach (var control in GetAllButtons())
        {
            var action = (GamePadAction)MakeAction(actionFactory);
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

        // Skip the first (= None) enumeration value
        var buttons = new GamePadControl[allEnums.Length - 1];
        Array.Copy(allEnums, 1, buttons, 0, buttons.Length);

        return buttons;
    }

    public override void OnStartListening(AbstractTriggerListener listener, ref IList<AbstractAction> otherActions)
    {
        base.OnStartListening(listener, ref otherActions);

        this.gamePadService.EnsurePluggedIn(this.GamePadIndex);
    }

    private void HandleMouseMove(IGamePad gamePad, MouseMoveInputBag mouseMoveInputBag)
    {
        var state = gamePad.GetState();

        switch (this.Control)
        {
            case GamePadControl.LeftStickLeft:
            case GamePadControl.LeftStickRight:
                state.LeftStickX = this.CalculateNewState(mouseMoveInputBag.DeltaX, state.LeftStickX);
                break;

            case GamePadControl.LeftStickUp:
            case GamePadControl.LeftStickDown:
                state.LeftStickY = this.CalculateNewState(mouseMoveInputBag.DeltaY, state.LeftStickY);
                break;

            case GamePadControl.RightStickLeft:
            case GamePadControl.RightStickRight:
                state.RightStickX = this.CalculateNewState(mouseMoveInputBag.DeltaX, state.RightStickX);
                break;

            case GamePadControl.RightStickUp:
            case GamePadControl.RightStickDown:
                state.RightStickY = this.CalculateNewState(mouseMoveInputBag.DeltaY, state.RightStickY);
                break;
        }

        gamePad.Update();
    }

    private short CalculateNewState(int delta, short currentState)
        => (short)((delta + currentState) / 2);

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

        var gamePad = this.gamePadService.GetGamePad(this.GamePadIndex);

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
        var gamePad = this.gamePadService.GetGamePad(this.GamePadIndex);

        if (inputBag is MouseMoveInputBag mouseMoveInputBag)
        {
            this.HandleMouseMove(gamePad, mouseMoveInputBag);
            return;
        }

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
        if (obj is not GamePadAction action)
        {
            return false;
        }

        return action.Control == this.Control
            && action.PressState == this.PressState;
    }
}
