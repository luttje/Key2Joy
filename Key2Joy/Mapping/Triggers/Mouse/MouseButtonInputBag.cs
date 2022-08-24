using Key2Joy.LowLevelInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Mapping
{
    internal class MouseButtonInputBag : IInputBag
    {
        public MouseState State { get; set; }
        public bool IsDown { get; set; }
        public int LastX { get; set; }
        public int LastY { get; set; }
    }
}
