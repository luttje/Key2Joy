using Key2Joy.Contracts.Mapping.Triggers;
using Key2Joy.LowLevelInput;

namespace Key2Joy.Mapping.Triggers.Mouse;

public class MouseButtonInputBag : AbstractInputBag
{
    public MouseState State { get; set; }
    public bool IsDown { get; set; }
    public int LastX { get; set; }
    public int LastY { get; set; }
}
