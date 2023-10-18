namespace Key2Joy.LowLevelInput.XInput;

/// <summary>
/// Represents the type of battery for an XInput device.
/// </summary>
public enum BatteryTypes : byte
{
    /// <summary>
    /// This device is not connected.
    /// </summary>
    BATTERY_TYPE_DISCONNECTED = 0x00,

    /// <summary>
    /// Wired device, no battery.
    /// </summary>
    BATTERY_TYPE_WIRED = 0x01,

    /// <summary>
    /// Alkaline battery source.
    /// </summary>
    BATTERY_TYPE_ALKALINE = 0x02,

    /// <summary>
    /// Nickel Metal Hydride battery source.
    /// </summary>
    BATTERY_TYPE_NIMH = 0x03,

    /// <summary>
    /// Cannot determine the battery type.
    /// </summary>
    BATTERY_TYPE_UNKNOWN = 0xFF,
}
