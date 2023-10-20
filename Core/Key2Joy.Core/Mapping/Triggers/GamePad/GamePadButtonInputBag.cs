using System.Collections.Generic;
using Key2Joy.Contracts.Mapping.Triggers;
using Key2Joy.LowLevelInput.XInput;

namespace Key2Joy.Mapping.Triggers.GamePad;

public class GamePadButtonInputBag : AbstractInputBag
{
    /// <summary>
    /// Buttons that were pressed since the last update.
    /// </summary>
    public IList<GamePadButton> PressedButtons { get; set; }

    /// <summary>
    /// Buttons that were released since the last update.
    /// </summary>
    public IList<GamePadButton> ReleasedButtons { get; set; }

    /// <summary>
    /// The raw state of the gamepad.
    /// </summary>
    public XInputGamePad State { get; set; }
}
