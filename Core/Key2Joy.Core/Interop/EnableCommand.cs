using Key2Joy.Util;
using System.Runtime.InteropServices;

namespace Key2Joy.Interop
{
    [Command(0x01)]
    [StructLayout(LayoutKind.Sequential)]
    public struct EnableCommand
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = FileSystem.MAX_PATH)] public string ProfilePath;
    }
}
