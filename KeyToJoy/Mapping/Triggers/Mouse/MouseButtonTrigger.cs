using System;
using System.Windows.Forms;
using KeyToJoy.Input.LowLevel;
using Linearstar.Windows.RawInput.Native;
using Newtonsoft.Json;

namespace KeyToJoy.Mapping
{
    [Trigger(
        Name = "Mouse Button Event",
        OptionsUserControl = typeof(MouseButtonTriggerOptionsControl)
    )]
    internal class MouseButtonTrigger : BaseTrigger, IEquatable<MouseButtonTrigger>
    {
        private const string MOUSE_SERIALIZE_PREFIX = "Mouse_";

        private MouseButtons mouseButtons;

        [JsonConstructor]
        internal MouseButtonTrigger(string name)
        {
            name = name.Substring(MOUSE_SERIALIZE_PREFIX.Length);
            this.mouseButtons = (MouseButtons)Enum.Parse(typeof(MouseButtons), name);
        }

        // TODO: Clean up (currently very prone to human error when adding buttons)
        // Note: Also add the other constructor: MouseBinding(MouseState mouseState) 
        internal MouseButtonTrigger(RawMouseButtonFlags mouseButton)
        {
            // TODO: Support up and down states seperately
            switch (mouseButton)
            {
                case RawMouseButtonFlags.LeftButtonUp:
                case RawMouseButtonFlags.LeftButtonDown:
                    this.mouseButtons = MouseButtons.Left;
                    break;
                case RawMouseButtonFlags.RightButtonUp:
                case RawMouseButtonFlags.RightButtonDown:
                    this.mouseButtons = MouseButtons.Right;
                    break;
                case RawMouseButtonFlags.MiddleButtonUp:
                case RawMouseButtonFlags.MiddleButtonDown:
                    this.mouseButtons = MouseButtons.Middle;
                    break;
                // TODO: Support these (requires knowing low level input numbers for MouseState)
                //case RawMouseButtonFlags.Button4Up:
                //case RawMouseButtonFlags.Button4Down:
                //    this.mouseButtons = MouseButtons.XButton1;
                //    break;
                //case RawMouseButtonFlags.Button5Up:
                //case RawMouseButtonFlags.Button5Down:
                //    this.mouseButtons = MouseButtons.XButton2;
                //    break;
                default: throw new ArgumentOutOfRangeException(mouseButton.ToString());
            }
        }

        // TODO: Clean up (currently very prone to human error when adding buttons)
        // Note: Also add the other constructor: MouseBinding(RawMouseButtonFlags mouseButton) 
        public MouseButtonTrigger(MouseState mouseState)
        {
            // TODO: Support up and down states seperately
            switch (mouseState)
            {
                case MouseState.LeftButtonUp:
                case MouseState.LeftButtonDown:
                    this.mouseButtons = MouseButtons.Left;
                    break;
                case MouseState.RightButtonUp:
                case MouseState.RightButtonDown:
                    this.mouseButtons = MouseButtons.Right;
                    break;
                case MouseState.MiddleButtonUp:
                case MouseState.MiddleButtonDown:
                    this.mouseButtons = MouseButtons.Middle;
                    break;
                default: throw new ArgumentOutOfRangeException(mouseState.ToString());
            }
        }

        internal override string GetUniqueKey()
        {
            return MOUSE_SERIALIZE_PREFIX + mouseButtons.ToString();
        }

        public override bool Equals(object obj)
        {
            if(!(obj is MouseButtonTrigger other)) 
                return false;

            return Equals(other);
        }

        public bool Equals(MouseButtonTrigger other)
        {
            return mouseButtons == other.mouseButtons;
        }

        public override string ToString()
        {
            return "(mouse) " + mouseButtons.ToString();
        }

        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
}
