namespace Key2Joy.LowLevelInput.XInput;

/// <summary>
/// Provides an interface for the XInput functionality.
/// </summary>
public interface IXInput
{
    /// <summary>
    /// Retrieves the state of a controller.
    /// </summary>
    /// <param name="userIndex">Index of the gamer associated with the device.</param>
    /// <param name="inputState">Receives the current state.</param>
    /// <returns>Returns the result code.</returns>
    XInputResultCode XInputGetState(int userIndex, ref XInputState inputState);

    /// <summary>
    /// Sets the vibration state of a controller.
    /// </summary>
    /// <param name="userIndex">Index of the gamer associated with the device.</param>
    /// <param name="vibrationInfo">The vibration information to send to the controller.</param>
    /// <returns>Returns the result code.</returns>
    XInputResultCode XInputSetState(int userIndex, ref XInputVibration vibrationInfo);

    /// <summary>
    /// Retrieves the capabilities of a controller.
    /// <see cref="XInputCapabilities"/>
    /// </summary>
    /// <param name="userIndex">Index of the gamer associated with the device.</param>
    /// <param name="flags">Input flags that identify the device type.</param>
    /// <param name="capabilities">Receives the capabilities.</param>
    /// <returns>Returns the result code.</returns>
    XInputResultCode XInputGetCapabilities(int userIndex, CapabilityRequestFlag flags, ref XInputCapabilities capabilities);

    /// <summary>
    /// Retrieves the capabilities of a controller for the default device type (gamepad).
    /// <see cref="XInputCapabilities"/>
    /// </summary>
    /// <param name="userIndex">Index of the gamer associated with the device.</param>
    /// <param name="capabilities">Receives the capabilities.</param>
    /// <returns>Returns the result code.</returns>
    XInputResultCode XInputGetCapabilities(int userIndex, ref XInputCapabilities capabilities);

    /// <summary>
    /// Retrieves battery information for a controller.
    /// </summary>
    /// <param name="userIndex">Index of the gamer associated with the device.</param>
    /// <param name="deviceType">Which device on this user index.</param>
    /// <param name="batteryInformation">Contains the level and types of batteries.</param>
    /// <returns>Returns the result code.</returns>
    XInputResultCode XInputGetBatteryInformation(int userIndex, BatteryDeviceType deviceType, ref XInputBatteryInformation batteryInformation);

    /// <summary>
    /// Retrieves a keystroke event from a controller.
    /// </summary>
    /// <param name="userIndex">Index of the gamer associated with the device.</param>
    /// <param name="reserved">Reserved for future use.</param>
    /// <param name="keystrokeData">Pointer to an XINPUT_KEYSTROKE structure that receives an input event.</param>
    /// <returns>Returns the result code.</returns>
    XInputResultCode XInputGetKeystroke(int userIndex, int reserved, ref XInputKeystroke keystrokeData);
}
