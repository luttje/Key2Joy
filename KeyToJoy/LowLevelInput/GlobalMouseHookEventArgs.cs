using System.ComponentModel;

namespace KeyToJoy
{
    class GlobalMouseHookEventArgs : HandledEventArgs
    {
        public GlobalInputHook.MouseState MouseState { get; private set; }
        public GlobalInputHook.LowLevelMouseInputEvent MouseData { get; private set; }

        public GlobalMouseHookEventArgs(
            GlobalInputHook.LowLevelMouseInputEvent mouseData,
            GlobalInputHook.MouseState mouseState)
        {
            MouseData = mouseData;
            MouseState = mouseState;
        }
    }
}
