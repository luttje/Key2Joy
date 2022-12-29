using Key2Joy.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Interop
{
    [Command(0x01)]
    [StructLayout(LayoutKind.Sequential)]
    public struct EnableCommand
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = FileSystem.MAX_PATH)] public string ProfilePath;
    }
}
