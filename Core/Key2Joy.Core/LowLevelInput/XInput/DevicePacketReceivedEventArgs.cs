namespace Key2Joy.LowLevelInput.XInput;

/// <summary>
/// Provides data for the DevicePacketReceivedEventArgs event.
/// </summary>
public class DevicePacketReceivedEventArgs
{
    /// <summary>
    /// Gets the index of the device that has sent a packet.
    /// </summary>
    public int DeviceIndex { get; }

    /// <summary>
    /// Gets the state of the device.
    /// </summary>
    public XInputState State { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DevicePacketReceivedEventArgs"/> class.
    /// </summary>
    /// <param name="deviceIndex">The index of the device that has sent a packet.</param>
    /// <param name="state">The state of the device.</param>
    public DevicePacketReceivedEventArgs(int deviceIndex, XInputState state)
    {
        this.DeviceIndex = deviceIndex;
        this.State = state;
    }
}
