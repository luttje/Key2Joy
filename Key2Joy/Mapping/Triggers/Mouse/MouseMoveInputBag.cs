﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Mapping
{
    internal class MouseMoveInputBag : InputBag
    {
        public int DeltaX { get; set; }
        public int DeltaY { get; set; }
    }
}