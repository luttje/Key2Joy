namespace Key2Joy.LowLevelInput.XInput;

/// <summary>
/// Flags representing various capabilities of an XInput controller.
/// </summary>
public enum CapabilityFlags
{
    /// <summary>
    /// The device has an integrated voice device.
    /// </summary>
    XINPUT_CAPS_VOICE_SUPPORTED = 0x0004,

    /// <summary>
    /// The device supports force feedback functionality. Note that these force-feedback features beyond rumble
    /// are not currently supported through XINPUT on Windows.
    /// </summary>
    XINPUT_CAPS_FFB_SUPPORTED = 0x0001,

    /// <summary>
    /// The device is wireless.
    /// </summary>
    XINPUT_CAPS_WIRELESS = 0x0002,

    /// <summary>
    /// The device supports plug-in modules. Note that plug-in modules like the text input device (TID)
    /// are not supported currently through XINPUT on Windows.
    /// </summary>
    XINPUT_CAPS_PMD_SUPPORTED = 0x0008,

    /// <summary>
    /// The device lacks menu navigation buttons (START, BACK, DPAD).
    /// </summary>
    XINPUT_CAPS_NO_NAVIGATION = 0x0010,
}
