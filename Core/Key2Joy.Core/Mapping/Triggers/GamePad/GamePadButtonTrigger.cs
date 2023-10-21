using System;
using System.Text.Json.Serialization;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Triggers;
using Key2Joy.LowLevelInput;
using Key2Joy.LowLevelInput.XInput;

namespace Key2Joy.Mapping.Triggers.GamePad;

[Trigger(
    Description = "GamePad Button Event"
)]
public class GamePadButtonTrigger : CoreTrigger, IPressState, IProvideReverseAspect, IReturnInputHash, IEquatable<GamePadButtonTrigger>
{
    public const string PREFIX_UNIQUE = nameof(GamePadButtonTrigger);

    public GamePadButton Button { get; set; }

    public PressState PressState { get; set; }

    [JsonConstructor]
    public GamePadButtonTrigger(string name)
        : base(name)
    { }

    public override AbstractTriggerListener GetTriggerListener()
        => GamePadButtonTriggerListener.Instance;

    /// <inheritdoc/>
    public void MakeReverse(AbstractMappingAspect aspect)
        => CommonReverseAspect.MakeReversePressState(this, aspect);

    public static int GetInputHashFor(GamePadButton button)
        => button.GetHashCode();

    public int GetInputHash()
        => GetInputHashFor(this.Button);

    // Keep Press and Release together while sorting
    public override int CompareTo(AbstractMappingAspect other)
    {
        if (other == null || other is not GamePadButtonTrigger otherGamePadButtonTrigger)
        {
            return base.CompareTo(other);
        }

        return $"{this.Button}#{(int)this.PressState}"
            .CompareTo($"{otherGamePadButtonTrigger.Button}#{(int)otherGamePadButtonTrigger.PressState}");
    }

    public override bool Equals(object obj)
    {
        if (obj is not GamePadButtonTrigger other)
        {
            return false;
        }

        return this.Equals(other);
    }

    public bool Equals(GamePadButtonTrigger other) => this.Button == other.Button
            && this.PressState == other.PressState;

    public override string ToString()
    {
        var format = "(gamepad) {1} {0}";
        return format.Replace("{0}", this.Button.ToString())
            .Replace("{1}", Enum.GetName(typeof(PressState), this.PressState));
    }
}
