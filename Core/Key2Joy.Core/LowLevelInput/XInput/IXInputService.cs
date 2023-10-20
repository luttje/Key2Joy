using System;
using System.Collections.Generic;

namespace Key2Joy.LowLevelInput.XInput;

/// <summary>
/// Represents the interface for interacting with XInput devices.
/// </summary>
public interface IXInputService
{
    /// <summary>
    /// Occurs when the state of a registered device changes.
    /// </summary>
    event EventHandler<DeviceStateChangedEventArgs> StateChanged;

    /// <summary>
    /// Registers a device by its index for monitoring its state.
    /// </summary>
    /// <param name="deviceIndex">The index of the device to register.</param>
    void RegisterDevice(int deviceIndex);

    /// <summary>
    /// Starts polling all registered devices for state changes.
    /// </summary>
    void StartPolling();

    /// <summary>
    /// Stops polling all registered devices.
    /// </summary>
    void StopPolling();

    /// <summary>
    /// Retrieves the current state of the specified device.
    /// </summary>
    /// <param name="deviceIndex">The index of the device.</param>
    /// <returns>The current state of the device.</returns>
    XInputState GetState(int deviceIndex);

    /// <summary>
    /// Retrieves the capabilities of the specified device.
    /// </summary>
    /// <param name="deviceIndex">The index of the device.</param>
    /// <returns>The capabilities of the device.</returns>
    XInputCapabilities GetCapabilities(int deviceIndex);

    /// <summary>
    /// Retrieves battery information for the specified device.
    /// </summary>
    /// <param name="deviceIndex">The index of the device.</param>
    /// <param name="deviceType">Specifies which type of device (e.g. gamepad, headset) on this user index to retrieve information for.</param>
    /// <returns>Battery information of the specified device.</returns>
    XInputBatteryInformation GetBatteryInformation(int deviceIndex, BatteryDeviceType deviceType);

    /// <summary>
    /// Retrieves a keystroke event from the specified device.
    /// </summary>
    /// <param name="deviceIndex">The index of the device.</param>
    /// <returns>Keystroke data from the device.</returns>
    XInputKeystroke GetKeystroke(int deviceIndex);

    /// <summary>
    /// Vibrates the given device's left and or right motor by the specified intensity.
    /// </summary>
    /// <param name="deviceIndex">The index of the device</param>
    /// <param name="leftMotorSpeedFraction">Fraction (0-1) indicating left motor intensity</param>
    /// <param name="rightMotorSpeedFraction">Fraction (0-1) indicating right motor intensity</param>
    /// <param name="duration">How long to vibrate for</param>
    void Vibrate(int deviceIndex, double leftMotorSpeedFraction, double rightMotorSpeedFraction, TimeSpan duration);

    /// <summary>
    /// Stops vibration of the given device.
    /// </summary>
    /// <param name="deviceIndex">The index of the device</param>
    void StopVibration(int deviceIndex);

    /// <summary>
    /// Gets the active gamepad device indexes.
    /// </summary>
    /// <returns></returns>
    IList<int> GetActiveDeviceIndices();
}
