using System.Runtime.InteropServices;
using Key2Joy.Util;

namespace Key2Joy.Interop.Commands;

[Command(0x01)]
[StructLayout(LayoutKind.Sequential)]
public struct EnableCommand
{
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = FileSystem.MAX_PATH)] public string ProfilePath;
}
