using System;

namespace Key2Joy.Config;

/// <summary>
/// Only applied to <see cref="ConfigManager"/>
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class TextConfigControlAttribute : ConfigControlAttribute
{
    public int MaxLength { get; set; }
}
