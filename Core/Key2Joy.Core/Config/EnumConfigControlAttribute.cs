using System;

namespace Key2Joy.Config;

/// <summary>
/// Only applied to <see cref="ConfigState"/>
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class EnumConfigControlAttribute : ConfigControlAttribute
{
    public Type EnumType { get; set; }
}
