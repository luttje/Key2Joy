using SimWinInput;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Linearstar.Windows.RawInput;
using RawKeyboardFlags = Linearstar.Windows.RawInput.Native.RawKeyboardFlags;
using System.Drawing;
using KeyToJoy.Input;
using KeyToJoy.Properties;

namespace KeyToJoy
{
    public partial class MainForm : Form
    {
        const double SENSITIVITY = 0.05;

        private BindingPreset selectedPreset;

        private Image defaultControllerImage;

        public MainForm()
        {
            InitializeComponent();

            defaultControllerImage = pctController.Image;

            SetupInputHooks();
            SimGamePad.Instance.PlugIn();

            cmbPreset.DisplayMember = "Display";
            cmbPreset.DataSource = BindingPreset.All;
            
            ReloadSelectedPreset();
        }

        private void ReloadSelectedPreset()
        {
            selectedPreset = cmbPreset.SelectedItem as BindingPreset;
            dgvBinds.DataSource = selectedPreset.Bindings;
            txtPresetName.Text = selectedPreset.Name;
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            dgvBinds.ClearSelection();
            pctController.Image = defaultControllerImage;
        }

        private void CmbPreset_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReloadSelectedPreset();
        }

        private void ChangeBinding(BindingOption bindingOption)
        {
            new BindingForm(bindingOption).ShowDialog();

            foreach (var option in selectedPreset.Bindings)
            {
                if (option == bindingOption)
                    continue;

                if (option.Binding == bindingOption.Binding)
                {
                    MessageBox.Show($"This binding is already in use for {option.Control}! Change {option.Control} to something else.");
                    ChangeBinding(option);
                    break;
                }
            }

            dgvBinds.Update();
            selectedPreset.Save();
        }

        private void DgvBinds_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var row = dgvBinds.Rows[e.RowIndex];
            var bindingOption = row.DataBoundItem as BindingOption;

            if (bindingOption == null)
                return;

            ChangeBinding(bindingOption);
        }

        private void DgvBinds_SelectionChanged(object sender, EventArgs e)
        {
            var rowsCount = dgvBinds.SelectedRows.Count;

            if (rowsCount == 0 || rowsCount > 1) 
                return;

            var row = dgvBinds.SelectedRows[0];

            if (!row.Selected)
            {
                pctController.Image = defaultControllerImage;
                return;
            }

            var bindingOption = row.DataBoundItem as BindingOption;

            if (bindingOption == null)
                return;

            pctController.Image = BindingOption.GetControllerImage(bindingOption.Control);
        }

        private void DgvBinds_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var bindingOption = dgvBinds.Rows[e.RowIndex].DataBoundItem as BindingOption;

            dgvBinds.Rows[e.RowIndex].Cells["colControl"].Value = bindingOption.GetControlDisplay();
            dgvBinds.Rows[e.RowIndex].Cells["colBind"].Value = bindingOption.GetBindDisplay();
        }

        public void SetupInputHooks()
        {
            RawInputDevice.RegisterDevice(HidUsageAndPage.Mouse, RawInputDeviceFlags.InputSink, Handle);
            RawInputDevice.RegisterDevice(HidUsageAndPage.Keyboard, RawInputDeviceFlags.InputSink, Handle);
        }

        private bool TryOverrideKeyboardInput(Keys keys, bool isPressed)
        {
            if (!chkEnabled.Checked)
                return false;

            if (!selectedPreset.TryGetBinding(new KeyboardBinding(keys), out var bindingOption))
                return false;

            if (isPressed)
                SimGamePad.Instance.SetControl(bindingOption.Control);
            else
                SimGamePad.Instance.ReleaseControl(bindingOption.Control);

            return true;
        }

        private bool TryOverrideMouseInput(int lastX, int lastY)
        {
            if (!chkEnabled.Checked)
                return false;

            timerAxisTimeout.Stop();
            timerAxisTimeout.Start();

            var controllerId = 0;
            var state = SimGamePad.Instance.State[controllerId];

            var deltaX = (short)Math.Min(Math.Max(lastX * short.MaxValue * SENSITIVITY, short.MinValue), short.MaxValue);
            var deltaY = (short)-Math.Min(Math.Max(lastY * short.MaxValue * SENSITIVITY, short.MinValue), short.MaxValue);
            BindingOption bindingOption;

            if (
                (
                    deltaX > 0
                    && selectedPreset.TryGetBinding(new MouseAxisBinding(AxisDirection.Right), out bindingOption)
                )
                ||
                (
                    deltaX < 0
                    && selectedPreset.TryGetBinding(new MouseAxisBinding(AxisDirection.Left), out bindingOption)
                )
            )
            {
                if (bindingOption.Control == GamePadControl.RightStickRight
                    || bindingOption.Control == GamePadControl.RightStickLeft) // TODO: The rest (LeftStick, DPad, etc)
                    state.RightStickX = (short)((deltaX + state.RightStickX) / 2);
            }
            if (
                (
                    deltaY > 0
                    && selectedPreset.TryGetBinding(new MouseAxisBinding(AxisDirection.Up), out bindingOption)
                )
                ||
                (
                    deltaY < 0
                    && selectedPreset.TryGetBinding(new MouseAxisBinding(AxisDirection.Down), out bindingOption)
                )
            )
            {
                if (bindingOption.Control == GamePadControl.RightStickUp
                    || bindingOption.Control == GamePadControl.RightStickDown) // TODO: The rest (LeftStick, DPad, etc)
                    state.RightStickY = (short)((deltaY + state.RightStickY) / 2);
            }

            SimGamePad.Instance.Update(controllerId);
            return true;
        }

        const int WM_INPUT = 0x00FF;
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg != WM_INPUT) 
            { 
                return;
            }

            var data = RawInputData.FromHandle(m.LParam);

            if (data is RawInputKeyboardData keyboard)
            {
                var keys = VirtualKeyConverter.KeysFromVirtual(keyboard.Keyboard.VirutalKey);

                if (TryOverrideKeyboardInput(keys, (keyboard.Keyboard.Flags & RawKeyboardFlags.Up) != RawKeyboardFlags.Up))
                {
                    return; // TODO: OVerride default input
                }
                else
                {
                    return;
                }
            }

            if (data is RawInputMouseData mouse)
            {
                if (TryOverrideMouseInput(mouse.Mouse.LastX, mouse.Mouse.LastY))
                {
                    return; // TODO: OVerride default input
                }
                else
                {
                    return;
                }
            }
        }

        private void TimerAxisTimeout_Tick(object sender, EventArgs e)
        {
            var controllerId = 0;
            var state = SimGamePad.Instance.State[controllerId];
            state.RightStickX = 0;
            state.RightStickY = 0;
            SimGamePad.Instance.Update(controllerId);
        }

        private void BtnOpenTest_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://devicetests.com/controller-tester");
        }

        private void ChkEnabled_CheckedChanged(object sender, EventArgs e)
        {
            btnOpenTest.Enabled = chkEnabled.Checked;
        }

        private void TxtPresetName_TextChanged(object sender, EventArgs e)
        {
            selectedPreset.Name = txtPresetName.Text;
            selectedPreset.Save();
            BindingPreset.All.ResetBindings();
        }

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            var preset = new BindingPreset(txtPresetName.Text, selectedPreset.Bindings);

            BindingPreset.Add(preset);
            cmbPreset.SelectedIndex = cmbPreset.Items.Count - 1;
        }

        private void BtnAbout_Click(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog();
        }
    }
}
