using System;

namespace Key2Joy.LowLevelInput.XInput;

/// <summary>
/// Represents a set of flags that indicate the state of various buttons on a device.
/// </summary>
[Flags]
public enum ButtonFlags : int
{
    /// <summary>
    /// The Up button on the directional pad.
    /// </summary>
    XINPUT_GAMEPAD_DPAD_UP = 0x0001,

    /// <summary>
    /// The Down button on the directional pad.
    /// </summary>
    XINPUT_GAMEPAD_DPAD_DOWN = 0x0002,

    /// <summary>
    /// The Left button on the directional pad.
    /// </summary>
    XINPUT_GAMEPAD_DPAD_LEFT = 0x0004,

    /// <summary>
    /// The Right button on the directional pad.
    /// </summary>
    XINPUT_GAMEPAD_DPAD_RIGHT = 0x0008,

    /// <summary>
    /// The Start button.
    /// </summary>
    XINPUT_GAMEPAD_START = 0x0010,

    /// <summary>
    /// The Back button.
    /// </summary>
    XINPUT_GAMEPAD_BACK = 0x0020,

    /// <summary>
    /// The Left Thumbstick button.
    /// </summary>
    XINPUT_GAMEPAD_LEFT_THUMB = 0x0040,

    /// <summary>
    /// The Right Thumbstick button.
    /// </summary>
    XINPUT_GAMEPAD_RIGHT_THUMB = 0x0080,

    /// <summary>
    /// The Left Shoulder button.
    /// </summary>
    XINPUT_GAMEPAD_LEFT_SHOULDER = 0x0100,

    /// <summary>
    /// The Right Shoulder button.
    /// </summary>
    XINPUT_GAMEPAD_RIGHT_SHOULDER = 0x0200,

    /// <summary>
    /// The A button.
    /// </summary>
    XINPUT_GAMEPAD_A = 0x1000,

    /// <summary>
    /// The B button.
    /// </summary>
    XINPUT_GAMEPAD_B = 0x2000,

    /// <summary>
    /// The X button.
    /// </summary>
    XINPUT_GAMEPAD_X = 0x4000,

    /// <summary>
    /// The Y button.
    /// </summary>
    XINPUT_GAMEPAD_Y = 0x8000,
}
