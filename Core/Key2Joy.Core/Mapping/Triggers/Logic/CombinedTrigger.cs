using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Triggers;

namespace Key2Joy.Mapping.Triggers.Logic;

[Trigger(
    Description = "Multiple Triggers Combined",
    Visibility = MappingMenuVisibility.OnlyTopLevel,
    GroupName = "Logic Triggers",
    GroupImage = "application_xp_terminal"
)]
public class CombinedTrigger : CoreTrigger, IEquatable<CombinedTrigger>
{
    public const string PREFIX_UNIQUE = nameof(CombinedTrigger);

    public List<AbstractTrigger> Triggers { get; set; }

    [JsonConstructor]
    public CombinedTrigger(string name)
        : base(name)
    { }

    public override AbstractTriggerListener GetTriggerListener() => CombinedTriggerListener.Instance;

    public override bool Equals(object obj)
    {
        if (obj is not CombinedTrigger other)
        {
            return false;
        }

        return this.Equals(other);
    }

    public bool Equals(CombinedTrigger other) => this.Triggers.SequenceEqual(other.Triggers);

    public override string GetNameDisplay()
        => "(combined) " + string.Join(" + ", this.Triggers);
}
