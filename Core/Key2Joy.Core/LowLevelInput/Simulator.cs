using System.Runtime.InteropServices;

namespace Key2Joy.LowLevelInput;

public class Simulator
{
    /// <summary>
    /// Declaration of external SendInput method
    /// </summary>
    [DllImport("user32.dll")]
    public static extern uint SendInput(
        uint nInputs,
        [MarshalAs(UnmanagedType.LPArray), In] INPUT[] pInputs,
        int cbSize);

    [DllImport("user32.dll")]
    private static extern int GetSystemMetrics(SystemMetric smIndex);

    public static int CalculateAbsoluteCoordinateX(int x) => x * 65536 / GetSystemMetrics(SystemMetric.SM_CXSCREEN);

    public static int CalculateAbsoluteCoordinateY(int y) => y * 65536 / GetSystemMetrics(SystemMetric.SM_CYSCREEN);

    private enum SystemMetric
    {
        SM_CXSCREEN = 0,
        SM_CYSCREEN = 1,
    }

    // Declare the INPUT struct
    [StructLayout(LayoutKind.Sequential)]
    public struct INPUT
    {
        public uint type;
        public InputUnion U;
        public static int Size => Marshal.SizeOf(typeof(INPUT));
    }

    // Declare the InputUnion struct
    [StructLayout(LayoutKind.Explicit)]
    public struct InputUnion
    {
        [FieldOffset(0)]
        public SimulatedMouse.MOUSEINPUT mi;

        [FieldOffset(0)]
        public SimulatedKeyboard.KEYBDINPUT ki;

        [FieldOffset(0)]
        public HARDWAREINPUT hi;
    }

    /// <summary>
    /// Define HARDWAREINPUT struct
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct HARDWAREINPUT
    {
        public int uMsg;
        public short wParamL;
        public short wParamH;
    }
}
