namespace Key2Joy.LowLevelInput;

// Source: https://stackoverflow.com/a/34384189
// Extra Info:
// - http://msdn.microsoft.com/en-us/library/ms644986(VS.85).aspx
// - https://www.autoitscript.com/forum/topic/81761-callback-low-level-mouse-hook/
public enum MouseState
{
    Move = 0x0200,

    LeftButtonDown = 0x0201,
    LeftButtonUp = 0x0202,
    RightButtonDown = 0x0204,
    RightButtonUp = 0x0205,

    MiddleButtonDown = 0x0207,
    MiddleButtonUp = 0x0208,
    MiddleButtonDoubleClick = 0x0209,

    Wheel = 0x020A, // Wheel Up/Down
    WheelHorizontal = 0x020E, // Wheel Left/Right

    /// <summary>
    /// Posted when the user double-clicks the first or second X button while the cursor is in 
    /// the nonclient area of a window. This message is posted to the window that contains the 
    /// cursor. If a window has captured the mouse, this message is not posted.
    /// </summary>
    NonClientXButtonDoubleClick = 0x00AD,
    NonClientXButtonDown = 0x00AB,
    NonClientXButtonUp = 0x00AC,

    XButtonDoubleClick = 0x020D,
    XButtonDown = 0x020B,
    XButtonUp = 0x020C,
}