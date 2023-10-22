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

    /// <summary>
    /// Group the aspect should be categorized under.
    /// </summary>
    public string GroupName { get; set; }

    /// <summary>
    /// Image for the group the aspect should be categorized under.
    /// </summary>
    public string GroupImage { get; set; }

    public override string ToString() => this.Description;

    public override int GetHashCode() => this.Description.GetHashCode();

    public int CompareTo(MappingAttribute other) => this.Description.CompareTo(other.Description);
}
