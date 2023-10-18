using System.Runtime.InteropServices;

namespace Key2Joy.LowLevelInput.XInput;

/// <summary>
/// Handles XInput functionality by calling native functions.
/// </summary>
/// <seealso href="https://www.codeproject.com/Articles/492473/Using-XInput-to-access-an-Xbox-360-Controller-in-M"/>
public class NativeXInput : IXInput
{
    [DllImport("xinput1_4.dll", EntryPoint = "XInputGetState")]
    private static extern int InternalXInputGetState(int dwUserIndex, ref XInputState pState);

    [DllImport("xinput1_4.dll", EntryPoint = "XInputSetState")]
    private static extern int InternalXInputSetState(int dwUserIndex, ref XInputVibration pVibration);

    [DllImport("xinput1_4.dll", EntryPoint = "XInputGetCapabilities")]
    private static extern int InternalXInputGetCapabilities(int dwUserIndex, int dwFlags, ref XInputCapabilities pCapabilities);

    [DllImport("xinput1_4.dll", EntryPoint = "XInputGetBatteryInformation")]
    private static extern int InternalXInputGetBatteryInformation(int dwUserIndex, byte devType, ref XInputBatteryInformation pBatteryInformation);

    [DllImport("xinput1_4.dll", EntryPoint = "XInputGetKeystroke")]
    private static extern int InternalXInputGetKeystroke(int dwUserIndex, int dwReserved, ref XInputKeystroke pKeystroke);

    /// <inheritdoc />
    public XInputResultCode XInputGetState(int userIndex, ref XInputState inputState)
        => XInputResult.FromResult(InternalXInputGetState(userIndex, ref inputState));

    /// <inheritdoc />
    public XInputResultCode XInputSetState(int userIndex, ref XInputVibration vibrationInfo)
        => XInputResult.FromResult(InternalXInputSetState(userIndex, ref vibrationInfo));

    /// <inheritdoc />
    public XInputResultCode XInputGetCapabilities(int userIndex, CapabilityRequestFlag flags, ref XInputCapabilities capabilities)
        => XInputResult.FromResult(InternalXInputGetCapabilities(userIndex, (int)flags, ref capabilities));

    /// <inheritdoc />
    public XInputResultCode XInputGetBatteryInformation(int userIndex, BatteryDeviceType deviceType, ref XInputBatteryInformation batteryInformation)
        => XInputResult.FromResult(InternalXInputGetBatteryInformation(userIndex, (byte)deviceType, ref batteryInformation));

    /// <inheritdoc />
    public XInputResultCode XInputGetKeystroke(int userIndex, int reserved, ref XInputKeystroke keystrokeData)
        => XInputResult.FromResult(InternalXInputGetKeystroke(userIndex, reserved, ref keystrokeData));

    /// <summary>
    /// Get capabilities for the specified controller.
    /// </summary>
    /// <param name="userIndex"></param>
    /// <param name="capabilities"></param>
    /// <returns></returns>
    public XInputResultCode XInputGetCapabilities(int userIndex, ref XInputCapabilities capabilities)
        => XInputResult.FromResult(InternalXInputGetCapabilities(userIndex, (int)CapabilityRequestFlag.XINPUT_FLAG_GAMEPAD, ref capabilities));
}
