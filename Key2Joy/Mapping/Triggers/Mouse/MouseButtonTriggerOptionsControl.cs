using Linearstar.Windows.RawInput;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Linearstar.Windows.RawInput.Native;
using Key2Joy.Mapping;
using Key2Joy.LowLevelInput;

namespace Key2Joy
{
    public partial class MouseButtonTriggerOptionsControl : UserControl, ITriggerOptionsControl
    {
        public event Action OptionsChanged;
        
        private Mouse.Buttons mouseButtons;

        public MouseButtonTriggerOptionsControl()
        {
            InitializeComponent();

            RawInputDevice.RegisterDevice(HidUsageAndPage.Mouse, RawInputDeviceFlags.InputSink, Handle);
            
            // Relieve input capturing by this mapping form and return it to the main form
            this.Disposed += (s, e) => RawInputDevice.UnregisterDevice(HidUsageAndPage.Mouse);

            cmbPressedState.DataSource = Enum.GetValues(typeof(PressState));
            cmbPressedState.SelectedIndex = 0;
        }

        public void Select(BaseTrigger trigger)
        {
            var thisTrigger = (MouseButtonTrigger)trigger;

            this.mouseButtons = thisTrigger.MouseButtons;
            cmbPressedState.SelectedItem = thisTrigger.PressedState;
            txtKeyBind.Text = $"{mouseButtons}";
        }

        public void Setup(BaseTrigger trigger)
        {
            var thisTrigger = (MouseButtonTrigger)trigger;

            thisTrigger.MouseButtons = mouseButtons;
            thisTrigger.PressedState = (PressState) cmbPressedState.SelectedItem;
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_INPUT = 0x00FF;

            if (m.Msg == WM_INPUT)
            {
                var data = RawInputData.FromHandle(m.LParam);

                if (data is RawInputMouseData mouse
                    && mouse.Mouse.Buttons != RawMouseButtonFlags.None
                    && txtKeyBind.ClientRectangle.Contains(txtKeyBind.PointToClient(MousePosition)))
                {
                    try
                    {
                        mouseButtons = Mouse.ButtonsFromRaw(mouse.Mouse, out var isDown);
                    }
                    catch (NotImplementedException ex)
                    {
                        MessageBox.Show($"{ex.Message}. Can't map this (yet).", "Unknown mouse button!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    txtKeyBind.Text = $"{mouseButtons}";
                    OptionsChanged?.Invoke();
                }
            }

            base.WndProc(ref m);
        }

        private void cmbPressedState_SelectedIndexChanged(object sender, EventArgs e)
        {
            OptionsChanged?.Invoke();
        }
    }
}
