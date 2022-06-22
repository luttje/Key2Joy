using KeyToJoy.Input;
using Linearstar.Windows.RawInput;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Linearstar.Windows.RawInput.Native;
using KeyToJoy.Mapping;

namespace KeyToJoy
{
    public partial class KeyboardTriggerOptionsControl : UserControl, ISetupTrigger
    {
        public KeyboardTriggerOptionsControl()
        {
            InitializeComponent();

            RawInputDevice.RegisterDevice(HidUsageAndPage.Keyboard, RawInputDeviceFlags.InputSink, Handle);
        }

        public void Setup(BaseTrigger trigger)
        {
            var thisTrigger = (KeyboardTrigger) trigger;

            // TODO: Fill the trigger with the key selected in this options panel
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_INPUT = 0x00FF;

            // TODO: Refactor
            //if (m.Msg == WM_INPUT)
            //{
            //    var data = RawInputData.FromHandle(m.LParam);

            //    if (data is RawInputKeyboardData keyboard)
            //    {
            //        var keys = VirtualKeyConverter.KeysFromVirtual(keyboard.Keyboard.VirutalKey);
            //        var trigger = new KeyboardTrigger(keys, keyboard.Keyboard.Flags);

            //        txtKeyBind.Text = $"(keyboard) {trigger}";
            //    }
            //}

            base.WndProc(ref m);
        }

        private void KeyboardTriggerOptionsControl_ControlRemoved(object sender, ControlEventArgs e)
        {
            // Relieve input capturing by this mapping form and return it to the main form
            RawInputDevice.UnregisterDevice(HidUsageAndPage.Keyboard);
        }
    }
}
