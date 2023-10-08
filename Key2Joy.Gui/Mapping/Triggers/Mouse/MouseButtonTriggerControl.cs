using Key2Joy.Contracts.Mapping;
using Key2Joy.LowLevelInput;
using Key2Joy.Mapping;
using System;
using System.Windows.Forms;

namespace Key2Joy.Gui.Mapping
{
    [MappingControl(
        ForType = typeof(Key2Joy.Mapping.MouseButtonTrigger),
        ImageResourceName = "mouse"
    )]
    public partial class MouseButtonTriggerControl : UserControl, ITriggerOptionsControl
    {
        public event EventHandler OptionsChanged;

        private Mouse.Buttons mouseButtons;
        private bool isShowingError;
        private bool isMouseOver;

        public MouseButtonTriggerControl()
        {
            InitializeComponent();

            // This captures global keyboard input and blocks default behaviour by setting e.Handled
            GlobalInputHook globalMouseHook = new();
            globalMouseHook.MouseInputEvent += OnMouseInputEvent;

            // Relieve input capturing by this mapping form
            Disposed += (s, e) =>
            {
                globalMouseHook.MouseInputEvent -= OnMouseInputEvent;
                globalMouseHook.Dispose();
                globalMouseHook = null;
            };
            ControlRemoved += (s, e) => Dispose();

            cmbPressState.DataSource = PressStates.ALL;
            cmbPressState.SelectedIndex = 0;
        }

        private void OnMouseInputEvent(object sender, GlobalMouseHookEventArgs e)
        {
            // Needed to make sure the cursor is immediately over the control, and not over a comboboxitem which is over the control.
            if (!isMouseOver)
            {
                return;
            }

            if (e.MouseState == MouseState.Move
                || !txtKeyBind.ClientRectangle.Contains(txtKeyBind.PointToClient(MousePosition)))
            {
                return;
            }

            var isDown = false;

            try
            {
                mouseButtons = Mouse.ButtonsFromEvent(e, out isDown);
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

        public void Select(AbstractTrigger trigger)
        {
            var thisTrigger = (MouseButtonTrigger)trigger;

            mouseButtons = thisTrigger.MouseButtons;
            cmbPressState.SelectedItem = thisTrigger.PressState;
            txtKeyBind.Text = $"{mouseButtons}";
        }

        public void Setup(AbstractTrigger trigger)
        {
            var thisTrigger = (MouseButtonTrigger)trigger;

            thisTrigger.MouseButtons = mouseButtons;
            thisTrigger.PressState = (PressState)cmbPressState.SelectedItem;
        }

        private void CmbPressedState_SelectedIndexChanged(object sender, EventArgs e)
        {
            OptionsChanged?.Invoke(this, EventArgs.Empty);
        }

        private void TxtKeyBind_MouseEnter(object sender, EventArgs e)
        {
            isMouseOver = true;
        }

        private void TxtKeyBind_MouseLeave(object sender, EventArgs e)
        {
            isMouseOver = false;
        }
    }
}
