using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Key2Joy.Contracts.Mapping.Triggers;

namespace Key2Joy.Mapping.Triggers.GamePad;

[Trigger(
    Description = "GamePad Stick Move Event"
)]
public class GamePadStickTrigger : CoreTrigger, IReturnInputHash, IEquatable<GamePadStickTrigger>
{
    public const string PREFIX_UNIQUE = nameof(GamePadStickTrigger);

    /// <summary>
    /// Which gamepad index activates this trigger?
    /// </summary>
    public int GamePadIndex { get; set; }

    /// <summary>
    /// Which stick activates this trigger?
    /// </summary>
    public GamePadStickSide StickSide { get; set; }

    /// <summary>
    /// With what margin should be moved to trigger?
    /// If null then trigger will be fired on any move (taking into account the default deadzone).
    /// </summary>
    public ExactAxisDirection? DeltaMargin { get; set; } = null;

    [JsonConstructor]
    public GamePadStickTrigger(string name)
        : base(name)
    { }

    /// <inheritdoc/>
    public override AbstractTriggerListener GetTriggerListener() => GamePadStickTriggerListener.Instance;

    /// <inheritdoc/>
    public override string GetUniqueKey()
        => $"{PREFIX_UNIQUE}_{this.GamePadIndex}_{this.StickSide}_{(this.DeltaMargin != null ? this.DeltaMargin : "Any")}";

    /// <inheritdoc/>
    public int GetInputHash()
        => this.GetHashCode();

    /// <inheritdoc/>
    public bool Equals(GamePadStickTrigger other)
        => other is not null
        && this.StickSide == other.StickSide
        && EqualityComparer<ExactAxisDirection?>.Default.Equals(this.DeltaMargin, other.DeltaMargin);

    /// <inheritdoc/>
    public override bool Equals(object obj)
    {
        if (obj is not GamePadStickTrigger other)
        {
            return false;
        }

        return this.Equals(other);
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        var axis = this.DeltaMargin != null ? Enum.GetName(typeof(ExactAxisDirection), this.DeltaMargin) : "Any";
        return $"(gamepad) Move #{this.GamePadIndex} {this.StickSide} Stick {axis}";
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        var hashCode = -1723086675;
        hashCode = (hashCode * -1521134295) + this.StickSide.GetHashCode();
        hashCode = (hashCode * -1521134295) + this.DeltaMargin.GetHashCode();
        return hashCode;
    }
}
