using System;
using System.Threading.Tasks;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Contracts.Mapping.Triggers;
using Key2Joy.LowLevelInput;

namespace Key2Joy.Mapping.Actions.Input;

[Action(
    Description = "Mouse Move Simulation",
    Visibility = MappingMenuVisibility.Never,
    NameFormat = "Move {0} {1},{2} on Mouse",
    GroupName = "Mouse Simulation",
    GroupImage = "mouse"
)]
public class MouseMoveAction : CoreAction, IProvideReverseAspect
{
    public Mouse.MoveType MoveType { get; set; }
    public int X { get; set; }
    public int Y { get; set; }

    public MouseMoveAction(string name)
        : base(name)
    {
    }

    /// <inheritdoc/>
    public void MakeReverse(AbstractMappingAspect aspect)
    {
        var reverse = aspect as MouseMoveAction;

        reverse.X = this.X * -1;
        reverse.Y = this.Y * -1;
    }

    /// <markdown-doc>
    /// <parent-name>Input</parent-name>
    /// <path>Api/Input</path>
    /// </markdown-doc>
    /// <summary>
    /// Simulate moving the mouse
    /// </summary>
    /// <markdown-example>
    /// Nudges the cursor 100 pixels to the left from where it is now.
    /// <code language="js">
    /// <![CDATA[
    /// Mouse.SimulateMove(-100,0)
    /// ]]>
    /// </code>
    /// </markdown-example>
    /// <markdown-example>
    /// Moves the cursor to an absolute position on the screen.
    /// <code language="lua">
    /// <![CDATA[
    /// Mouse.SimulateMove(1024,1050,MoveType.Absolute)
    /// ]]>
    /// </code>
    /// </markdown-example>
    /// <param name="x">X coordinate to move by/to</param>
    /// <param name="y">Y coordinate to move by/to</param>
    /// <param name="moveType">Whether to move relative to the current cursor position (default) or to an absolute position on screen</param>
    /// <name>Mouse.SimulateMove</name>
    [ExposesScriptingMethod("Mouse.SimulateMove")]
    public async void ExecuteForScript(int x, int y, Mouse.MoveType moveType = Mouse.MoveType.Relative)
    {
        this.X = x;
        this.Y = y;
        this.MoveType = moveType;

        await this.Execute();
    }

    public override async Task Execute(AbstractInputBag inputBag = null) => SimulatedMouse.Move(this.X, this.Y, this.MoveType);

    public override string GetNameDisplay() => this.Name.Replace("{0}", this.MoveType == Mouse.MoveType.Absolute ? "To" : "By")
            .Replace("{1}", this.X.ToString())
            .Replace("{2}", this.Y.ToString());

    public override bool Equals(object obj)
    {
        if (obj is not MouseMoveAction action)
        {
            return false;
        }

        return action.MoveType == this.MoveType
            && action.X == this.X
            && action.Y == this.Y;
    }
}
