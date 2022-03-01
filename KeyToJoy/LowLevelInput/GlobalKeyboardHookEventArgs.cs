using System.ComponentModel;

namespace KeyToJoy
{
    class GlobalKeyboardHookEventArgs : HandledEventArgs
    {
        public GlobalInputHook.KeyboardState KeyboardState { get; private set; }
        public GlobalInputHook.LowLevelKeyboardInputEvent KeyboardData { get; private set; }

        public GlobalKeyboardHookEventArgs(
            GlobalInputHook.LowLevelKeyboardInputEvent keyboardData,
            GlobalInputHook.KeyboardState keyboardState)
        {
            KeyboardData = keyboardData;
            KeyboardState = keyboardState;
        }
    }
}
