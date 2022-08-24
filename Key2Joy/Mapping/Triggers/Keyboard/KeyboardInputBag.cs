using Key2Joy.LowLevelInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Key2Joy.Mapping
{
    internal class KeyboardInputBag : IInputBag
    {
        public Keys Keys { get; set; }
        public KeyboardState State { get; set; }
    }
}
