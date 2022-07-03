using Linearstar.Windows.RawInput;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Linearstar.Windows.RawInput.Native;
using Key2Joy.Mapping;
using Key2Joy.LowLevelInput;

namespace Key2Joy
{
    public partial class KeyboardTriggerOptionsControl : UserControl, ITriggerOptionsControl
    {
        const string TEXT_CHANGE = "(press any key to select it as the trigger)";
        const string TEXT_CHANGE_INSTRUCTION = "(click here, then press any key to set it as the trigger)";
        const int WM_INPUT = 0x00FF;

        public event Action OptionsChanged;
        
        private Keys keys;
        private bool isTrapping;

        public KeyboardTriggerOptionsControl()
        {
            InitializeComponent();

            RawInputDevice.RegisterDevice(HidUsageAndPage.Keyboard, RawInputDeviceFlags.InputSink, Handle);

            // Relieve input capturing by this mapping form and return it to the main form
            this.Disposed += (s,e) => RawInputDevice.UnregisterDevice(HidUsageAndPage.Keyboard);

            cmbPressedState.DataSource = Enum.GetValues(typeof(PressState));
            cmbPressedState.SelectedIndex = 0;

            StartTrapping();
        }

        public void Select(BaseTrigger trigger)
        {
            var thisTrigger = (KeyboardTrigger)trigger;

            this.keys = thisTrigger.Keys;
            cmbPressedState.SelectedItem = thisTrigger.PressState;
            UpdateKeys();
        }

        public void Setup(BaseTrigger trigger)
        {
            var thisTrigger = (KeyboardTrigger)trigger;

            thisTrigger.Keys = this.keys;
            thisTrigger.PressState = (PressState) cmbPressedState.SelectedItem;
        }

        private void StartTrapping()
        {
            txtKeyBind.Text = TEXT_CHANGE;
            txtKeyBind.Focus();
            isTrapping = true;
        }
        
        private void StopTrapping()
        {
            isTrapping = false;
        }

        private void ResetTrapping()
        {
            txtKeyBind.Text = TEXT_CHANGE_INSTRUCTION;
            StopTrapping();
        }

        private void UpdateKeys(RawKeyboardFlags? flags = null)
        {
            if (flags != null)
            {
                if ((flags & RawKeyboardFlags.KeyE0) == RawKeyboardFlags.KeyE0)
                {
                    if (keys == Keys.ControlKey)
                        keys = Keys.RControlKey;
                    if (keys == Keys.ShiftKey)
                        keys = Keys.RShiftKey;
                }
                else
                {
                    if (keys == Keys.ControlKey)
                        keys = Keys.LControlKey;
                    if (keys == Keys.ShiftKey)
                        keys = Keys.LShiftKey;
                }
            }

            txtKeyBind.Text = $"{keys} {TEXT_CHANGE_INSTRUCTION}";
            OptionsChanged?.Invoke();
        }

        private void cmbPressedState_SelectedIndexChanged(object sender, EventArgs e)
        {
            OptionsChanged?.Invoke();
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            
            if (!isTrapping)
                return;
            
            if (m.Msg == WM_INPUT)
            {
                var data = RawInputData.FromHandle(m.LParam);

                if (data is RawInputKeyboardData keyboard)
                {
                    keys = VirtualKeyConverter.KeysFromVirtual(keyboard.Keyboard.VirutalKey);
                    UpdateKeys(keyboard.Keyboard.Flags);
                    StopTrapping();
                }
            }
        }

        private void txtKeyBind_MouseUp(object sender, MouseEventArgs e)
        {
            StartTrapping();
        }
    }
}
