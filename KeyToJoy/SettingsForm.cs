using SimWinInput;
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
    public partial class SettingsForm : Form
    {
        private List<GamePadInputSetting> allSettings;
        private Dictionary<Keys, GamePadInputSetting> binds = new Dictionary<Keys, GamePadInputSetting>();

        private GlobalKeyboardHook _globalKeyboardHook;

        public SettingsForm()
        {
            InitializeComponent();

            SimGamePad.Instance.Initialize();
            SetupKeyboardHooks();
            SimGamePad.Instance.PlugIn();

            allSettings = new List<GamePadInputSetting>();

            // Top of controller
            allSettings.Add(new GamePadInputSetting
            {
                Control = GamePadControl.LeftShoulder,
                TargetSettingInput = txtShoulderLeft,
                DefaultBind = Keys.Q
            });
            allSettings.Add(new GamePadInputSetting
            {
                Control = GamePadControl.LeftTrigger,
                TargetSettingInput = txtTriggerLeft,
                DefaultBind = Keys.D1
            });
            allSettings.Add(new GamePadInputSetting
            {
                Control = GamePadControl.RightShoulder,
                TargetSettingInput = txtShoulderRight,
                DefaultBind = Keys.E
            });
            allSettings.Add(new GamePadInputSetting
            {
                Control = GamePadControl.RightTrigger,
                TargetSettingInput = txtTriggerRight,
                DefaultBind = Keys.D2
            });

            // Left half of controller
            allSettings.Add(new GamePadInputSetting
            {
                Control = GamePadControl.LeftStickUp,
                TargetSettingInput = txtStickLUp,
                DefaultBind = Keys.W
            });
            allSettings.Add(new GamePadInputSetting
            {
                Control = GamePadControl.LeftStickRight,
                TargetSettingInput = txtStickLRight,
                DefaultBind = Keys.D
            });
            allSettings.Add(new GamePadInputSetting
            {
                Control = GamePadControl.LeftStickDown,
                TargetSettingInput = txtStickLDown,
                DefaultBind = Keys.S
            });
            allSettings.Add(new GamePadInputSetting
            {
                Control = GamePadControl.LeftStickLeft,
                TargetSettingInput = txtStickLLeft,
                DefaultBind = Keys.A
            });
            allSettings.Add(new GamePadInputSetting
            {
                Control = GamePadControl.LeftStickClick,
                TargetSettingInput = txtStickLClick,
                DefaultBind = Keys.LControlKey
            });

            allSettings.Add(new GamePadInputSetting
            {
                Control = GamePadControl.DPadUp,
                TargetSettingInput = txtDpadUp,
                DefaultBind = Keys.I
            });
            allSettings.Add(new GamePadInputSetting
            {
                Control = GamePadControl.DPadRight,
                TargetSettingInput = txtDpadRight,
                DefaultBind = Keys.L
            });
            allSettings.Add(new GamePadInputSetting
            {
                Control = GamePadControl.DPadDown,
                TargetSettingInput = txtDpadDown,
                DefaultBind = Keys.K
            });
            allSettings.Add(new GamePadInputSetting
            {
                Control = GamePadControl.DPadLeft,
                TargetSettingInput = txtDpadLeft,
                DefaultBind = Keys.J
            });

            // Right half of controller
            allSettings.Add(new GamePadInputSetting
            {
                Control = GamePadControl.RightStickUp,
                TargetSettingInput = txtStickRUp,
                DefaultBind = Keys.Up
            });
            allSettings.Add(new GamePadInputSetting
            {
                Control = GamePadControl.RightStickRight,
                TargetSettingInput = txtStickRRight,
                DefaultBind = Keys.Right
            });
            allSettings.Add(new GamePadInputSetting
            {
                Control = GamePadControl.RightStickDown,
                TargetSettingInput = txtStickRDown,
                DefaultBind = Keys.Down
            });
            allSettings.Add(new GamePadInputSetting
            {
                Control = GamePadControl.RightStickLeft,
                TargetSettingInput = txtStickRLeft,
                DefaultBind = Keys.Left
            });
            allSettings.Add(new GamePadInputSetting
            {
                Control = GamePadControl.RightStickClick,
                TargetSettingInput = txtStickRClick,
                DefaultBind = Keys.RControlKey
            });

            allSettings.Add(new GamePadInputSetting
            {
                Control = GamePadControl.X,
                TargetSettingInput = txtX,
                DefaultBind = Keys.X
            });
            allSettings.Add(new GamePadInputSetting
            {
                Control = GamePadControl.Y,
                TargetSettingInput = txtY,
                DefaultBind = Keys.Y
            });
            allSettings.Add(new GamePadInputSetting
            {
                Control = GamePadControl.A,
                TargetSettingInput = txtA,
                DefaultBind = Keys.F
            });
            allSettings.Add(new GamePadInputSetting
            {
                Control = GamePadControl.B,
                TargetSettingInput = txtB,
                DefaultBind = Keys.Z
            });

            foreach (var inputSetting in allSettings)
            {
                inputSetting.TargetSettingInput.Text = VirtualKeyConverter.KeysToString(inputSetting.DefaultBind);

                binds.Add(inputSetting.DefaultBind, inputSetting);
            }
        }

        public void SetupKeyboardHooks()
        {
            _globalKeyboardHook = new GlobalKeyboardHook();
            _globalKeyboardHook.KeyboardInputEvent += OnKeyInputEvent;
        }

        private void OnKeyInputEvent(object sender, GlobalKeyboardHookEventArgs e)
        {
            if (!chkEnabled.Checked)
                return;

            //https://docs.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes
            var keys = VirtualKeyConverter.KeysFromVirtual(e.KeyboardData.VirtualCode);

            if (!binds.TryGetValue(keys, out var inputSetting))
                return;

            if (e.KeyboardState == GlobalKeyboardHook.KeyboardState.KeyDown)
                SimGamePad.Instance.SetControl(inputSetting.Control);
            else if (e.KeyboardState == GlobalKeyboardHook.KeyboardState.KeyUp)
                SimGamePad.Instance.ReleaseControl(inputSetting.Control);

            e.Handled = true;
        }

        public void Dispose()
        {
            _globalKeyboardHook?.Dispose();
        }
    }
}
