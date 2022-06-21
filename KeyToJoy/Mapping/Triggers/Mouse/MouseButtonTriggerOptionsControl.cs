using KeyToJoy.Input;
using Linearstar.Windows.RawInput;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Linearstar.Windows.RawInput.Native;
using KeyToJoy.Mapping;

namespace KeyToJoy
{
    public partial class MouseButtonTriggerOptionsControl : UserControl
    {
        public MouseButtonTriggerOptionsControl()
        {
            InitializeComponent();

            RawInputDevice.RegisterDevice(HidUsageAndPage.Mouse, RawInputDeviceFlags.InputSink, Handle);
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
                    try
                    {
                        var trigger = new MouseButtonTrigger(mouse.Mouse.Buttons);

                        txtKeyBind.Text = $"{trigger}";
                    }
                    catch (ArgumentOutOfRangeException ex)
                    {
                        MessageBox.Show($"Unknown mouse button pressed ({ex.Message}). Can't map this (yet).", "Unknown mouse button!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            base.WndProc(ref m);
        }

        private void MouseButtonTriggerOptionsControl_ControlRemoved(object sender, ControlEventArgs e)
        {
            // Relieve input capturing by this mapping form and return it to the main form
            RawInputDevice.UnregisterDevice(HidUsageAndPage.Mouse);
        }
    }
}
