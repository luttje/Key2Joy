using Key2Joy.LowLevelInput;

namespace Key2Joy.Mapping;

public interface IPressState
{
    /// <summary>
    /// Which press state this is
    /// </summary>
    PressState PressState { get; set; }
}
