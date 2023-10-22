using System;
using System.Runtime.InteropServices;

namespace Key2Joy.LowLevelInput.XInput;

/// <summary>
/// Represents motor speed levels for the vibration function of a controller.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct XInputVibration
{
    private const double MaxMotorSpeed = 65535.0;

    /// <summary>
    /// Speed of the left motor. Valid values are in the range 0 to 65,535.
    /// Zero signifies no motor use; 65,535 signifies 100 percent motor use.
    /// </summary>
    [MarshalAs(UnmanagedType.I2)]
    public ushort LeftMotorSpeed;

    /// <summary>
    /// Speed of the right motor. Valid values are in the range 0 to 65,535.
    /// Zero signifies no motor use; 65,535 signifies 100 percent motor use.
    /// </summary>
    [MarshalAs(UnmanagedType.I2)]
    public ushort RightMotorSpeed;

    /// <summary>
    /// Construct a new XInputVibration struct with the specified motor speeds as fractions.
    /// </summary>
    /// <param name="leftMotorSpeedFraction"></param>
    /// <param name="rightMotorSpeedFraction"></param>
    public XInputVibration(double leftMotorSpeedFraction, double rightMotorSpeedFraction)
    {
        this.LeftMotorSpeed = GetTrueMotorSpeed(leftMotorSpeedFraction);
        this.RightMotorSpeed = GetTrueMotorSpeed(rightMotorSpeedFraction);
    }

    /// <summary>
    /// Construct a new XInputVibration struct with the specified motor speeds.
    /// </summary>
    /// <param name="leftMotorSpeed"></param>
    /// <param name="rightMotorSpeed"></param>
    public XInputVibration(ushort leftMotorSpeed, ushort rightMotorSpeed)
    {
        this.LeftMotorSpeed = leftMotorSpeed;
        this.RightMotorSpeed = rightMotorSpeed;
    }

    /// <summary>
    /// Get the true speed for a motor by specifying a fraction of the maximum speed.
    /// </summary>
    /// <param name="motorSpeed">The speed for a motor as a fraction of the maximum speed.</param>
    /// <returns>The true speed for a motor.</returns>
    public static ushort GetTrueMotorSpeed(double motorSpeed)
    {
        if (motorSpeed is < 0.0 or > 1.0)
        {
            throw new ArgumentOutOfRangeException(nameof(motorSpeed), "Speed must be in the range 0.0 to 1.0.");
        }

        return (ushort)(MaxMotorSpeed * motorSpeed);
    }
}
