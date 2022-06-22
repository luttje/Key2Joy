using KeyToJoy.Input;
using Linearstar.Windows.RawInput;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Linearstar.Windows.RawInput.Native;
using KeyToJoy.Mapping;
using EnumMouseButtons = KeyToJoy.Mapping.MouseButtons;

namespace KeyToJoy
{
    public partial class MouseButtonTriggerOptionsControl : UserControl, ISetupTrigger
    {
        private EnumMouseButtons mouseButtons;

        public MouseButtonTriggerOptionsControl()
        {
            InitializeComponent();

            RawInputDevice.RegisterDevice(HidUsageAndPage.Mouse, RawInputDeviceFlags.InputSink, Handle);
            
            // Relieve input capturing by this mapping form and return it to the main form
            this.Disposed += (s, e) => RawInputDevice.UnregisterDevice(HidUsageAndPage.Mouse);
        }

        public void Setup(BaseTrigger trigger)
        {
            var thisTrigger = (MouseButtonTrigger)trigger;

            thisTrigger.MouseButtons = mouseButtons;
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_INPUT = 0x00FF;

            if (m.Msg == WM_INPUT)
            {
                var data = RawInputData.FromHandle(m.LParam);

                if (data is RawInputMouseData mouse
                    && mouse.Mouse.Buttons != RawMouseButtonFlags.None
                    && txtKeyBind.ClientRectangle.Contains(txtKeyBind.PointToClient(MousePosition)))
                {
                    var flags = mouse.Mouse.Buttons;

                    // TODO: Support up and down states seperately
                    switch (flags)
                    {
                        case RawMouseButtonFlags.LeftButtonUp:
                        case RawMouseButtonFlags.LeftButtonDown:
                            mouseButtons = EnumMouseButtons.Left;
                            break;
                        case RawMouseButtonFlags.RightButtonUp:
                        case RawMouseButtonFlags.RightButtonDown:
                            mouseButtons = EnumMouseButtons.Right;
                            break;
                        case RawMouseButtonFlags.MiddleButtonUp:
                        case RawMouseButtonFlags.MiddleButtonDown:
                            mouseButtons = EnumMouseButtons.Middle;
                            break;
                        case RawMouseButtonFlags.MouseWheel:
                            if (mouse.Mouse.ButtonData >= 0)
                                mouseButtons = EnumMouseButtons.WheelUp;
                            else
                                mouseButtons = EnumMouseButtons.WheelDown;
                            break;
                        // TODO: Support these
                        //case RawMouseButtonFlags.Button4Up:
                        //case RawMouseButtonFlags.Button4Down:
                        //    this.mouseButtons = MouseButtons.XButton1;
                        //    break;
                        //case RawMouseButtonFlags.Button5Up:
                        //case RawMouseButtonFlags.Button5Down:
                        //    this.mouseButtons = MouseButtons.XButton2;
                        //    break;
                        default:
                            MessageBox.Show($"Unknown mouse button pressed ({flags}). Can't map this (yet).", "Unknown mouse button!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                    }

                    txtKeyBind.Text = $"{mouseButtons}";
                }
            }

            base.WndProc(ref m);
        }
    }
}
