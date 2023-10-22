using Key2Joy.Contracts.Mapping.Triggers;

namespace Key2Joy.Mapping.Triggers.GamePad;

public class GamePadStickInputBag : AbstractInputBag
{
    /// <summary>
    /// The raw data from the gamepad for the left stick.
    /// </summary>
    public ExactAxisDirection LeftStickDelta { get; set; }

    /// <summary>
    /// The raw data from the gamepad for the right stick.
    /// </summary>
    public ExactAxisDirection RightStickDelta { get; set; }
}
