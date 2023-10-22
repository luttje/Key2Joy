using System;
using System.Text.Json.Serialization;
using Key2Joy.Contracts.Mapping.Triggers;

namespace Key2Joy.Mapping.Triggers.Mouse;

[Trigger(
    Description = "Mouse Move Event",
    GroupName = "Mouse Triggers",
    GroupImage = "mouse"
)]
public class MouseMoveTrigger : CoreTrigger, IReturnInputHash
{
    public const string PREFIX_UNIQUE = nameof(MouseMoveTrigger);

    /// <summary>
    /// The direction that the mouse must move in order to trigger this action.
    /// </summary>
    public AxisDirection AxisBinding { get; set; }

    [JsonConstructor]
    public MouseMoveTrigger(string name)
        : base(name)
    { }

    public override AbstractTriggerListener GetTriggerListener()
        => MouseMoveTriggerListener.Instance;

    public static int GetInputHashFor(AxisDirection axisBinding)
        => axisBinding.GetHashCode();

    public int GetInputHash()
        => GetInputHashFor(this.AxisBinding);

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
