using System.ComponentModel;

namespace Key2Joy.LowLevelInput
{
    // Source: https://stackoverflow.com/a/34384189
    public class GlobalMouseHookEventArgs : HandledEventArgs
    {
        public MouseState MouseState { get; private set; }
        public LowLevelMouseInputEvent RawData { get; private set; }

        public GlobalMouseHookEventArgs(
            LowLevelMouseInputEvent mouseData,
            MouseState mouseState)
        {
            this.RawData = mouseData;
            this.MouseState = mouseState;
        }
    }
}
