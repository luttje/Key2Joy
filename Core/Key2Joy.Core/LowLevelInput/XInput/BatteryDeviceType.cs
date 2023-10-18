namespace Key2Joy.LowLevelInput.XInput;

/// <summary>
/// Represents the battery device types for XInput Game Controllers.
/// </summary>
public enum BatteryDeviceType : byte
{
    /// <summary>
    /// The device type is a gamepad.
    /// </summary>
    BATTERY_DEVTYPE_GAMEPAD = 0x00,

    /// <summary>
    /// The device type is a headset.
    /// </summary>
    BATTERY_DEVTYPE_HEADSET = 0x01,
}
