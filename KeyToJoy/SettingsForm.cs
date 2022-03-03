using SimWinInput;
using SimGamePad = KeyToJoy.DavidRieman.SimGamePad;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Linearstar.Windows.RawInput;
using System.Drawing;
using KeyToJoy.Properties;

namespace KeyToJoy
{
    public partial class SettingsForm : Form
    {
        const double SENSITIVITY = 0.05;

        private Dictionary<Keys, BindingSetting> bindsLookup;
        private Dictionary<AxisDirection, BindingSetting> mouseAxisBindsLookup;

        private GlobalInputHook _globalKeyboardHook;
        private string debug = string.Empty;
        private Image defaultControllerImage;

        private BindingList<BindingPreset> presets = new BindingList<BindingPreset>();

        public SettingsForm()
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

            new BindingForm().ShowDialog();
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

            dgvBinds.Rows[e.RowIndex].Cells["colControl"].Value = bindingSetting.Control;

            if (bindingSetting.DefaultKeyBind != null)
                dgvBinds.Rows[e.RowIndex].Cells["colBind"].Value = bindingSetting.DefaultKeyBind;
            else if (bindingSetting.DefaultAxisBind != null)
                dgvBinds.Rows[e.RowIndex].Cells["colBind"].Value = "Mouse " + Enum.GetName(typeof(AxisDirection), bindingSetting.DefaultAxisBind);
        }

        public void SetupInputHooks()
        {
            RawInputDevice.RegisterDevice(HidUsageAndPage.Mouse, RawInputDeviceFlags.InputSink, Handle);

            _globalKeyboardHook = new GlobalInputHook();
            _globalKeyboardHook.KeyboardInputEvent += OnKeyInputEvent;
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_INPUT = 0x00FF;

            if (m.Msg == WM_INPUT)
            {
                var data = RawInputData.FromHandle(m.LParam);

                // The data will be an instance of either RawInputMouseData, RawInputKeyboardData, or RawInputHidData.
                // They contain the raw input data in their properties.
                if (!(data is RawInputMouseData mouse))
                    return;

                debug = mouse.Mouse.ToString();

                if (!chkEnabled.Checked)
                    return;

                if (mouseAxisBindsLookup.Count == 0)
                    return;

                timerAxisTimeout.Stop();
                timerAxisTimeout.Start();

                var controllerId = 0;
                var state = SimGamePad.Instance.State[controllerId];

                var screen = Screen.FromPoint(Cursor.Position);
                var deltaX = (short)Math.Min(Math.Max(mouse.Mouse.LastX * short.MaxValue * SENSITIVITY, short.MinValue), short.MaxValue);
                var deltaY = (short)-Math.Min(Math.Max(mouse.Mouse.LastY * short.MaxValue * SENSITIVITY, short.MinValue), short.MaxValue);
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

                dbgLabel.Text = $"{deltaX}, {deltaY} " +
                    $"\n(raw: {mouse.Mouse.LastX}, {mouse.Mouse.LastY}) " +
                    $"\n(screen: {screen.WorkingArea.Width}, {screen.WorkingArea.Height}) " +
                    $"\n{debug}";
            }

            base.WndProc(ref m);
        }

        private void OnKeyInputEvent(object sender, GlobalKeyboardHookEventArgs e)
        {
            if (!chkEnabled.Checked)
                return;

            //https://docs.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes
            var keys = VirtualKeyConverter.KeysFromVirtual(e.KeyboardData.VirtualCode);

            if (!bindsLookup.TryGetValue(keys, out var bindingSetting))
                return;

            if (e.KeyboardState == GlobalInputHook.KeyboardState.KeyDown)
                SimGamePad.Instance.SetControl(bindingSetting.Control);
            else if (e.KeyboardState == GlobalInputHook.KeyboardState.KeyUp)
                SimGamePad.Instance.ReleaseControl(bindingSetting.Control);

            e.Handled = true;
        }

        public void Dispose()
        {
            _globalKeyboardHook?.Dispose();
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
