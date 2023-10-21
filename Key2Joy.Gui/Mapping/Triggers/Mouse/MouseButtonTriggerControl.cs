using System;
using System.Windows.Forms;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Triggers;
using Key2Joy.LowLevelInput;
using Key2Joy.Mapping.Triggers;
using Key2Joy.Mapping.Triggers.Mouse;

namespace Key2Joy.Gui.Mapping;

[MappingControl(
    ForType = typeof(MouseButtonTrigger),
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
        this.InitializeComponent();

        // This captures global keyboard input and blocks default behaviour by setting e.Handled
        GlobalInputHook globalMouseHook = new();
        globalMouseHook.MouseInputEvent += this.OnMouseInputEvent;

        // Relieve input capturing by this mapping form
        Disposed += (s, e) =>
        {
            globalMouseHook.MouseInputEvent -= this.OnMouseInputEvent;
            globalMouseHook.Dispose();
            globalMouseHook = null;
        };
        ControlRemoved += (s, e) => this.Dispose();

        this.cmbPressState.DataSource = PressStates.ALL;
        this.cmbPressState.SelectedIndex = 0;
    }

    private void OnMouseInputEvent(object sender, GlobalMouseHookEventArgs e)
    {
        // Needed to make sure the cursor is immediately over the control, and not over a comboboxitem which is over the control.
        if (!this.isMouseOver)
        {
            return;
        }

        if (e.MouseState == MouseState.Move
            || !this.txtKeyBind.ClientRectangle.Contains(this.txtKeyBind.PointToClient(MousePosition)))
        {
            return;
        }

        var isDown = false;

        try
        {
            this.mouseButtons = Mouse.ButtonsFromEvent(e, out isDown);
        }
        catch (NotImplementedException ex)
        {
            if (!this.isShowingError)
            {
                this.isShowingError = true;
                MessageBox.Show($"{ex.Message}. Can't map this (yet).", "Unknown mouse button!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.isShowingError = false;
            }
        }

        this.txtKeyBind.Text = $"{this.mouseButtons}";
        OptionsChanged?.Invoke(this, EventArgs.Empty);
    }

    public void Select(AbstractTrigger trigger)
    {
        var thisTrigger = (MouseButtonTrigger)trigger;

        this.mouseButtons = thisTrigger.MouseButtons;
        this.cmbPressState.SelectedItem = thisTrigger.PressState;
        this.txtKeyBind.Text = $"{this.mouseButtons}";
    }

    public void Setup(AbstractTrigger trigger)
    {
        var thisTrigger = (MouseButtonTrigger)trigger;

        thisTrigger.MouseButtons = this.mouseButtons;
        thisTrigger.PressState = (PressState)this.cmbPressState.SelectedItem;
    }
    
    public bool CanMappingSave(AbstractTrigger trigger) => true;

    private void CmbPressedState_SelectedIndexChanged(object sender, EventArgs e) => OptionsChanged?.Invoke(this, EventArgs.Empty);

    private void TxtKeyBind_MouseEnter(object sender, EventArgs e) => this.isMouseOver = true;

    private void TxtKeyBind_MouseLeave(object sender, EventArgs e) => this.isMouseOver = false;
}
