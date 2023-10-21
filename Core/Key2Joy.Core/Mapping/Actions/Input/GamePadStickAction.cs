using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using CommonServiceLocator;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Contracts.Mapping.Triggers;
using Key2Joy.LowLevelInput.SimulatedGamePad;
using Key2Joy.Mapping.Triggers.GamePad;
using Key2Joy.Mapping.Triggers.Mouse;

namespace Key2Joy.Mapping.Actions.Input;

[Action(
    Description = "GamePad Stick Simulation",
    NameFormat = "Move {0} Stick on GamePad #{3} by ({1}, {2})",
    GroupName = "GamePad Stick Simulation",
    GroupImage = "joystick"
)]
public class GamePadStickAction : CoreAction, IEquatable<GamePadStickAction>
{
    /// <summary>
    /// This describes what 'a lot of input movement' is, so we can scale
    /// the delta and make the scaling numbers feel intuitive.
    /// </summary>
    private const int BIG_DELTA_MOVEMENT = 250;

    private const short MIN_STICK_VALUE = short.MinValue;
    private const short MAX_STICK_VALUE = short.MaxValue;

    /// <summary>
    /// Which side to simulate
    /// </summary>
    public GamePadSide Side { get; set; }

    /// <summary>
    /// How much to simulate on the X axis. If null, then the inputbag of the input
    /// that triggered this will be used if possible
    /// </summary>
    public short? DeltaX { get; set; } = null;

    /// <summary>
    /// How much to simulate on the Y axis. If null, then the inputbag of the input
    /// that triggered this will be used if possible
    /// </summary>
    public short? DeltaY { get; set; } = null;

    /// <summary>
    /// If DeltaX is null, this is the scale factor to apply to the inputbag
    /// </summary>
    public float InputScaleX { get; set; } = 1;

    /// <summary>
    /// If DeltaY is null, this is the scale factor to apply to the inputbag
    /// </summary>
    public float InputScaleY { get; set; } = -1;

    /// <summary>
    /// Which gamepad to simulate
    /// </summary>
    public int GamePadIndex { get; set; }

    public GamePadStickAction(string name) : base(name)
    { }

    /// <inheritdoc/>
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
    /// Simulate moving a gamepad joystick
    /// </summary>
    /// <markdown-example>
    /// Moves the left gamepad joystick halfway down and to the right, then resets after 500ms
    /// <code language="lua">
    /// <![CDATA[
    /// GamePad.SimulateMove(0.5,0.5)
    /// SetTimeout(function()
    ///    GamePad.Reset()
    /// end, 500)
    /// ]]>
    /// </code>
    /// </markdown-example>
    /// <param name="deltaX">The fraction by which to move the stick forward (negative) or backward (positive)</param>
    /// <param name="deltaY">The fraction by which to move the stick right (positive) or left (negative)</param>
    /// <param name="side">Which gamepad stick to move, either GamePadStick.Left (default) or .Right</param>
    /// <param name="gamepadIndex">Which of 4 possible gamepads to simulate: 0 (default), 1, 2 or 3</param>
    /// <name>GamePad.SimulateMove</name>
    [ExposesScriptingMethod("GamePad.SimulateMove")]
    public async void ExecuteForScript(
        short deltaX,
        short deltaY,
        GamePadSide side = GamePadSide.Left,
        int gamepadIndex = 0)
    {
        this.DeltaX = deltaX;
        this.DeltaY = deltaY;
        this.Side = side;
        this.GamePadIndex = gamepadIndex;

        var gamePadService = ServiceLocator.Current.GetInstance<ISimulatedGamePadService>();
        gamePadService.EnsurePluggedIn(this.GamePadIndex);

        await this.Execute();
    }

    /// <summary>
    /// Scales InputBag values to a reasonable range for gamepad sticks
    /// </summary>
    /// <param name="value"></param>
    /// <param name="sensitivity"></param>
    /// <returns></returns>
    private static short Scale(int value, float sensitivity = 1.0f)
    {
        var scaled = (int)((float)value / BIG_DELTA_MOVEMENT * sensitivity * (MAX_STICK_VALUE - MIN_STICK_VALUE));
        var gamePadStickX = (short)Math.Min(Math.Max(scaled, MIN_STICK_VALUE), MAX_STICK_VALUE);

        return gamePadStickX;
    }

    /// <inheritdoc/>
    public override async Task Execute(AbstractInputBag inputBag = null)
    {
        var gamePadService = ServiceLocator.Current.GetInstance<ISimulatedGamePadService>();
        var gamePad = gamePadService.GetGamePad(this.GamePadIndex);

        if (!gamePad.GetIsPluggedIn())
        {
            gamePad.PlugIn();
        }

        var state = gamePad.GetState();

        var deltaX = (short)0;
        var deltaY = (short)0;

        if (this.DeltaX is not null)
        {
            deltaX = (short)this.DeltaX;
        }
        else if (inputBag is AxisDeltaInputBag axisInputBag)
        {
            deltaX = Scale(axisInputBag.DeltaX, this.InputScaleX);
        }

        if (this.DeltaY is not null)
        {
            deltaY = (short)this.DeltaY;
        }
        else if (inputBag is AxisDeltaInputBag axisInputBag)
        {
            deltaY = Scale(axisInputBag.DeltaY, this.InputScaleY);
        }

        if (this.Side == GamePadSide.Left)
        {
            state.LeftStickX = (short)Math.Min(Math.Max(state.LeftStickX + deltaX, MIN_STICK_VALUE), MAX_STICK_VALUE);
            state.LeftStickY = (short)Math.Min(Math.Max(state.LeftStickY + deltaY, MIN_STICK_VALUE), MAX_STICK_VALUE);
        }
        else
        {
            state.RightStickX = (short)Math.Min(Math.Max(state.RightStickX + deltaX, MIN_STICK_VALUE), MAX_STICK_VALUE);
            state.RightStickY = (short)Math.Min(Math.Max(state.RightStickY + deltaY, MIN_STICK_VALUE), MAX_STICK_VALUE);
        }

        gamePad.Update();
    }

    /// <inheritdoc/>
    public override string GetNameDisplay()
        => this.Name.Replace("{0}", Enum.GetName(typeof(GamePadSide), this.Side))
            .Replace("{1}", this.DeltaX?.ToString() ?? $"<TriggerInputX * {this.InputScaleX}>")
            .Replace("{2}", this.DeltaY?.ToString() ?? $"<TriggerInputY * {this.InputScaleY}>")
            .Replace("{3}", this.GamePadIndex.ToString());

    /// <inheritdoc/>
    public bool Equals(GamePadStickAction other)
        => other is not null
        && this.Side == other.Side
        && this.DeltaX == other.DeltaX
        && this.DeltaY == other.DeltaY
        && this.InputScaleX == other.InputScaleX
        && this.InputScaleY == other.InputScaleY
        && this.GamePadIndex == other.GamePadIndex;

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        var hashCode = -2085055944;
        hashCode = (hashCode * -1521134295) + this.Side.GetHashCode();
        hashCode = (hashCode * -1521134295) + this.DeltaX.GetHashCode();
        hashCode = (hashCode * -1521134295) + this.DeltaY.GetHashCode();
        hashCode = (hashCode * -1521134295) + this.InputScaleX.GetHashCode();
        hashCode = (hashCode * -1521134295) + this.InputScaleY.GetHashCode();
        hashCode = (hashCode * -1521134295) + this.GamePadIndex.GetHashCode();
        return hashCode;
    }
}
