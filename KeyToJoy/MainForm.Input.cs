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
        private const double SENSITIVITY = 0.05;
        private const int WM_INPUT = 0x00FF;
        private readonly Keys[] KEYS_ABORT = new[] { Keys.LControlKey, Keys.LShiftKey, Keys.Escape };

        private Keys abortKeysMask; // On load this is set to the bitwise or of all in KEYS_ABORT
        private Keys currentAbortKeysDown = Keys.None;

        private void Init()
        {
            abortKeysMask = Keys.None;

            for (int i = 0; i < KEYS_ABORT.Length; i++)
            {
                abortKeysMask |= KEYS_ABORT[i];
            }
        }

        private bool TryOverrideKeyboardInput(BindingOption bindingOption, bool isPressedDown)
        {
            if (!chkEnabled.Checked)
                return false;

            if (isPressedDown)
                SimGamePad.Instance.SetControl(bindingOption.Control);
            else
                SimGamePad.Instance.ReleaseControl(bindingOption.Control);

            return true;
        }

        private bool TryOverrideMouseButtonInput(BindingOption bindingOption, bool isPressedDown)
        {
            if (!chkEnabled.Checked)
                return false;

            if (isPressedDown)
                SimGamePad.Instance.SetControl(bindingOption.Control);
            else
                SimGamePad.Instance.ReleaseControl(bindingOption.Control);

            return true;
        }

        // TODO: Needs to be cleaned up
        // TODO: Sensitivity should be tweakable by user
        private bool TryOverrideMouseMoveInput(int lastX, int lastY)
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

            if (TryOverrideMouseMoveInput(mouse.Mouse.LastX, mouse.Mouse.LastY))
                return;
        }

        private void OnKeyInputEvent(object sender, GlobalKeyboardHookEventArgs e)
        {
            if (!chkEnabled.Checked)
                return;

            // Test if this is a bound key, if so halt default input behaviour
            var keys = VirtualKeyConverter.KeysFromVirtual(e.KeyboardData.VirtualCode);

            // Check early if we want to abort
            for (int i = 0; i < KEYS_ABORT.Length; i++)
            {
                var abortKey = KEYS_ABORT[i];

                if((keys & abortKey) == abortKey)
                {
                    if (e.KeyboardState == KeyboardState.KeyDown)
                        currentAbortKeysDown |= abortKey;
                    else
                        currentAbortKeysDown &= ~abortKey;
                }
            }

            if (currentAbortKeysDown == abortKeysMask)
            {
                currentAbortKeysDown = Keys.None;
                chkEnabled.Checked = false;
                return;
            }

            if (!selectedPreset.TryGetBinding(new KeyboardBinding(keys), out var bindingOption))
                return;

            if (!TryOverrideKeyboardInput(bindingOption, e.KeyboardState == KeyboardState.KeyDown))
                return;

            e.Handled = true;
        }

        private void OnMouseButtonInputEvent(object sender, GlobalMouseHookEventArgs e)
        {
            if (!chkEnabled.Checked)
                return;

            // Mouse movement is handled through WndProc and TryOverrideMouseMoveInput
            if (e.MouseState == MouseState.Move)
                return;

            try
            {
                // Test if this is a bound mouse button, if so halt default input behaviour
                if (!selectedPreset.TryGetBinding(new MouseBinding(e.MouseState), out var bindingOption))
                    return;

                if (!TryOverrideMouseButtonInput(bindingOption, e.AreButtonsDown()))
                    return;

                e.Handled = true;
            }
            catch (ArgumentOutOfRangeException _) { }
        }
    }
}
