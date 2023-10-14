using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Contracts.Mapping.Triggers;
using Key2Joy.LowLevelInput;

namespace Key2Joy.Mapping.Actions.Input;

[Action(
    Description = "Get Cursor Position",
    Visibility = MappingMenuVisibility.Never,
    NameFormat = "Get the Cursor Position"
)]
public class GetCursorPositionAction : CoreAction
{
    [DllImport("user32.dll")]
    private static extern bool GetCursorPos(ref Point lpPoint);

    public GetCursorPositionAction(string name)
        : base(name)
    { }

    /// <markdown-doc>
    /// <parent-name>Input</parent-name>
    /// <path>Api/Input</path>
    /// </markdown-doc>
    /// <summary>
    /// Gets the current cursor position
    /// </summary>
    /// <markdown-example>
    /// The code below prints 0, 0 when the cursor is held in the top left of the first monitor.
    /// <code language="js">
    /// <![CDATA[
    /// var cursorPosition = Cursor.GetPosition()
    /// Print(`${cursorPosition.X}, ${cursorPosition.Y}`)
    /// ]]>
    /// </code>
    /// <code language="lua">
    /// <![CDATA[
    /// local cursorPosition = Cursor.GetPosition()
    /// print(cursorPosition.X .. ", " .. cursorPosition.Y)
    /// ]]>
    /// </code>
    /// </markdown-example>
    /// <returns>A Point object with X and Y properties that represent the cursor X and Y</returns>
    /// <name>Cursor.GetPosition</name>
    [ExposesScriptingMethod("Cursor.GetPosition")]
    public Point ExecuteForScript()
    {
        Point cursorPosition = new();
        GetCursorPos(ref cursorPosition);
        return cursorPosition;
    }

    public override async Task Execute(AbstractInputBag inputBag = null)
    {
        // TODO: Currently this is only a script action...
    }

    public override bool Equals(object obj)
    {
        if (obj is not GetCursorPositionAction)
        {
            return false;
        }

        return true;
    }
}
