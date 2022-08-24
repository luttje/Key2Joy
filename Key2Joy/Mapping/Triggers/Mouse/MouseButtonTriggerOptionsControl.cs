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
        public event EventHandler OptionsChanged;
        
        private Mouse.Buttons mouseButtons;
        private bool isShowingError;

        public MouseButtonTriggerOptionsControl()
        {
            InitializeComponent();

            // This captures global keyboard input and blocks default behaviour by setting e.Handled
            var globalMouseHook = new GlobalInputHook();
            globalMouseHook.MouseInputEvent += OnMouseInputEvent;

            // Relieve input capturing by this mapping form
            Disposed += (s, e) =>
            {
                globalMouseHook.MouseInputEvent -= OnMouseInputEvent;
                globalMouseHook.Dispose();
                globalMouseHook = null;
            };
            ControlRemoved += (s, e) => Dispose();

            cmbPressState.DataSource = LegacyPressStateConverter.GetPressStatesWithoutLegacy();
            cmbPressState.SelectedIndex = 0;
        }

        private void OnMouseInputEvent(object sender, GlobalMouseHookEventArgs e)
        {
            if (e.MouseState == MouseState.Move
                || !txtKeyBind.ClientRectangle.Contains(txtKeyBind.PointToClient(MousePosition)))
                return;

            var isDown = false;

            try
            {
                mouseButtons = Mouse.ButtonsFromState(e.MouseState, out isDown);
            }
            catch (NotImplementedException ex) 
            {
                if (!isShowingError)
                {
                    isShowingError = true;
                    MessageBox.Show($"{ex.Message}. Can't map this (yet).", "Unknown mouse button!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    isShowingError = false;
                }
            }

            txtKeyBind.Text = $"{mouseButtons}";
            OptionsChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Select(BaseTrigger trigger)
        {
            var thisTrigger = (MouseButtonTrigger)trigger;

            mouseButtons = thisTrigger.MouseButtons;
            cmbPressState.SelectedItem = thisTrigger.PressState;
            txtKeyBind.Text = $"{mouseButtons}";
        }

        public void Setup(BaseTrigger trigger)
        {
            var thisTrigger = (MouseButtonTrigger)trigger;

            thisTrigger.MouseButtons = mouseButtons;
            thisTrigger.PressState = (PressState) cmbPressState.SelectedItem;
        }

        private void cmbPressedState_SelectedIndexChanged(object sender, EventArgs e)
        {
            OptionsChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
