using System.Windows.Forms;
using Key2Joy.Contracts.Mapping.Triggers;
using Key2Joy.LowLevelInput;

namespace Key2Joy.Mapping.Triggers.Keyboard
{
    public class KeyboardInputBag : AbstractInputBag
    {
        public Keys Keys { get; set; }
        public KeyboardState State { get; set; }
    }
}
