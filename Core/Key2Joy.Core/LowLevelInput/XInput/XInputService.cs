using System;
using System.Collections.Generic;
using System.Threading;
using Timer = System.Timers.Timer;

namespace Key2Joy.LowLevelInput.XInput;

public class XInputService : IXInputService
{
    private const int UpdateIntervalInMs = 20;

    /// <summary>
    /// Called when the state of a device changes.
    /// </summary>
    public event EventHandler<DeviceStateChangedEventArgs> StateChanged;

    /// <summary>
    /// Called whenever the device sends a new packet
    /// </summary>
    public event EventHandler<DevicePacketReceivedEventArgs> PacketReceived;

    private readonly IXInput xInputInstance;
    private readonly HashSet<int> registeredDevices;
    private readonly Dictionary<int, Timer> vibrationTimers;
    private readonly Dictionary<int, XInputState> lastStates;
    private Thread pollingThread;
    private bool isPolling;

    public XInputService(IXInput xinputInstance = null)
    {
        this.xInputInstance = xinputInstance ?? new NativeXInput();
        this.registeredDevices = new();
        this.vibrationTimers = new();
        this.lastStates = new();
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
                            this.PacketReceived?.Invoke(this, new DevicePacketReceivedEventArgs(deviceIndex, newState));
                            var hasLastState = this.lastStates.TryGetValue(deviceIndex, out var lastState);

                            if (!hasLastState || !lastState.Equals(newState))
                            {
                                if (!hasLastState)
                                {
                                    this.lastStates.Add(deviceIndex, newState);
                                }
                                else
                                {
                                    this.lastStates[deviceIndex] = newState;
                                }

                                this.StateChanged?.Invoke(this, new DeviceStateChangedEventArgs(deviceIndex, newState));
                            }
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
