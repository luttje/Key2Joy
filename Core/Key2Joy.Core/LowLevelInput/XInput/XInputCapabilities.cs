using System;
using System.Runtime.InteropServices;

namespace Key2Joy.LowLevelInput.XInput;

/// <summary>
/// Describes the capabilities of a connected controller.
/// The <see cref="IXInput.XInputGetCapabilities"/> function returns this.
/// <see href="https://learn.microsoft.com/en-us/windows/win32/api/xinput/ns-xinput-xinput_capabilities"/>
/// </summary>
[StructLayout(LayoutKind.Explicit)]
public struct XInputCapabilities
{
    /// <summary>
    /// Device type. <see cref="Type"/>
    /// </summary>
    [MarshalAs(UnmanagedType.I1)]
    [FieldOffset(0)]
    public byte RawType;

    /// <summary>
    /// Subtype of the game controller. <see cref="SubType"/>
    /// </summary>
    [MarshalAs(UnmanagedType.I1)]
    [FieldOffset(1)]
    public byte RawSubType;

    /// <summary>
    /// Features of the controller.
    /// </summary>
    [MarshalAs(UnmanagedType.I2)]
    [FieldOffset(2)]
    public short Flags;

    /// <summary>
    /// XINPUT_GAMEPAD structure that describes available controller features
    /// and control resolutions.
    /// </summary>
    [FieldOffset(4)]
    public XInputGamepad Gamepad;

    /// <summary>
    /// XINPUT_VIBRATION structure that describes available vibration functionality
    /// and resolutions.
    /// </summary>
    [FieldOffset(16)]
    public XInputVibration Vibration;

    /// <summary>
    /// Device type.
    /// </summary>
    public readonly DeviceType Type
        => (DeviceType)this.RawType;

    /// <summary>
    /// Device sub type.
    /// </summary>
    public readonly DeviceSubType SubType
        => Enum.IsDefined(typeof(DeviceSubType), this.RawSubType)
            ? (DeviceSubType)this.RawSubType
            : DeviceSubType.XINPUT_DEVSUBTYPE_GAMEPAD;

    /// <summary>
    /// The capability flags.
    /// </summary>
    public readonly CapabilityFlags CapabilityFlags
        => (CapabilityFlags)this.Flags;
}
