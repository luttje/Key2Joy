﻿using KeyToJoy.Input.LowLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyToJoy.Mapping
{
    internal class MouseButtonInputBag : InputBag
    {
        public MouseState State { get; set; }
        public int LastX { get; set; }
        public int LastY { get; set; }
    }
}
