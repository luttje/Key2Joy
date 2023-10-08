using System;

namespace Key2Joy.Interop;

[AttributeUsage(AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
public class CommandAttribute : Attribute
{
    public byte Id { get; set; }

    public CommandAttribute(byte id) => this.Id = id;
}
