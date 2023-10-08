using System;

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

            XButton1 = 5,
            XButton2 = 6,

            WheelUp = 8,
            WheelDown = 16
        }

        private static Buttons GetXButton(int data)
        {
            var xButton = (data >> 16) & 0xFFFF;

            if (xButton == 1)
                return Buttons.XButton1;
            else if (xButton == 2)
                return Buttons.XButton2;

            return Buttons.None;
        }

        public static Buttons ButtonsFromEvent(GlobalMouseHookEventArgs e, out bool isDown)
        {
            isDown = true;

            switch (e.MouseState)
            {
                case MouseState.LeftButtonUp:
                case MouseState.LeftButtonDown:
                    isDown = e.MouseState == MouseState.LeftButtonDown;
                    return Buttons.Left;
                case MouseState.RightButtonUp:
                case MouseState.RightButtonDown:
                    isDown = e.MouseState == MouseState.RightButtonDown;
                    return Buttons.Right;
                case MouseState.MiddleButtonUp:
                case MouseState.MiddleButtonDown:
                    isDown = e.MouseState == MouseState.MiddleButtonDown;
                    return Buttons.Middle;
                case MouseState.XButtonUp:
                case MouseState.XButtonDown:
                    isDown = e.MouseState == MouseState.XButtonDown;

                    var xButton = GetXButton(e.RawData.MouseData);

                    if (xButton != Buttons.None)
                        return xButton;

                    break;
            }

            Console.WriteLine($"Mouse button ({e.RawData}) not yet supported!");

            return Buttons.None;
        }
    }
}
