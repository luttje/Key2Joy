using System;

namespace Key2Joy.LowLevelInput.XInput;

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
