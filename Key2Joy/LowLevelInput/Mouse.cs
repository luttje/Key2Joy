using Linearstar.Windows.RawInput.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.LowLevelInput
{
    public class Mouse
    {
        public enum MoveType
        {
            Relative = 0,
            Absolute = 1
        }
        
        public enum Buttons
        {
            None = 0,
            Left = 1,
            Right = 2,
            Middle = 4,
            WheelUp = 8,
            WheelDown = 16
        }

        public static Buttons ButtonsFromRaw(RawMouse rawMouse, out bool isDown)
        {
            isDown = true;
            
            switch (rawMouse.Buttons)
            {
                case RawMouseButtonFlags.LeftButtonUp:
                case RawMouseButtonFlags.LeftButtonDown:
                    isDown = rawMouse.Buttons == RawMouseButtonFlags.LeftButtonDown;
                    return Buttons.Left;
                case RawMouseButtonFlags.RightButtonUp:
                case RawMouseButtonFlags.RightButtonDown:
                    isDown = rawMouse.Buttons == RawMouseButtonFlags.RightButtonDown;
                    return Buttons.Right;
                case RawMouseButtonFlags.MiddleButtonUp:
                case RawMouseButtonFlags.MiddleButtonDown:
                    isDown = rawMouse.Buttons == RawMouseButtonFlags.MiddleButtonDown;
                    return Buttons.Middle;
                case RawMouseButtonFlags.MouseWheel:
                    if (rawMouse.ButtonData >= 0)
                        return Buttons.WheelUp;
                    else
                        return Buttons.WheelDown;
                // TODO: Support these
                //case RawMouseButtonFlags.Button4Up:
                //case RawMouseButtonFlags.Button4Down:
                //    this.mouseButtons = MouseButtons.XButton1;
                //    break;
                //case RawMouseButtonFlags.Button5Up:
                //case RawMouseButtonFlags.Button5Down:
                //    this.mouseButtons = MouseButtons.XButton2;
                //    break;
;
            }

            throw new NotImplementedException($"Mouse button ({rawMouse.Buttons}) not yet supported!");
        }

        public static Buttons ButtonsFromState(MouseState mouseState, out bool isDown)
        {
            isDown = true;
            
            switch (mouseState)
            {
                case MouseState.LeftButtonUp:
                case MouseState.LeftButtonDown:
                    isDown = mouseState == MouseState.LeftButtonDown;
                    return Buttons.Left;
                case MouseState.RightButtonUp:
                case MouseState.RightButtonDown:
                    isDown = mouseState == MouseState.RightButtonDown;
                    return Buttons.Right;
                case MouseState.MiddleButtonUp:
                case MouseState.MiddleButtonDown:
                    isDown = mouseState == MouseState.MiddleButtonDown;
                    return Buttons.Middle;
            }
            
            throw new NotImplementedException($"Mouse button ({mouseState}) not yet supported!");
        }
    }
}
