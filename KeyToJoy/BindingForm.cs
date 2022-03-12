using KeyToJoy.Input;
using Linearstar.Windows.RawInput;
using System;
using System.Collections.Generic;
using RawKeyboardFlags = Linearstar.Windows.RawInput.Native.RawKeyboardFlags;
using System.Windows.Forms;

namespace KeyToJoy
{
    public partial class BindingForm : Form
    {
        internal BindingSetting BindingSetting { get; set; }

        private List<RadioButton> radioButtonGroup = new List<RadioButton>();

        internal BindingForm(BindingSetting bindingSetting)
            :this()
        {
            this.BindingSetting = bindingSetting;

            pctController.Image = bindingSetting.HighlightImage;

            lblInfo.Text = $"Pretend the {bindingSetting.GetControlDisplay()} button is pressed when...";

            SetConfirmBindButtonText();
        }

        private BindingForm()
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

            if (!radMouseBind.Checked)
            {
                cmbMouseDirection.Enabled = false;
                return;
            }

            UpdateMouseAxisBind();
            cmbMouseDirection.Enabled = true;
        }

        private void cmbMouseDirection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (radMouseBind.Checked)
                UpdateMouseAxisBind();
        }

        private void UpdateMouseAxisBind()
        {
            if (BindingSetting == null)
                return;

            BindingSetting.DefaultAxisBind = (AxisDirection?)Enum.Parse(typeof(AxisDirection), cmbMouseDirection.Text);
            SetConfirmBindButtonText($"Mouse {cmbMouseDirection.Text}");
        }

        private void SetConfirmBindButtonText(string bind = null)
        {
            btnConfirm.Text = $"Confirm binding {BindingSetting.GetControlDisplay()} to {bind ?? "..."}";

            if (bind != null)
                btnConfirm.Enabled = true;
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_INPUT = 0x00FF;

            if (m.Msg == WM_INPUT)
            {
                var data = RawInputData.FromHandle(m.LParam);

                if (!radKeyBind.Checked)
                    return;

                if (data is RawInputKeyboardData keyboard)
                {
                    var keys = VirtualKeyConverter.KeysFromVirtual(keyboard.Keyboard.VirutalKey);
                    BindingSetting.DefaultKeyBind = keys;


                    if ((keyboard.Keyboard.Flags & RawKeyboardFlags.KeyE0) == RawKeyboardFlags.KeyE0)
                    {
                        if (BindingSetting.DefaultKeyBind == Keys.ControlKey)
                            BindingSetting.DefaultKeyBind = Keys.RControlKey;
                        if (BindingSetting.DefaultKeyBind == Keys.ShiftKey)
                            BindingSetting.DefaultKeyBind = Keys.RShiftKey;
                    }
                    else
                    {
                        if (BindingSetting.DefaultKeyBind == Keys.ControlKey)
                            BindingSetting.DefaultKeyBind = Keys.LControlKey;
                        if (BindingSetting.DefaultKeyBind == Keys.ShiftKey)
                            BindingSetting.DefaultKeyBind = Keys.LShiftKey;
                    }

                    txtKeyBind.Text = $"(keyboard) {BindingSetting.DefaultKeyBind}";

                    SetConfirmBindButtonText(BindingSetting.DefaultKeyBind.ToString());
                }
            }

            base.WndProc(ref m);
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
