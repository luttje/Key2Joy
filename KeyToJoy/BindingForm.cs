using Linearstar.Windows.RawInput;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyToJoy
{
    public partial class BindingForm : Form
    {
        private List<RadioButton> radioButtonGroup = new List<RadioButton>();
        private GlobalInputHook globalKeyboardHook;

        public BindingForm()
        {
            InitializeComponent();

            radioButtonGroup.Add(radKeyBind);
            radioButtonGroup.Add(radMouseBind);

            foreach (var radioButton in radioButtonGroup)
            {
                radioButton.CheckedChanged += RadioButton_CheckedChanged;
            }

            cmbMouseDirection.DataSource = Enum.GetNames(typeof(AxisDirection));

            RawInputDevice.RegisterDevice(HidUsageAndPage.Keyboard, RawInputDeviceFlags.InputSink, Handle);
            RawInputDevice.RegisterDevice(HidUsageAndPage.Mouse, RawInputDeviceFlags.InputSink, Handle);
        }

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            if (radioButton.Checked)
            {
                foreach (RadioButton other in radioButtonGroup)
                {
                    if (other == radioButton)
                        continue;

                    other.Checked = false;
                }
            }
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_INPUT = 0x00FF;

            if (m.Msg == WM_INPUT)
            {
                var data = RawInputData.FromHandle(m.LParam);

                if (!radKeyBind.Checked)
                    return;

                //if (data is RawInputMouseData mouse)
                //{
                //    if (mouse.Mouse.Buttons == Linearstar.Windows.RawInput.Native.RawMouseButtonFlags.None)
                //        return;

                //    txtKeyBind.Text = $"(mouse) {mouse.Mouse.Buttons} (TODO)";
                //    throw new NotImplementedException("TODO!");
                //}
                //else 
                if (data is RawInputKeyboardData keyboard)
                {
                    var keys = VirtualKeyConverter.KeysFromVirtual(keyboard.Keyboard.VirutalKey);
                    txtKeyBind.Text = $"(keyboard) {keys}";
                }
            }

            base.WndProc(ref m);
        }
    }
}
