using System;
using System.Runtime.InteropServices;

namespace Key2Joy.LowLevelInput.XInput;

/// <summary>
/// Represents the state of an Xbox 360 controller.
/// </summary>
/// <remarks>
/// The <see cref="PacketNumber"/> member is incremented only if the status of the controller has changed since the controller was last polled.
/// </remarks>
[StructLayout(LayoutKind.Explicit)]
public struct XInputState : IEquatable<XInputState>
{
    /// <summary>
    /// State packet number. Indicates whether there have been any changes in the state of the controller.
    /// If the <see cref="PacketNumber"/> member is the same in sequentially returned XInputState structures, the controller state has not changed.
    /// </summary>
    [FieldOffset(0)]
    public int PacketNumber;

    /// <summary>
    /// XINPUT_GAMEPAD structure containing the current state of an Xbox 360 Controller.
    /// </summary>
    [FieldOffset(4)]
    public XInputGamepad Gamepad;

    /// <summary>
    /// Copies the state from another XInputState object.
    /// </summary>
    /// <param name="source">The source XInputState to copy from.</param>
    public void Copy(XInputState source)
    {
        this.PacketNumber = source.PacketNumber;
        this.Gamepad.Copy(source.Gamepad);
    }

    /// <inheritdoc/>
    public override readonly bool Equals(object obj)
        => obj is XInputState state && this.Equals(state);

    /// <inheritdoc/>
    public readonly bool Equals(XInputState other)
        => this.PacketNumber == other.PacketNumber
        && this.Gamepad.Equals(other.Gamepad);

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        var hashCode = -1459879740;
        hashCode = (hashCode * -1521134295) + this.PacketNumber.GetHashCode();
        hashCode = (hashCode * -1521134295) + this.Gamepad.GetHashCode();
        return hashCode;
    }
}
