using SimWinInput;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Linearstar.Windows.RawInput;
using RawKeyboardFlags = Linearstar.Windows.RawInput.Native.RawKeyboardFlags;
using System.Drawing;
using KeyToJoy.Input;

namespace KeyToJoy
{
    public partial class MainForm : Form
    {
        const double SENSITIVITY = 0.05;

        private Dictionary<Keys, BindingSetting> bindsLookup;
        private Dictionary<AxisDirection, BindingSetting> mouseAxisBindsLookup;
        private BindingList<BindingPreset> presets = new BindingList<BindingPreset>();

        private Image defaultControllerImage;

        public MainForm()
        {
            InitializeComponent();
            defaultControllerImage = pctController.Image;

            SetupInputHooks();
            SimGamePad.Instance.PlugIn();

            presets.Add(BindingPreset.Default);

            cmbPreset.DisplayMember = "Name";
            cmbPreset.DataSource = presets;

            LoadPreset(presets[0]);
        }

        private void LoadPreset(BindingPreset preset)
        {
            dgvBinds.DataSource = preset.Bindings;

            bindsLookup = new Dictionary<Keys, BindingSetting>();
            mouseAxisBindsLookup = new Dictionary<AxisDirection, BindingSetting>();

            foreach (var bindingSetting in preset.Bindings)
            {
                if (bindingSetting.DefaultKeyBind != null)
                    bindsLookup.Add((Keys)bindingSetting.DefaultKeyBind, bindingSetting);
                else if (bindingSetting.DefaultAxisBind != null)
                    mouseAxisBindsLookup.Add((AxisDirection)bindingSetting.DefaultAxisBind, bindingSetting);
            }
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            dgvBinds.ClearSelection();
            pctController.Image = defaultControllerImage;
        }

        private void dgvBinds_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var row = dgvBinds.Rows[e.RowIndex];
            var bindingSetting = row.DataBoundItem as BindingSetting;

            if (bindingSetting == null)
                return;

            new BindingForm(bindingSetting).ShowDialog();

            dgvBinds.Update();
        }

        private void dgvBinds_SelectionChanged(object sender, EventArgs e)
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

            var bindingSetting = row.DataBoundItem as BindingSetting;

            if (bindingSetting == null)
                return;

            pctController.Image = bindingSetting.HighlightImage;
        }

        private void DgvBinds_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var bindingSetting = dgvBinds.Rows[e.RowIndex].DataBoundItem as BindingSetting;

            dgvBinds.Rows[e.RowIndex].Cells["colControl"].Value = bindingSetting.GetControlDisplay();
            dgvBinds.Rows[e.RowIndex].Cells["colBind"].Value = bindingSetting.GetBindDisplay();
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

            if (!bindsLookup.TryGetValue(keys, out var bindingSetting))
                return false;

            if (isPressed)
                SimGamePad.Instance.SetControl(bindingSetting.Control);
            else
                SimGamePad.Instance.ReleaseControl(bindingSetting.Control);

            return true;
        }

        private bool TryOverrideMouseInput(int lastX, int lastY)
        {
            if (!chkEnabled.Checked)
                return false;

            if (mouseAxisBindsLookup.Count == 0)
                return false;

            timerAxisTimeout.Stop();
            timerAxisTimeout.Start();

            var controllerId = 0;
            var state = SimGamePad.Instance.State[controllerId];

            var screen = Screen.FromPoint(Cursor.Position);
            var deltaX = (short)Math.Min(Math.Max(lastX * short.MaxValue * SENSITIVITY, short.MinValue), short.MaxValue);
            var deltaY = (short)-Math.Min(Math.Max(lastY * short.MaxValue * SENSITIVITY, short.MinValue), short.MaxValue);
            BindingSetting bindingSetting;

            if (
                (
                    deltaX > 0
                    && mouseAxisBindsLookup.TryGetValue(AxisDirection.Right, out bindingSetting)
                )
                ||
                (
                    deltaX < 0
                    && mouseAxisBindsLookup.TryGetValue(AxisDirection.Left, out bindingSetting)
                )
            )
            {
                if (bindingSetting.Control == GamePadControl.RightStickRight
                    || bindingSetting.Control == GamePadControl.RightStickLeft) // TODO: The rest (LeftStick, DPad, etc)
                    state.RightStickX = (short)((deltaX + state.RightStickX) / 2);
            }
            if (
                (
                    deltaY > 0
                    && mouseAxisBindsLookup.TryGetValue(AxisDirection.Up, out bindingSetting)
                )
                ||
                (
                    deltaY < 0
                    && mouseAxisBindsLookup.TryGetValue(AxisDirection.Down, out bindingSetting)
                )
            )
            {
                if (bindingSetting.Control == GamePadControl.RightStickUp
                    || bindingSetting.Control == GamePadControl.RightStickDown) // TODO: The rest (LeftStick, DPad, etc)
                    state.RightStickY = (short)((deltaY + state.RightStickY) / 2);
            }

            SimGamePad.Instance.Update(controllerId);
            return true;
        }

        const int WM_INPUT = 0x00FF;
        protected override void WndProc(ref Message m)
        {
            if (m.Msg != WM_INPUT) 
            { 
                base.WndProc(ref m);
                return;
            }

            var data = RawInputData.FromHandle(m.LParam);

            if (data is RawInputKeyboardData keyboard)
            {
                var keys = VirtualKeyConverter.KeysFromVirtual(keyboard.Keyboard.VirutalKey);

                if (TryOverrideKeyboardInput(keys, (keyboard.Keyboard.Flags & RawKeyboardFlags.Up) != RawKeyboardFlags.Up))
                {
                    return;
                }
                else
                {
                    base.WndProc(ref m);
                    return;
                }
            }

            if (data is RawInputMouseData mouse)
            {
                if (TryOverrideMouseInput(mouse.Mouse.LastX, mouse.Mouse.LastY))
                {
                    return;
                }
                else
                {
                    base.WndProc(ref m);
                    return;
                }
            }
        }

        private void timerAxisTimeout_Tick(object sender, EventArgs e)
        {
            var controllerId = 0;
            var state = SimGamePad.Instance.State[controllerId];
            state.RightStickX = 0;
            state.RightStickY = 0;
            SimGamePad.Instance.Update(controllerId);
        }

        private void btnOpenTest_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://devicetests.com/controller-tester");
        }

        private void chkEnabled_CheckedChanged(object sender, EventArgs e)
        {
            btnOpenTest.Enabled = chkEnabled.Checked;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            var preset = new BindingPreset(cmbPreset.Text);

            presets.Add(preset);
            cmbPreset.SelectedIndex = cmbPreset.Items.Count - 1;
        }
    }
}
