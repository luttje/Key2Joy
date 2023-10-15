using System;
using System.Threading.Tasks;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Contracts.Mapping.Triggers;
using Key2Joy.LowLevelInput;
using Key2Joy.Mapping.Triggers.Keyboard;

namespace Key2Joy.Mapping.Actions.Input;

[Action(
    Description = "Get Keyboard Key Down",
    Visibility = MappingMenuVisibility.Never,
    NameFormat = "Get Keyboard Key Down"
)]
public class KeyboardGetKeyDownAction : CoreAction
{
    private readonly VirtualKeyConverter virtualKeyConverter = new();

    public KeyboardGetKeyDownAction(string name)
        : base(name)
    { }

    /// <markdown-doc>
    /// <parent-name>Input</parent-name>
    /// <path>Api/Input</path>
    /// </markdown-doc>
    /// <summary>
    /// Tests if the provided keyboard key is currently pressed.
    ///
    /// Note: This function currently has trouble distinguishing between left and right keys. This means that `Keyboard.GetKeyDown(KeyboardKey.RightControl)` will return true even if the left control is pressed.
    ///
    /// You can find the keycodes in <see href="../Enumerations/KeyboardKey.md">the KeyboardKey Enumeration</see>.
    /// </summary>
    /// <markdown-example>
    /// Shows how to show all keys currently pressed.
    /// <code language="lua">
    /// <![CDATA[
    /// for keyName, key in pairs(KeyboardKey)do
    ///    if(Keyboard.GetKeyDown(key))then
    ///       Print(keyName, "is currently pressed")
    ///    end
    /// end
    /// ]]>
    /// </code>
    /// </markdown-example>
    /// <markdown-example>
    /// Shows how to only simulate pressing "A" when shift is also held down. This allows binding to multiple keys, where one is the trigger and the rest of the inputs are checked in the script.
    /// <code language="js">
    /// <![CDATA[
    /// if(Keyboard.GetKeyDown(KeyboardKey.Shift)) {
    ///   GamePad.Simulate(GamePadControl.A, PressState.Press);
    /// }
    /// ]]>
    /// </code>
    /// </markdown-example>
    /// <param name="key">The key to test for</param>
    /// <returns>True if the key is currently pressed down, false otherwise</returns>
    /// <name>Keyboard.GetKeyDown</name>
    [ExposesScriptingMethod("Keyboard.GetKeyDown")]
    public bool ExecuteForScript(KeyboardKey key)
        => KeyboardTriggerListener.Instance.GetKeyDown(this.virtualKeyConverter.KeysFromScanCode(key));

    public override async Task Execute(AbstractInputBag inputBag = null)
    {
        // TODO: Currently this is only a script action...
    }

    public override string GetNameDisplay() => this.Name;

    public override bool Equals(object obj)
    {
        if (obj is not KeyboardGetKeyDownAction)
        {
            return false;
        }

        // TODO: Currently this is only a script action so this is irrelevant
        return false;
    }
}
