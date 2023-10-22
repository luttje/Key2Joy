using System;

namespace Key2Joy.LowLevelInput.XInput;

/// <summary>
/// Represents a set of flags that indicate the state of various buttons on a device.
/// </summary>
[Flags]
public enum GamePadButton : int
{
    /// <summary>
    /// The Up button on the directional pad.
    /// </summary>
    DPadUp = 0x0001,

    /// <summary>
    /// The Down button on the directional pad.
    /// </summary>
    DPadDown = 0x0002,

    /// <summary>
    /// The Left button on the directional pad.
    /// </summary>
    DPadLeft = 0x0004,

    /// <summary>
    /// The Right button on the directional pad.
    /// </summary>
    DPadRight = 0x0008,

    /// <summary>
    /// The Start button.
    /// </summary>
    Start = 0x0010,

    /// <summary>
    /// The Back button.
    /// </summary>
    Back = 0x0020,

    /// <summary>
    /// The Left Thumbstick button.
    /// </summary>
    LeftThumbstick = 0x0040,

    /// <summary>
    /// The Right Thumbstick button.
    /// </summary>
    RightThumbstick = 0x0080,

    /// <summary>
    /// The Left Shoulder button.
    /// </summary>
    LeftShoulder = 0x0100,

    /// <summary>
    /// The Right Shoulder button.
    /// </summary>
    RightShoulder = 0x0200,

    /// <summary>
    /// The A button.
    /// </summary>
    A = 0x1000,

    /// <summary>
    /// The B button.
    /// </summary>
    B = 0x2000,

    /// <summary>
    /// The X button.
    /// </summary>
    X = 0x4000,

    /// <summary>
    /// The Y button.
    /// </summary>
    Y = 0x8000,
}
