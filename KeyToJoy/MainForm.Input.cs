using KeyToJoy.Input;
using KeyToJoy.Input.LowLevel;
using KeyToJoy.Mapping;
using Linearstar.Windows.RawInput;
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

        private bool TryOverrideKeyboardInput(MappedOption mappedOption, KeyboardInputBag inputBag)
        {
            if (!chkEnabled.Checked)
                return false;

            mappedOption.Action.Execute(inputBag);

            return true;
        }

        private bool TryOverrideMouseButtonInput(MappedOption mappedOption, MouseButtonInputBag inputBag)
        {
            if (!chkEnabled.Checked)
                return false;

            mappedOption.Action.Execute(inputBag);

            return true;
        }

        private bool TryOverrideMouseMoveInput(int lastX, int lastY)
        {
            if (!chkEnabled.Checked)
                return false;

            tmrAxisTimeout.Stop();
            tmrAxisTimeout.Start();

            var deltaX = (short)Math.Min(Math.Max(lastX * short.MaxValue * SENSITIVITY, short.MinValue), short.MaxValue);
            var deltaY = (short)-Math.Min(Math.Max(lastY * short.MaxValue * SENSITIVITY, short.MinValue), short.MaxValue);
            MappedOption mappedOption;

            if (
                (
                    deltaX > 0
                    && selectedPreset.TryGetMappedOption(new MouseMoveTrigger(AxisDirection.Right), out mappedOption)
                )
                ||
                (
                    deltaX < 0
                    && selectedPreset.TryGetMappedOption(new MouseMoveTrigger(AxisDirection.Left), out mappedOption)
                )
            )
            {
                if (mappedOption != null)
                    mappedOption.Action.Execute(new MouseMoveInputBag
                    {
                        DeltaX = deltaX,
                    });      
            }
            if (
                (
                    deltaY > 0
                    && selectedPreset.TryGetMappedOption(new MouseMoveTrigger(AxisDirection.Up), out mappedOption)
                )
                ||
                (
                    deltaY < 0
                    && selectedPreset.TryGetMappedOption(new MouseMoveTrigger(AxisDirection.Down), out mappedOption)
                )
            )
            {
                if (mappedOption != null)
                    mappedOption.Action.Execute(new MouseMoveInputBag
                    {
                        DeltaY = deltaY,
                    });
            }

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

            if (!selectedPreset.TryGetMappedOption(new KeyboardTrigger(keys), out var mappedOption))
                return;

            if (!TryOverrideKeyboardInput(mappedOption, new KeyboardInputBag
            {
                State = e.KeyboardState,
                Keys = keys
            }))
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
                if (!selectedPreset.TryGetMappedOption(new MouseButtonTrigger(e.MouseState), out var mappedOption))
                    return;

                if (!TryOverrideMouseButtonInput(mappedOption, new MouseButtonInputBag
                {
                    State = e.MouseState,
                    LastX = e.MouseData.Position.X,
                    LastY = e.MouseData.Position.Y,
                }))
                    return;

                e.Handled = true;
            }
            catch (ArgumentOutOfRangeException _) { }
        }
    }
}
