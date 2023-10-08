using Key2Joy.Contracts.Mapping;

namespace Key2Joy.Mapping
{
    public class MouseMoveInputBag : AbstractInputBag
    {
        public int DeltaX { get; set; }
        public int DeltaY { get; set; }
    }
}
