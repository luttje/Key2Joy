using KeyToJoy.Input.LowLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyToJoy.Mapping
{
    internal class KeyboardInputBag : InputBag
    {
        public Keys Keys { get; set; }
        public KeyboardState State { get; set; }
    }
}
