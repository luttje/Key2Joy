using Key2Joy.Contracts.Mapping.Triggers;

namespace Key2Joy.Mapping.Triggers.Mouse
{
    public class MouseMoveInputBag : AbstractInputBag
    {
        public int DeltaX { get; set; }
        public int DeltaY { get; set; }
    }
}
