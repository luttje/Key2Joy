using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Interop
{
    [Command(0x02)]
    [StructLayout(LayoutKind.Sequential)]
    public struct DisableCommand
    {
    }
}
