using Key2Joy.Contracts.Mapping.Triggers;

namespace Key2Joy.Mapping.Triggers.GamePad;

public class GamePadTriggerInputBag : AbstractInputBag
{
    /// <summary>
    /// How much the left trigger was pulled back.
    /// </summary>
    public float LeftTriggerDelta { get; set; }

    /// <summary>
    /// How much the right trigger was pulled back.
    /// </summary>
    public float RightTriggerDelta { get; set; }
}
