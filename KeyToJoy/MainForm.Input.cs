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

        public void RefreshInputCaptures()
        {
            RawInputDevice.RegisterDevice(HidUsageAndPage.Mouse, RawInputDeviceFlags.InputSink, Handle);

            // This captures global keyboard input and blocks default behaviour by setting e.Handled
            globalKeyboardHook = new GlobalInputHook();
            globalKeyboardHook.KeyboardInputEvent += OnKeyInputEvent;
            globalKeyboardHook.MouseInputEvent += OnMouseButtonInputEvent;
        }

        private bool TryOverrideKeyboardInput(BindingOption bindingOption, bool isPressedDown)
        {
            if (!chkEnabled.Checked)
                return false;

            bindingOption.Action.PerformPressBind(isPressedDown);

            return true;
        }

        private bool TryOverrideMouseButtonInput(BindingOption bindingOption, bool isPressedDown)
        {
            if (!chkEnabled.Checked)
                return false;

            bindingOption.Action.PerformPressBind(isPressedDown);

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
                if(bindingOption != null)
                    // TODO: The rest (LeftStick, DPad, etc)
                    state.RightStickX = bindingOption.Action.PerformMoveBind(deltaX, state.RightStickX);
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
                if (bindingOption != null)
                    // TODO: The rest (LeftStick, DPad, etc)
                    state.RightStickY = bindingOption.Action.PerformMoveBind(deltaY, state.RightStickY);
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
