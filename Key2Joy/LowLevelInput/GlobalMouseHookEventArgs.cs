using System.ComponentModel;

namespace Key2Joy.LowLevelInput
{
    // Source: https://stackoverflow.com/a/34384189
    internal class GlobalMouseHookEventArgs : HandledEventArgs
    {
        public MouseState MouseState { get; private set; }
        public LowLevelMouseInputEvent MouseData { get; private set; }

        public GlobalMouseHookEventArgs(
            LowLevelMouseInputEvent mouseData,
            MouseState mouseState)
        {
            MouseData = mouseData;
            MouseState = mouseState;
        }

        public bool AreButtonsDown()
        {
            switch (MouseState)
            {
                case MouseState.LeftButtonDown:
                case MouseState.RightButtonDown:
                case MouseState.MiddleButtonDown:
                    return true;
            }

            return false;
        }
    }
}
