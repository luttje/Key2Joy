namespace KeyToJoy.Input.LowLevel
{
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
    }
}