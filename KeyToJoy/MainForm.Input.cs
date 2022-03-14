using KeyToJoy.Input;
using KeyToJoy.Input.LowLevel;
using Linearstar.Windows.RawInput;
using Linearstar.Windows.RawInput.Native;
using SimWinInput;
using System;
using System.Windows.Forms;

namespace KeyToJoy
{
    public partial class MainForm
    {
        const double SENSITIVITY = 0.05;
        const int WM_INPUT = 0x00FF;

        private bool TryOverrideKeyboardInput(Keys keys, bool isPressedDown)
        {
            if (!chkEnabled.Checked)
                return false;

            if (!selectedPreset.TryGetBinding(new KeyboardBinding(keys), out var bindingOption))
                return false;

            if (isPressedDown)
                SimGamePad.Instance.SetControl(bindingOption.Control);
            else
                SimGamePad.Instance.ReleaseControl(bindingOption.Control);

            return true;
        }

        // TODO: Needs to be cleaned up
        // TODO: Sensitivity should be tweakable by user
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

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg != WM_INPUT)
                return;

            var data = RawInputData.FromHandle(m.LParam);

            if (!(data is RawInputMouseData mouse))
                return;

            if (TryOverrideMouseInput(mouse.Mouse.LastX, mouse.Mouse.LastY))
                return;
        }

        private void OnKeyInputEvent(object sender, GlobalKeyboardHookEventArgs e)
        {
            if (!chkEnabled.Checked)
                return;

            // Test if this is a bound key, if so halt default input behaviour
            var keys = VirtualKeyConverter.KeysFromVirtual(e.KeyboardData.VirtualCode);
            if (!selectedPreset.TryGetBinding(new KeyboardBinding(keys), out var _))
                return;

            if (!TryOverrideKeyboardInput(keys, e.KeyboardState == KeyboardState.KeyDown))
                return;

            e.Handled = true;
        }
    }
}
