using System.Runtime.InteropServices;

namespace Key2Joy.Interop.Commands;

[Command(0x02)]
[StructLayout(LayoutKind.Sequential)]
public struct DisableCommand
{
}
