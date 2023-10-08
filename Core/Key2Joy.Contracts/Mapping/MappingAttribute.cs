using System;

namespace Key2Joy.Contracts.Mapping;

[AttributeUsage(AttributeTargets.Class)]
public abstract class MappingAttribute : Attribute, IComparable<MappingAttribute>
{
    /// <summary>
    /// Customizable name format for the action/trigger
    /// </summary>
    public string NameFormat { get; set; }

    /// <summary>
    /// Description for the action/trigger
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// When this action should be visibile in menu's.
    /// </summary>
    public MappingMenuVisibility Visibility { get; set; } = MappingMenuVisibility.Always;

    public override string ToString() => this.Description;

    public override int GetHashCode() => this.Description.GetHashCode();

    public int CompareTo(MappingAttribute other) => this.Description.CompareTo(other.Description);
}
