using System.Runtime.InteropServices;

namespace Key2Joy.LowLevelInput.XInput;

/// <summary>
/// Represents battery information for an XInput device, including its type and charge state.
/// </summary>
[StructLayout(LayoutKind.Explicit)]
public struct XInputBatteryInformation
{
    /// <summary>
    /// Gets or sets the type of battery for the XInput device.
    /// </summary>
    [MarshalAs(UnmanagedType.I1)]
    [FieldOffset(0)]
    public byte BatteryType;

    /// <summary>
    /// Gets or sets the charge state of the battery for the XInput device.
    /// </summary>
    [MarshalAs(UnmanagedType.I1)]
    [FieldOffset(1)]
    public byte BatteryLevel;

    /// <summary>
    /// Returns a string representation of the XInputBatteryInformation struct.
    /// </summary>
    /// <returns>A string containing the battery type and charge level.</returns>
    public override readonly string ToString()
        => string.Format(
                "{0} {1}",
                (BatteryTypes)this.BatteryType,
                (BatteryLevel)this.BatteryLevel
            );
}
