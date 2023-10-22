namespace Key2Joy.LowLevelInput.XInput;

/// <summary>
/// Represents the charge state of the battery for a wireless XInput device.
/// </summary>
public enum BatteryLevel : byte
{
    /// <summary>
    /// The battery is empty.
    /// </summary>
    BATTERY_LEVEL_EMPTY = 0x00,

    /// <summary>
    /// The battery is low.
    /// </summary>
    BATTERY_LEVEL_LOW = 0x01,

    /// <summary>
    /// The battery is at a medium charge level.
    /// </summary>
    BATTERY_LEVEL_MEDIUM = 0x02,

    /// <summary>
    /// The battery is full.
    /// </summary>
    BATTERY_LEVEL_FULL = 0x03,
}
