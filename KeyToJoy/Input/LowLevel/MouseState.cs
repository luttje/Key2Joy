using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyToJoy.Input.LowLevel
{
    // Source: https://stackoverflow.com/a/34384189
    public enum MouseState
    {
        Move = 0x0200,
        LeftButtonDown = 0x0201,
        LeftButtonUp = 0x0202,
        RightButtonDown = 0x0204,
        RightButtonUp = 0x0205,
        Wheel = 0x020A,
        WheelH = 0x020E,
    }
}