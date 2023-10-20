using Key2Joy.Contracts.Mapping.Triggers;

namespace Key2Joy.Mapping.Triggers;

[Trigger(
    Description = "Disabled Trigger",
    NameFormat = DisabledNameFormat,
    Visibility = Contracts.Mapping.MappingMenuVisibility.Never
)]
public class DisabledTrigger : CoreTrigger
{
    private const string DisabledNameFormat = "The trigger '{0}' was unavailable upon loading Key2Joy. We have replaced it with this placeholder.";
    public string TriggerName { get; set; }

    public DisabledTrigger(string name)
        : base(name)
    { }

    public override AbstractTriggerListener GetTriggerListener() => DisabledTriggerListener.Instance;

    public override bool Equals(object obj)
    {
        if (obj is not DisabledTrigger trigger)
        {
            return false;
        }

        return trigger.TriggerName == this.TriggerName;
    }
}
