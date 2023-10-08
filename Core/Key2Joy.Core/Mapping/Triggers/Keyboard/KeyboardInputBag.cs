using Key2Joy.Contracts.Mapping;
using Key2Joy.LowLevelInput;
using System.Windows.Forms;

namespace Key2Joy.Mapping
{
    public class KeyboardInputBag : AbstractInputBag
    {
        public Keys Keys { get; set; }
        public KeyboardState State { get; set; }
    }
}
