using Key2Joy.Contracts.Mapping;
using Key2Joy.LowLevelInput;

namespace Key2Joy.Mapping
{
    public class MouseButtonInputBag : AbstractInputBag
    {
        public MouseState State { get; set; }
        public bool IsDown { get; set; }
        public int LastX { get; set; }
        public int LastY { get; set; }
    }
}
