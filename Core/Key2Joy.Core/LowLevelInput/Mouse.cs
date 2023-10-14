using System;

namespace Key2Joy.LowLevelInput;

public class Mouse
{
    /// <summary>
    /// Represents the type of mouse movement.
    /// </summary>
    public enum MoveType
    {
        /// <summary>
        /// Specifies relative movement to where the cursor is now.
        /// </summary>
        Relative = 0,

        /// <summary>
        /// Specifies absolute movement of the cursor on screen.
        /// </summary>
        Absolute = 1
    }

    /// <summary>
    /// Represents the buttons on a mouse.
    /// </summary>
    public enum Buttons
    {
        /// <summary>
        /// No button is pressed.
        /// </summary>
        None = 0,

        /// <summary>
        /// The left mouse button.
        /// </summary>
        Left = 1,

        /// <summary>
        /// The right mouse button.
        /// </summary>
        Right = 2,

        /// <summary>
        /// The middle mouse button.
        /// </summary>
        Middle = 4,

        /// <summary>
        /// The first extra mouse button.
        /// </summary>
        XButton1 = 5,

        /// <summary>
        /// The second extra mouse button.
        /// </summary>
        XButton2 = 6,

        /// <summary>
        /// The mouse wheel is moved upward.
        /// </summary>
        WheelUp = 8,

        /// <summary>
        /// The mouse wheel is moved downward.
        /// </summary>
        WheelDown = 16
    }

    private static Buttons GetXButton(int data)
    {
        var xButton = (data >> 16) & 0xFFFF;

        if (xButton == 1)
        {
            return Buttons.XButton1;
        }
        else if (xButton == 2)
        {
            return Buttons.XButton2;
        }

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
                {
                    return xButton;
                }

                break;

            case MouseState.Move:
                break;

            case MouseState.MiddleButtonDoubleClick:
                break;

            case MouseState.Wheel:
                break;

            case MouseState.WheelHorizontal:
                break;

            case MouseState.NonClientXButtonDoubleClick:
                break;

            case MouseState.NonClientXButtonDown:
                break;

            case MouseState.NonClientXButtonUp:
                break;

            case MouseState.XButtonDoubleClick:
                break;

            default:
                break;
        }

        Console.WriteLine($"Mouse button ({e.RawData}) not yet supported!");

        return Buttons.None;
    }
}
