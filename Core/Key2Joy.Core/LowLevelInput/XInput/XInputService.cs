using System;
using System.Collections.Generic;
using System.Threading;
using Timer = System.Timers.Timer;

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
}

/// <summary>
/// Provides data for the DeviceStateChanged event.
/// </summary>
public class DeviceStateChangedEventArgs : EventArgs
{
    /// <summary>
    /// Gets the index of the device that has changed state.
    /// </summary>
    public int DeviceIndex { get; }

    /// <summary>
    /// Gets the new state of the device.
    /// </summary>
    public XInputState NewState { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DeviceStateChangedEventArgs"/> class.
    /// </summary>
    /// <param name="deviceIndex">The index of the device that has changed state.</param>
    /// <param name="newState">The new state of the device.</param>
    public DeviceStateChangedEventArgs(int deviceIndex, XInputState newState)
    {
        this.DeviceIndex = deviceIndex;
        this.NewState = newState;
    }
}

public class XInputService : IXInputService
{
    private const int UpdateIntervalInMs = 20;

    /// <summary>
    /// Called when the state of a device changes.
    /// </summary>
    public event EventHandler<DeviceStateChangedEventArgs> StateChanged;

    private readonly IXInput xInputInstance;
    private readonly HashSet<int> registeredDevices;
    private readonly Dictionary<int, Timer> vibrationTimers;
    private Thread pollingThread;
    private bool isPolling;

    public XInputService(IXInput xinputInstance = null)
    {
        this.xInputInstance = xinputInstance ?? new NativeXInput();
        this.registeredDevices = new();
        this.vibrationTimers = new();
    }

    /// <inheritdoc/>
    public void RegisterDevice(int deviceIndex)
    {
        lock (this.registeredDevices)
        {
            this.registeredDevices.Add(deviceIndex);
        }
    }

    /// <inheritdoc/>
    public void StartPolling()
    {
        if (this.isPolling)
        {
            return;
        }

        this.isPolling = true;

        this.pollingThread = new Thread(() =>
        {
            while (this.isPolling)
            {
                lock (this.registeredDevices)
                {
                    foreach (var deviceIndex in this.registeredDevices)
                    {
                        var newState = new XInputState();
                        var resultCode = this.xInputInstance.XInputGetState(deviceIndex, ref newState);

                        if (resultCode == XInputResultCode.ERROR_SUCCESS)
                        {
                            StateChanged?.Invoke(this, new DeviceStateChangedEventArgs(deviceIndex, newState));
                        }

                        // Add a delay to avoid hammering the IXInput instance too rapidly
                        Thread.Sleep(UpdateIntervalInMs);
                    }
                }
            }
        });

        this.pollingThread.Start();
    }

    /// <inheritdoc/>
    public void StopPolling()
    {
        this.isPolling = false;
        this.pollingThread?.Join();
        this.pollingThread = null;
    }

    /// <inheritdoc/>
    public XInputState GetState(int deviceIndex)
    {
        var inputState = new XInputState();

        this.xInputInstance.XInputGetState(deviceIndex, ref inputState);

        return inputState;
    }

    /// <inheritdoc/>
    public XInputCapabilities GetCapabilities(int deviceIndex)
    {
        var capabilities = new XInputCapabilities();

        this.xInputInstance.XInputGetCapabilities(deviceIndex, ref capabilities);

        return capabilities;
    }

    /// <inheritdoc/>
    public XInputBatteryInformation GetBatteryInformation(int deviceIndex, BatteryDeviceType deviceType)
    {
        var batteryInformation = new XInputBatteryInformation();

        this.xInputInstance.XInputGetBatteryInformation(deviceIndex, deviceType, ref batteryInformation);

        return batteryInformation;
    }

    /// <inheritdoc/>
    public XInputKeystroke GetKeystroke(int deviceIndex)
    {
        var keystrokeData = new XInputKeystroke();

        this.xInputInstance.XInputGetKeystroke(deviceIndex, 0, ref keystrokeData);

        return keystrokeData;
    }

    /// <inheritdoc/>
    public void Vibrate(int deviceIndex, double leftMotorSpeedFraction, double rightMotorSpeedFraction, TimeSpan duration)
    {
        var vibrationInfo = new XInputVibration(leftMotorSpeedFraction, rightMotorSpeedFraction);
        this.xInputInstance.XInputSetState(deviceIndex, ref vibrationInfo);

        // If a timer for this device is already running, stop it first
        if (this.vibrationTimers.ContainsKey(deviceIndex))
        {
            var existingTimer = this.vibrationTimers[deviceIndex];
            existingTimer.Stop();
            existingTimer.Dispose();
        }

        // Create a new timer for this vibration
        var timer = new Timer(duration.TotalMilliseconds);
        timer.Elapsed += (sender, e) => this.StopVibration(deviceIndex);
        timer.AutoReset = false;
        timer.Start();

        this.vibrationTimers[deviceIndex] = timer;
    }

    /// <inheritdoc/>
    public void StopVibration(int deviceIndex)
    {
        var vibrationInfo = new XInputVibration(0, 0);
        this.xInputInstance.XInputSetState(deviceIndex, ref vibrationInfo);

        if (this.vibrationTimers.ContainsKey(deviceIndex))
        {
            var timer = this.vibrationTimers[deviceIndex];
            timer.Stop();
            timer.Dispose();
            this.vibrationTimers.Remove(deviceIndex);
        }
    }
}
