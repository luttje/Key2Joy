using System;

namespace Key2Joy.LowLevelInput.XInput;

/// <summary>
/// Specifies the subtype of an XInput device.
/// </summary>
[Flags]
public enum DeviceSubType : byte
{
    /// <summary>
    /// Unknown controller subtype.
    /// </summary>
    XINPUT_DEVSUBTYPE_UNKNOWN = 0x00,

    /// <summary>
    /// Gamepad controller subtype.
    /// Includes Left and Right Sticks, Left and Right Triggers, Directional Pad,
    /// and all standard buttons (A, B, X, Y, START, BACK, LB, RB, LSB, RSB).
    /// </summary>
    XINPUT_DEVSUBTYPE_GAMEPAD = 0x01,

    /// <summary>
    /// Racing wheel controller subtype.
    /// Left Stick X reports the wheel rotation, Right Trigger is the acceleration pedal,
    /// and Left Trigger is the brake pedal. Includes Directional Pad and most standard buttons
    /// (A, B, X, Y, START, BACK, LB, RB). LSB and RSB are optional.
    /// </summary>
    XINPUT_DEVSUBTYPE_WHEEL = 0x02,

    /// <summary>
    /// Arcade stick controller subtype.
    /// Includes a Digital Stick that reports as a DPAD (up, down, left, right), and most standard buttons
    /// (A, B, X, Y, START, BACK). The Left and Right Triggers are implemented as digital buttons and
    /// report either 0 or 0xFF. LB, LSB, RB, and RSB are optional.
    /// </summary>
    XINPUT_DEVSUBTYPE_ARCADE_STICK = 0x03,

    /// <summary>
    /// Flight stick controller subtype.
    /// Includes a pitch and roll stick that reports as the Left Stick, a POV Hat which reports as the
    /// Right Stick, a rudder (handle twist or rocker) that reports as Left Trigger, and a throttle control
    /// as the Right Trigger. Includes support for a primary weapon (A), secondary weapon (B), and other
    /// standard buttons (X, Y, START, BACK). LB, LSB, RB, and RSB are optional.
    /// </summary>
    XINPUT_DEVSUBTYPE_FLIGHT_STICK = 0x04,

    /// <summary>
    /// Dance pad controller subtype.
    /// Includes the Directional Pad and standard buttons (A, B, X, Y) on the pad, plus BACK and START.
    /// </summary>
    XINPUT_DEVSUBTYPE_DANCE_PAD = 0x05,

    /// <summary>
    /// Guitar controller subtype.
    /// The strum bar maps to DPAD (up and down), and the frets are assigned to A (green), B (red),
    /// Y (yellow), X (blue), and LB (orange). Right Stick Y is associated with a vertical orientation sensor;
    /// Right Stick X is the whammy bar. Includes support for BACK, START, DPAD (left, right).
    /// Left Trigger (pickup selector), Right Trigger, RB, LSB (fret modifier), RSB are optional.
    /// </summary>
    XINPUT_DEVSUBTYPE_GUITAR = 0x06,

    /// <summary>
    /// Alternate guitar controller subtype.
    /// Supports a larger range of movement for the vertical orientation sensor.
    /// </summary>
    XINPUT_DEVSUBTYPE_GUITAR_ALTERNATE = 0x07,

    /// <summary>
    /// Drum controller subtype.
    /// The drum pads are assigned to buttons: A for green (Floor Tom), B for red (Snare Drum),
    /// X for blue (Low Tom), Y for yellow (High Tom), and LB for the pedal (Bass Drum).
    /// Includes Directional-Pad, BACK, and START. RB, LSB, and RSB are optional.
    /// </summary>
    XINPUT_DEVSUBTYPE_DRUM_KIT = 0x08,

    /// <summary>
    /// Bass guitar controller subtype.
    /// Identical to Guitar, with the distinct subtype to simplify setup.
    /// </summary>
    XINPUT_DEVSUBTYPE_GUITAR_BASS = 0x0B,

    /// <summary>
    /// Arcade pad controller subtype.
    /// Includes Directional Pad and most standard buttons (A, B, X, Y, START, BACK, LB, RB).
    /// The Left and Right Triggers are implemented as digital buttons and report either 0 or 0xFF.
    /// Left Stick, Right Stick, LSB, and RSB are optional.
    /// </summary>
    XINPUT_DEVSUBTYPE_ARCADE_PAD = 0x13
}
