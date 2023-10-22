using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Triggers;

namespace Key2Joy.Mapping.Triggers;

[Trigger(
    Description = "Disabled Trigger",
    NameFormat = DisabledNameFormat,
    Visibility = MappingMenuVisibility.Never,
    GroupName = "Requires Attention",
    GroupImage = "cross"
)]
public class DisabledTrigger : CoreTrigger
{
    private const string DisabledNameFormat = "The trigger '{0}' was unavailable upon loading Key2Joy. The error that caused this was: {1}";
    public string TriggerName { get; set; }

    public DisabledTrigger(string name)
        : base(name)
    { }

    public override AbstractTriggerListener GetTriggerListener() => DisabledTriggerListener.Instance;

    /// <inheritdoc/>
    public override string GetNameDisplay()
        => DisabledNameFormat
            .Replace("{0}", this.TriggerName)
            .Replace("{1}", this.Name);

    /// <inheritdoc/>
    public override bool Equals(object obj)
    {
        if (obj is not DisabledTrigger trigger)
        {
            return false;
        }

        return trigger.TriggerName == this.TriggerName;
    }
}
