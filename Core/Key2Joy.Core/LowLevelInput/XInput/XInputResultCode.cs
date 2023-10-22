using System;

namespace Key2Joy.LowLevelInput.XInput;

/// <summary>
/// Represents the result codes for XInput operations.
///
/// Note that <see href="https://learn.microsoft.com/en-us/windows/win32/api/xinput/nf-xinput-xinputgetcapabilities"/> mentions:
/// "If the function fails, the return value is an error code defined in WinError.h. The function does not use SetLastError to set the calling thread's last-error code."
///
/// This may mean that we'll have to add those possible error codes here, or at least the ones we're interested in.
/// </summary>
public enum XInputResultCode : int
{
    /// <summary>
    /// The operation completed successfully.
    /// </summary>
    ERROR_SUCCESS = 0,

    /// <summary>
    /// The requested resource is empty or not found.
    /// </summary>
    ERROR_EMPTY = 4306,

    /// <summary>
    /// The XInput device is not connected or available.
    /// </summary>
    ERROR_DEVICE_NOT_CONNECTED = 1167
}

/// <summary>
/// Helps translate result codes from Native methods
/// </summary>
public static class XInputResult
{
    public static XInputResultCode FromResult(int result)
        => Enum.IsDefined(typeof(XInputResultCode), result)
            ? (XInputResultCode)result
            : throw new ArgumentOutOfRangeException(nameof(result), result, $"The result code {result} is not defined in {nameof(XInputResultCode)}.");
}
