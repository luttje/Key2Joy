using System;
using System.Text.Json.Serialization;
using System.Windows.Forms;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Triggers;
using Key2Joy.LowLevelInput;

namespace Key2Joy.Mapping.Triggers.Keyboard;

[Trigger(
    Description = "Keyboard Event",
    GroupName = "Keyboard Triggers",
    GroupImage = "keyboard"
)]
public class KeyboardTrigger : CoreTrigger, IPressState, IProvideReverseAspect, IReturnInputHash, IEquatable<KeyboardTrigger>
{
    public const string PREFIX_UNIQUE = nameof(KeyboardTrigger);

    public Keys Keys { get; set; }

    public PressState PressState { get; set; }

    [JsonConstructor]
    public KeyboardTrigger(string name)
        : base(name)
    { }

    public override AbstractTriggerListener GetTriggerListener() => KeyboardTriggerListener.Instance;

    /// <inheritdoc/>
    public void MakeReverse(AbstractMappingAspect aspect)
        => CommonReverseAspect.MakeReversePressState(this, aspect);

    public static int GetInputHashFor(Keys keys) => (int)keys;

    public int GetInputHash() => GetInputHashFor(this.Keys);

    // Keep Press and Release together while sorting
    public override int CompareTo(AbstractMappingAspect other)
    {
        if (other == null || other is not KeyboardTrigger otherKeyboardTrigger)
        {
            return base.CompareTo(other);
        }

        return $"{this.Keys}#{(int)this.PressState}"
            .CompareTo($"{otherKeyboardTrigger.Keys}#{(int)otherKeyboardTrigger.PressState}");
    }

    public override bool Equals(object obj)
    {
        if (obj is not KeyboardTrigger other)
        {
            return false;
        }

        return this.Equals(other);
    }

    public bool Equals(KeyboardTrigger other) => this.Keys == other.Keys
            && this.PressState == other.PressState;

    public override string GetNameDisplay()
    {
        var format = "(keyboard) {1} {0}";
        return format.Replace("{0}", this.Keys.ToString())
            .Replace("{1}", Enum.GetName(typeof(PressState), this.PressState));
    }

    public KeyboardState GetKeyboardState()
    {
        if (this.PressState == PressState.Press)
        {
            return KeyboardState.KeyDown;
        }

        return KeyboardState.KeyUp;
    }
}
