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

namespace KeyToJoy
{
    public partial class SettingsForm : Form
    {
        const double SENSITIVITY = 0.05;

        private List<GamePadInputSetting> allSettings;
        private Dictionary<Keys, GamePadInputSetting> binds = new Dictionary<Keys, GamePadInputSetting>();
        private Dictionary<AxisDirection, GamePadInputSetting> mouseAxisBinds = new Dictionary<AxisDirection, GamePadInputSetting>();

        private GlobalInputHook _globalKeyboardHook;
        private string debug = string.Empty;

        public SettingsForm()
        {
            InitializeComponent();

            RawInputDevice.RegisterDevice(HidUsageAndPage.Mouse, RawInputDeviceFlags.InputSink, Handle);

            SetupKeyboardHooks();
            SimGamePad.Instance.PlugIn();

            allSettings = new List<GamePadInputSetting>();

            // Top of controller
            allSettings.Add(new GamePadInputSetting
            {
                Control = GamePadControl.LeftShoulder,
                TargetSettingInput = txtShoulderLeft,
                DefaultKeyBind = Keys.Q
            });
            allSettings.Add(new GamePadInputSetting
            {
                Control = GamePadControl.LeftTrigger,
                TargetSettingInput = txtTriggerLeft,
                DefaultKeyBind = Keys.D1
            });
            allSettings.Add(new GamePadInputSetting
            {
                Control = GamePadControl.RightShoulder,
                TargetSettingInput = txtShoulderRight,
                DefaultKeyBind = Keys.E
            });
            allSettings.Add(new GamePadInputSetting
            {
                Control = GamePadControl.RightTrigger,
                TargetSettingInput = txtTriggerRight,
                DefaultKeyBind = Keys.D2
            });

            // Left half of controller
            allSettings.Add(new GamePadInputSetting
            {
                Control = GamePadControl.LeftStickUp,
                TargetSettingInput = txtStickLUp,
                DefaultKeyBind = Keys.W
            });
            allSettings.Add(new GamePadInputSetting
            {
                Control = GamePadControl.LeftStickRight,
                TargetSettingInput = txtStickLRight,
                DefaultKeyBind = Keys.D
            });
            allSettings.Add(new GamePadInputSetting
            {
                Control = GamePadControl.LeftStickDown,
                TargetSettingInput = txtStickLDown,
                DefaultKeyBind = Keys.S
            });
            allSettings.Add(new GamePadInputSetting
            {
                Control = GamePadControl.LeftStickLeft,
                TargetSettingInput = txtStickLLeft,
                DefaultKeyBind = Keys.A
            });
            allSettings.Add(new GamePadInputSetting
            {
                Control = GamePadControl.LeftStickClick,
                TargetSettingInput = txtStickLClick,
                DefaultKeyBind = Keys.LControlKey
            });

            allSettings.Add(new GamePadInputSetting
            {
                Control = GamePadControl.DPadUp,
                TargetSettingInput = txtDpadUp,
                DefaultKeyBind = Keys.Up
            });
            allSettings.Add(new GamePadInputSetting
            {
                Control = GamePadControl.DPadRight,
                TargetSettingInput = txtDpadRight,
                DefaultKeyBind = Keys.Right
            });
            allSettings.Add(new GamePadInputSetting
            {
                Control = GamePadControl.DPadDown,
                TargetSettingInput = txtDpadDown,
                DefaultKeyBind = Keys.Down
            });
            allSettings.Add(new GamePadInputSetting
            {
                Control = GamePadControl.DPadLeft,
                TargetSettingInput = txtDpadLeft,
                DefaultKeyBind = Keys.Left
            });

            // Right half of controller
            allSettings.Add(new GamePadInputSetting
            {
                Control = GamePadControl.RightStickUp,
                TargetSettingInput = txtStickRUp,
                //DefaultKeyBind = Keys.I,
                DefaultAxisBind = AxisDirection.Up
            });
            allSettings.Add(new GamePadInputSetting
            {
                Control = GamePadControl.RightStickRight,
                TargetSettingInput = txtStickRRight,
                //DefaultKeyBind = Keys.L,
                DefaultAxisBind = AxisDirection.Right
            });
            allSettings.Add(new GamePadInputSetting
            {
                Control = GamePadControl.RightStickDown,
                TargetSettingInput = txtStickRDown,
                //DefaultKeyBind = Keys.K,
                DefaultAxisBind = AxisDirection.Down
            });
            allSettings.Add(new GamePadInputSetting
            {
                Control = GamePadControl.RightStickLeft,
                TargetSettingInput = txtStickRLeft,
                //DefaultKeyBind = Keys.J,
                DefaultAxisBind = AxisDirection.Left
            });
            allSettings.Add(new GamePadInputSetting
            {
                Control = GamePadControl.RightStickClick,
                TargetSettingInput = txtStickRClick,
                DefaultKeyBind = Keys.RControlKey
            });

            allSettings.Add(new GamePadInputSetting
            {
                Control = GamePadControl.X,
                TargetSettingInput = txtX,
                DefaultKeyBind = Keys.X
            });
            allSettings.Add(new GamePadInputSetting
            {
                Control = GamePadControl.Y,
                TargetSettingInput = txtY,
                DefaultKeyBind = Keys.Y
            });
            allSettings.Add(new GamePadInputSetting
            {
                Control = GamePadControl.A,
                TargetSettingInput = txtA,
                DefaultKeyBind = Keys.F
            });
            allSettings.Add(new GamePadInputSetting
            {
                Control = GamePadControl.B,
                TargetSettingInput = txtB,
                DefaultKeyBind = Keys.Z
            });

            foreach (var inputSetting in allSettings)
            {
                if (inputSetting.DefaultKeyBind != null)
                {
                    inputSetting.TargetSettingInput.Text = VirtualKeyConverter.KeysToString((Keys)inputSetting.DefaultKeyBind);

                    binds.Add((Keys)inputSetting.DefaultKeyBind, inputSetting);
                }else if(inputSetting.DefaultAxisBind != null)
                {
                    inputSetting.TargetSettingInput.Text = "Mouse " + Enum.GetName(typeof(AxisDirection), inputSetting.DefaultAxisBind);

                    mouseAxisBinds.Add((AxisDirection)inputSetting.DefaultAxisBind, inputSetting);
                }
            }
        }

