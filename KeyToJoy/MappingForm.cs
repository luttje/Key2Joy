﻿using KeyToJoy.Input;
using Linearstar.Windows.RawInput;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Linearstar.Windows.RawInput.Native;
using KeyToJoy.Mapping;

namespace KeyToJoy
{
    public partial class MappingForm : Form
    {
        internal MappedOption BindingSetting { get; set; }

        private readonly List<RadioButton> radioButtonGroup = new List<RadioButton>();

        internal MappingForm(MappedOption mappedOption)
            :this()
        {
            BindingSetting = mappedOption;

            var image = mappedOption.Action.GetImage();
            if(image != null)
                pctController.Image = image;

            lblInfo.Text = $"Pretend the {mappedOption.GetActionDisplay()} button is pressed when...";

            SetConfirmMappingButtonText();
        }

        private MappingForm()
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

            BindingSetting.Trigger = new MouseMoveTrigger((AxisDirection)Enum.Parse(typeof(AxisDirection), cmbMouseDirection.Text));
            SetConfirmMappingButtonText($"Mouse {cmbMouseDirection.Text}");
        }

        private void SetConfirmMappingButtonText(string trigger = null)
        {
            btnConfirm.Text = $"Confirm mapping {BindingSetting.GetActionDisplay()} to {trigger ?? "..."}";

            if (trigger != null)
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
                    BindingSetting.Trigger = new KeyboardTrigger(keys, keyboard.Keyboard.Flags);


                    txtKeyBind.Text = $"(keyboard) {BindingSetting.Trigger}";

                    SetConfirmMappingButtonText(BindingSetting.Trigger.ToString());
                }
                else if (data is RawInputMouseData mouse 
                    && mouse.Mouse.Buttons != RawMouseButtonFlags.None
                    && txtKeyBind.ClientRectangle.Contains(txtKeyBind.PointToClient(MousePosition)))
                {
                    try
                    {
                        BindingSetting.Trigger = new MouseButtonTrigger(mouse.Mouse.Buttons);

                        txtKeyBind.Text = $"{BindingSetting.Trigger}";

                        SetConfirmMappingButtonText(BindingSetting.Trigger.ToString());
                    }
                    catch (ArgumentOutOfRangeException ex)
                    {
                        MessageBox.Show($"Unknown mouse button pressed ({ex.Message}). Can't map this (yet).", "Unknown mouse button!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            base.WndProc(ref m);
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BindingForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Relieve input capturing by this mapping form and return it to the main form
            RawInputDevice.UnregisterDevice(HidUsageAndPage.Keyboard);
            RawInputDevice.UnregisterDevice(HidUsageAndPage.Mouse);
        }
    }
}
