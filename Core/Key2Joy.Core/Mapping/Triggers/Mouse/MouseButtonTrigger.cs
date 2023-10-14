using System;
using System.Text.Json.Serialization;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Triggers;
using Key2Joy.LowLevelInput;

namespace Key2Joy.Mapping.Triggers.Mouse;

[Trigger(
    Description = "Mouse Button Event"
)]
public class MouseButtonTrigger : CoreTrigger, IPressState, IReturnInputHash, IEquatable<MouseButtonTrigger>
{
    public const string PREFIX_UNIQUE = nameof(MouseButtonTrigger);

    public LowLevelInput.Mouse.Buttons MouseButtons { get; set; }
    public PressState PressState { get; set; }

    [JsonConstructor]
    public MouseButtonTrigger(string name)
        : base(name)
    { }

    public override AbstractTriggerListener GetTriggerListener() => MouseButtonTriggerListener.Instance;

    public override string GetUniqueKey() => $"{PREFIX_UNIQUE}_{this.MouseButtons}";

    public static int GetInputHashFor(LowLevelInput.Mouse.Buttons mouseButtons) => (int)mouseButtons;

    public int GetInputHash() => GetInputHashFor(this.MouseButtons);

    // Keep Press and Release together while sorting
    public override int CompareTo(AbstractMappingAspect other)
    {
        if (other == null || other is not MouseButtonTrigger otherMouseTrigger)
        {
            return base.CompareTo(other);
        }

        return $"{this.MouseButtons}#{(int)this.PressState}"
            .CompareTo($"{otherMouseTrigger.MouseButtons}#{(int)otherMouseTrigger.PressState}");
    }

    public override bool Equals(object obj)
    {
        if (obj is not MouseButtonTrigger other)
        {
            return false;
        }

        return this.Equals(other);
    }

    public bool Equals(MouseButtonTrigger other) => this.MouseButtons == other.MouseButtons
            && this.PressState == other.PressState;

    public override string ToString()
    {
        var format = "(mouse) {1} {0}";
        return format.Replace("{0}", this.MouseButtons.ToString())
            .Replace("{1}", Enum.GetName(typeof(PressState), this.PressState));
    }
}
