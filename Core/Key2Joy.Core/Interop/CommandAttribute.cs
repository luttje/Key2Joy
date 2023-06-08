using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Interop
{
    [AttributeUsage(AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    public class CommandAttribute : Attribute
    {
        public byte Id { get; set; }

        public CommandAttribute(byte id)
        {
            Id = id;
        }
    }
}
