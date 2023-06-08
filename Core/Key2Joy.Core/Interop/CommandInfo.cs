using Jint.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Interop
{
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
                
                return Marshal.PtrToStructure(pointer, StructType);
            }
            finally
            {
                Marshal.FreeHGlobal(pointer);
            }
        }

        public byte[] CommandToBytes<CommandType>(CommandType command)
        {
            int size = Marshal.SizeOf(command);
            byte[] bytes = new byte[size];
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
}
