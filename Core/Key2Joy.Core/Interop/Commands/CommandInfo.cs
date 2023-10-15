using System;
using System.Runtime.InteropServices;

namespace Key2Joy.Interop.Commands;

public class CommandInfo
{
    public byte Id { get; set; }
    public Type StructType { get; set; }

    public object CommandFromBytes(byte[] bytes)
    {
        var pointer = IntPtr.Zero;

        try
        {
            pointer = Marshal.AllocHGlobal(bytes.Length);
            Marshal.Copy(bytes, 0, pointer, bytes.Length);

            return Marshal.PtrToStructure(pointer, this.StructType);
        }
        finally
        {
            Marshal.FreeHGlobal(pointer);
        }
    }

    public static byte[] CommandToBytes<CommandType>(CommandType command)
    {
        var size = Marshal.SizeOf<CommandType>();
        var bytes = new byte[size];
        var pointer = IntPtr.Zero;

        try
        {
            pointer = Marshal.AllocHGlobal(bytes.Length);
            Marshal.StructureToPtr(command, pointer, true);
            Marshal.Copy(pointer, bytes, 0, size);

            return bytes;
        }
        finally
        {
            Marshal.FreeHGlobal(pointer);
        }
    }
}
