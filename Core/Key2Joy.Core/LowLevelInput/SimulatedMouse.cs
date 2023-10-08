using System;
using System.Runtime.InteropServices;

namespace Key2Joy.LowLevelInput
{
    public static class SimulatedMouse
    {
        private static MOUSEEVENTF TranslateButton(Mouse.Buttons buttons, bool keyUp = false)
        {
            switch (buttons)
            {
                case Mouse.Buttons.Left:
                    return keyUp ? MOUSEEVENTF.LEFTUP : MOUSEEVENTF.LEFTDOWN;
                case Mouse.Buttons.Right:
                    return keyUp ? MOUSEEVENTF.RIGHTUP : MOUSEEVENTF.RIGHTDOWN;
                case Mouse.Buttons.Middle:
                    return keyUp ? MOUSEEVENTF.MIDDLEUP : MOUSEEVENTF.MIDDLEDOWN;
                default:
                    throw new NotImplementedException($"Can't simulate {buttons} yet!");
            }
        }

        public static void PressButton(Mouse.Buttons buttons)
        {
            Send(TranslateButton(buttons));
        }

        public static void ReleaseButton(Mouse.Buttons buttons)
        {
            Send(TranslateButton(buttons, true));
        }

        public static void Send(MOUSEEVENTF mouseFlags, int dx = 0, int dy = 0)
        {
            var Inputs = new Simulator.INPUT[1];
            var Input = new Simulator.INPUT();

            Input.type = 0; // 0 = Mouse Input
            Input.U.mi.dwFlags = mouseFlags;
            Input.U.mi.dwExtraInfo = UIntPtr.Zero;
            Input.U.mi.dx = dx;
            Input.U.mi.dy = dy;
            Inputs[0] = Input;

            Simulator.SendInput(1, Inputs, Simulator.INPUT.Size);
        }

        public static void Move(int x, int y, Mouse.MoveType moveType)
        {
            if (moveType == Mouse.MoveType.Absolute)
            {
                Send(
                    MOUSEEVENTF.MOVE | MOUSEEVENTF.ABSOLUTE,
                    Simulator.CalculateAbsoluteCoordinateX(x),
                    Simulator.CalculateAbsoluteCoordinateY(y));
                return;
            }

            Send(
                MOUSEEVENTF.MOVE,
                x,
                y
            );
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public MouseEventDataXButtons mouseData;
            public MOUSEEVENTF dwFlags;
            public uint time;
            public UIntPtr dwExtraInfo;
        }

        [Flags]
        public enum MouseEventDataXButtons : uint
        {
            Nothing = 0x00000000,
            XBUTTON1 = 0x00000001,
            XBUTTON2 = 0x00000002
        }

        [Flags]
        public enum MOUSEEVENTF : uint
        {
            ABSOLUTE = 0x8000,
            HWHEEL = 0x01000,
            MOVE = 0x0001,
            MOVE_NOCOALESCE = 0x2000,
            LEFTDOWN = 0x0002,
            LEFTUP = 0x0004,
            RIGHTDOWN = 0x0008,
            RIGHTUP = 0x0010,
            MIDDLEDOWN = 0x0020,
            MIDDLEUP = 0x0040,
            VIRTUALDESK = 0x4000,
            WHEEL = 0x0800,
            XDOWN = 0x0080,
            XUP = 0x0100
        }
    }
}
