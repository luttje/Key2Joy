using System;
using System.Runtime.InteropServices;

namespace Key2Joy.LowLevelInput.XInput;

/// <summary>
/// Represents the state of a controller.
/// </summary>
[StructLayout(LayoutKind.Explicit)]
public struct XInputGamepad : IEquatable<XInputGamepad>
{
    /// <summary>
    /// Can be used as a positive and negative value to filter left thumbstick input.
    /// </summary>
    public const int XINPUT_GAMEPAD_LEFT_THUMB_DEADZONE = 7849;

    /// <summary>
    /// Can be used as a positive and negative value to filter right thumbstick input.
    /// </summary>
    public const int XINPUT_GAMEPAD_RIGHT_THUMB_DEADZONE = 8689;

    /// <summary>
    /// May be used as the value which bLeftTrigger and bRightTrigger must be greater than to register as pressed. This is optional, but often desirable. Xbox 360 Controller buttons do not manifest crosstalk.
    /// </summary>
    public const int XINPUT_GAMEPAD_TRIGGER_THRESHOLD = 30;

    /// <summary>
    /// Bitmask of the device digital buttons. A set bit indicates that the corresponding button is pressed.
    /// </summary>
    /// <remarks>
    /// Refer to XINPUT_GAMEPAD_* bitmasks for specific button mappings.
    /// Bits that are set but not defined are reserved, and their state is undefined.
    /// </summary>
    [MarshalAs(UnmanagedType.I2)]
    [FieldOffset(0)]
    public short ButtonsBitmask;

    /// <summary>
    /// The current value of the left trigger analog control. The value is between 0 and 255.
    /// </summary>
    [MarshalAs(UnmanagedType.I1)]
    [FieldOffset(2)]
    public byte LeftTrigger;

    /// <summary>
    /// The current value of the right trigger analog control. The value is between 0 and 255.
    /// </summary>
    [MarshalAs(UnmanagedType.I1)]
    [FieldOffset(3)]
    public byte RightTrigger;

    /// <summary>
    /// Left thumbstick x-axis value. Negative values signify down or to the left,
    /// positive values signify up or to the right. The value is between -32768 and 32767.
    /// A value of 0 is centered. Negative values signify down or to the left. Positive values
    /// signify up or to the right.
    /// </summary>
    [MarshalAs(UnmanagedType.I2)]
    [FieldOffset(4)]
    public short LeftThumbX;

    /// <summary>
    /// Left thumbstick y-axis value. The value is between -32768 and 32767.
    /// </summary>
    [MarshalAs(UnmanagedType.I2)]
    [FieldOffset(6)]
    public short LeftThumbY;

    /// <summary>
    /// Right thumbstick x-axis value. The value is between -32768 and 32767.
    /// </summary>
    [MarshalAs(UnmanagedType.I2)]
    [FieldOffset(8)]
    public short RightThumbX;

    /// <summary>
    /// Right thumbstick y-axis value. The value is between -32768 and 32767.
    /// </summary>
    [MarshalAs(UnmanagedType.I2)]
    [FieldOffset(10)]
    public short RightThumbY;

    /// <summary>
    /// Checks if a specific button or buttons represented by the bitmask are pressed.
    /// </summary>
    /// <param name="buttonFlags">The bitmask representing the button(s).</param>
    /// <returns>True if the button(s) are pressed, otherwise false.</returns>
    public readonly bool IsButtonPressed(ButtonFlags buttonFlags)
        => (this.ButtonsBitmask & (int)buttonFlags) == (int)buttonFlags;

    /// <summary>
    /// Checks if a specific button or buttons represented by the bitmask are present.
    /// </summary>
    /// <param name="buttonFlags">The bitmask representing the button(s).</param>
    /// <returns>True if the button(s) are present, otherwise false.</returns>
    public readonly bool IsButtonPresent(ButtonFlags buttonFlags)
        => (this.ButtonsBitmask & (int)buttonFlags) == (int)buttonFlags;

    /// <summary>
    /// Copies the values from the source gamepad to the current instance.
    /// </summary>
    /// <param name="source">The source gamepad from which values should be copied.</param>
    public void Copy(XInputGamepad source)
    {
        this.LeftThumbX = source.LeftThumbX;
        this.LeftThumbY = source.LeftThumbY;
        this.RightThumbX = source.RightThumbX;
        this.RightThumbY = source.RightThumbY;
        this.LeftTrigger = source.LeftTrigger;
        this.RightTrigger = source.RightTrigger;
        this.ButtonsBitmask = source.ButtonsBitmask;
    }

    /// <inheritdoc/>
    public readonly bool Equals(XInputGamepad other)
        => this.ButtonsBitmask == other.ButtonsBitmask
        && this.LeftTrigger == other.LeftTrigger
        && this.RightTrigger == other.RightTrigger
        && this.LeftThumbX == other.LeftThumbX
        && this.LeftThumbY == other.LeftThumbY
        && this.RightThumbX == other.RightThumbX
        && this.RightThumbY == other.RightThumbY;

    /// <inheritdoc/>
    public override readonly bool Equals(object obj)
        => obj is XInputGamepad gamepad && this.Equals(gamepad);

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        var hashCode = 235782390;
        hashCode = (hashCode * -1521134295) + this.ButtonsBitmask.GetHashCode();
        hashCode = (hashCode * -1521134295) + this.LeftTrigger.GetHashCode();
        hashCode = (hashCode * -1521134295) + this.RightTrigger.GetHashCode();
        hashCode = (hashCode * -1521134295) + this.LeftThumbX.GetHashCode();
        hashCode = (hashCode * -1521134295) + this.LeftThumbY.GetHashCode();
        hashCode = (hashCode * -1521134295) + this.RightThumbX.GetHashCode();
        hashCode = (hashCode * -1521134295) + this.RightThumbY.GetHashCode();
        return hashCode;
    }
}
