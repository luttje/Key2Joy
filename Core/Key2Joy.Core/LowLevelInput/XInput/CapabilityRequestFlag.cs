namespace Key2Joy.LowLevelInput.XInput;

/// <summary>
/// <see cref="IXInput.XInputGetCapabilities"/> requires a flag to specify which device type to retrieve
/// capabilities for. This is the only available flag.
/// </summary>
public enum CapabilityRequestFlag : int
{
    /// <summary>
    /// Limit query to devices of Xbox 360 Controller type.
    /// </summary>
    XINPUT_FLAG_GAMEPAD = 0x00000001,
}
