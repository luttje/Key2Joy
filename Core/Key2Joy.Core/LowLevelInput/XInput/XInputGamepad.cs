using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Key2Joy.Mapping;
using Key2Joy.Mapping.Triggers.GamePad;

namespace Key2Joy.LowLevelInput.XInput;

/// <summary>
/// Represents the state of a controller.
/// </summary>
[StructLayout(LayoutKind.Explicit)]
public struct XInputGamePad : IEquatable<XInputGamePad>
{
    public const int ThumbstickValueMin = short.MinValue;
    public const int ThumbstickValueMax = short.MaxValue;
    public const int TriggerValueMin = 0;
    public const int TriggerValueMax = 255;

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
    public readonly bool IsButtonPressed(GamePadButton buttonFlags)
        => (this.ButtonsBitmask & (int)buttonFlags) == (int)buttonFlags;

    /// <summary>
    /// Checks if a specific button or buttons represented by the bitmask are present.
    /// </summary>
    /// <param name="buttonFlags">The bitmask representing the button(s).</param>
    /// <returns>True if the button(s) are present, otherwise false.</returns>
    public readonly bool IsButtonPresent(GamePadButton buttonFlags)
        => (this.ButtonsBitmask & (int)buttonFlags) == (int)buttonFlags;

    /// <summary>
    /// Returns all pressed buttons in a List.
    /// </summary>
    /// <returns></returns>
    public readonly IList<GamePadButton> GetPressedButtonsList()
    {
        var pressedButtons = new List<GamePadButton>();

        foreach (GamePadButton button in Enum.GetValues(typeof(GamePadButton)))
        {
            if (this.IsButtonPressed(button))
            {
                pressedButtons.Add(button);
            }
        }

        return pressedButtons;
    }

    /// <summary>
    /// Checks if the thumb stick on the given side has moved past a certain threshold (or else the default deadzone)
    /// </summary>
    /// <param name="side"></param>
    /// <param name="deltaMargin"></param>
    /// <returns></returns>
    public readonly bool IsThumbstickMoved(GamePadSide side, ExactAxisDirection? deltaMargin = null)
    {
        var defaultDeadzone = side == GamePadSide.Left ? XINPUT_GAMEPAD_LEFT_THUMB_DEADZONE : XINPUT_GAMEPAD_RIGHT_THUMB_DEADZONE;
        var thumbstickX = side == GamePadSide.Left ? this.LeftThumbX : this.RightThumbX;
        var thumbstickY = side == GamePadSide.Left ? this.LeftThumbY : this.RightThumbY;
        var deadzoneX = deltaMargin?.X * ThumbstickValueMax ?? defaultDeadzone;
        var deadzoneY = deltaMargin?.Y * ThumbstickValueMax ?? defaultDeadzone;

        // We must convert to an int, otherwise the absolute of short -32768 (32768) would fail since it's too big
        if (Math.Abs((int)thumbstickX) > deadzoneX || Math.Abs((int)thumbstickY) > deadzoneY)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Checks if the trigger on a certain side has pulled back past a certain threshold (or else the default deadzone)
    /// </summary>
    /// <param name="side"></param>
    /// <param name="deltaMargin"></param>
    /// <returns></returns>
    public readonly bool IsTriggerPulled(GamePadSide side, float? deltaMargin)
    {
        var deadzone = deltaMargin ?? XINPUT_GAMEPAD_TRIGGER_THRESHOLD;
        var trigger = side == GamePadSide.Left ? this.LeftTrigger : this.RightTrigger;

        if (trigger > deadzone)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Returns the stick delta for a given side as an exact axis fraction.
    /// </summary>
    /// <param name="side"></param>
    /// <returns></returns>
    public readonly ExactAxisDirection GetStickDelta(GamePadSide side)
    {
        if (side == GamePadSide.Left)
        {
            return new ExactAxisDirection(
                (float)this.LeftThumbX / ThumbstickValueMax,
                (float)this.LeftThumbY / ThumbstickValueMax);
        }

        return new ExactAxisDirection(
            (float)this.RightThumbX / ThumbstickValueMax,
            (float)this.RightThumbY / ThumbstickValueMax);
    }

    /// <summary>
    /// Returns the trigger delta for a given side as an exact axis fraction.
    /// </summary>
    /// <param name="side"></param>
    /// <returns></returns>
    public readonly float GetTriggerDelta(GamePadSide side)
    {
        if (side == GamePadSide.Left)
        {
            return (float)this.LeftTrigger / TriggerValueMax;
        }

        return (float)this.RightTrigger / TriggerValueMax;
    }

    /// <summary>
    /// Copies the values from the source gamepad to the current instance.
    /// </summary>
    /// <param name="source">The source gamepad from which values should be copied.</param>
    public void Copy(XInputGamePad source)
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
    public readonly bool Equals(XInputGamePad other)
        => this.ButtonsBitmask == other.ButtonsBitmask
        && this.LeftTrigger == other.LeftTrigger
        && this.RightTrigger == other.RightTrigger
        && this.LeftThumbX == other.LeftThumbX
        && this.LeftThumbY == other.LeftThumbY
        && this.RightThumbX == other.RightThumbX
        && this.RightThumbY == other.RightThumbY;

    /// <inheritdoc/>
    public override readonly bool Equals(object obj)
        => obj is XInputGamePad gamepad && this.Equals(gamepad);

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

    public override string ToString()
        => $"{nameof(XInputGamePad)} Buttons: {this.ButtonsBitmask}, LeftTrigger: {this.LeftTrigger}, RightTrigger: {this.RightTrigger}, LeftThumb: ({this.LeftThumbX}, {this.LeftThumbY}), RightThumb: ({this.RightThumbX}, {this.RightThumbY})";
}
