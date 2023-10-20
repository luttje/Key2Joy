using System;
using System.Text.Json.Serialization;
using Key2Joy.Contracts.Mapping.Triggers;

namespace Key2Joy.Mapping.Triggers.Mouse;

[Trigger(
    Description = "Mouse Move Event"
)]
public class MouseMoveTrigger : CoreTrigger, IReturnInputHash
{
    public const string PREFIX_UNIQUE = nameof(MouseMoveTrigger);

    public AxisDirection AxisBinding { get; set; }

    [JsonConstructor]
    public MouseMoveTrigger(string name)
        : base(name)
    { }

    public override AbstractTriggerListener GetTriggerListener() => MouseMoveTriggerListener.Instance;

    public override string GetUniqueKey() => $"{PREFIX_UNIQUE}_{this.AxisBinding}";

    public static int GetInputHashFor(AxisDirection axisBinding) => (int)axisBinding;

    public int GetInputHash() => GetInputHashFor(this.AxisBinding);

    public override bool Equals(object obj)
    {
        if (obj is not MouseMoveTrigger other)
        {
            return false;
        }

        return this.AxisBinding == other.AxisBinding;
    }

    public override string ToString()
    {
        var axis = Enum.GetName(typeof(AxisDirection), this.AxisBinding);
        return $"(mouse) Move {axis}";
    }
}