        public void SetupKeyboardHooks()
        {
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

                if (mouseAxisBinds.Count == 0)
                    return;

                timerAxisTimeout.Stop();
                timerAxisTimeout.Start();

                var controllerId = 0;
                var state = SimGamePad.Instance.State[controllerId];

                var screen = Screen.FromPoint(Cursor.Position);
                var deltaX = (short)Math.Min(Math.Max(mouse.Mouse.LastX * short.MaxValue * SENSITIVITY, short.MinValue), short.MaxValue);
                var deltaY = (short)-Math.Min(Math.Max(mouse.Mouse.LastY * short.MaxValue * SENSITIVITY, short.MinValue), short.MaxValue);
                GamePadInputSetting inputSetting;

                if (
                    (
                        deltaX > 0
                        && mouseAxisBinds.TryGetValue(AxisDirection.Right, out inputSetting)
                    )
                    ||
                    (
                        deltaX < 0
                        && mouseAxisBinds.TryGetValue(AxisDirection.Left, out inputSetting)
                    )
                )
                {
                    if (inputSetting.Control == GamePadControl.RightStickRight
                        || inputSetting.Control == GamePadControl.RightStickLeft) // TODO: The rest (LeftStick, DPad, etc)
                        state.RightStickX = (short)((deltaX + state.RightStickX) / 2);
                }
                if (
                    (
                        deltaY > 0
                        && mouseAxisBinds.TryGetValue(AxisDirection.Up, out inputSetting)
                    )
                    ||
                    (
                        deltaY < 0
                        && mouseAxisBinds.TryGetValue(AxisDirection.Down, out inputSetting)
                    )
                )
                {
                    if (inputSetting.Control == GamePadControl.RightStickUp
                        || inputSetting.Control == GamePadControl.RightStickDown) // TODO: The rest (LeftStick, DPad, etc)
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

            if (!binds.TryGetValue(keys, out var inputSetting))
                return;

            if (e.KeyboardState == GlobalInputHook.KeyboardState.KeyDown)
                SimGamePad.Instance.SetControl(inputSetting.Control);
            else if (e.KeyboardState == GlobalInputHook.KeyboardState.KeyUp)
                SimGamePad.Instance.ReleaseControl(inputSetting.Control);

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
    }
}
