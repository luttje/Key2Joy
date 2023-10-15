using System;
using System.Windows.Forms;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Triggers;
using Key2Joy.LowLevelInput;
using Key2Joy.Mapping.Triggers;
using Key2Joy.Mapping.Triggers.Keyboard;

namespace Key2Joy.Gui.Mapping;

[MappingControl(
    ForType = typeof(KeyboardTrigger),
    ImageResourceName = "keyboard"
)]
public partial class KeyboardTriggerControl : UserControl, ITriggerOptionsControl
{
    private const string TEXT_CHANGE = "(press any key to select it as the trigger)";
    private const string TEXT_CHANGE_INSTRUCTION = "(click here, then press any key to set it as the trigger)";
    private readonly VirtualKeyConverter virtualKeyConverter = new();

    public event EventHandler OptionsChanged;

    private Keys keys;
    private bool isTrapping;

    public KeyboardTriggerControl()
    {
        this.InitializeComponent();

        // This captures global keyboard input and blocks default behaviour by setting e.Handled
        GlobalInputHook globalKeyboardHook = new();
        globalKeyboardHook.KeyboardInputEvent += this.OnKeyInputEvent;

        // Relieve input capturing by this mapping form
        Disposed += (s, e) =>
        {
            if (globalKeyboardHook == null)
            {
                return;
            }

            globalKeyboardHook.KeyboardInputEvent -= this.OnKeyInputEvent;
            globalKeyboardHook.Dispose();
            globalKeyboardHook = null;
        };
        ControlRemoved += (s, e) => this.Dispose();

        this.cmbPressState.DataSource = PressStates.ALL;
        this.cmbPressState.SelectedIndex = 0;

        // Removed because this is annoying when you want to just edit code
        //StartTrapping();
    }

    private void OnKeyInputEvent(object sender, GlobalKeyboardHookEventArgs e)
    {
        if (!this.isTrapping)
        {
            return;
        }

        this.keys = this.virtualKeyConverter.KeysFromVirtual(e.KeyboardData.VirtualCode);
        this.UpdateKeys();
        this.StopTrapping();
    }

    public void Select(AbstractTrigger trigger)
    {
        var thisTrigger = (KeyboardTrigger)trigger;

        this.keys = thisTrigger.Keys;
        this.cmbPressState.SelectedItem = thisTrigger.PressState;
        this.UpdateKeys();
    }

    public void Setup(AbstractTrigger trigger)
    {
        var thisTrigger = (KeyboardTrigger)trigger;

        thisTrigger.Keys = this.keys;
        thisTrigger.PressState = (PressState)this.cmbPressState.SelectedItem;
    }

    private void StartTrapping()
    {
        this.txtKeyBind.Text = TEXT_CHANGE;
        this.txtKeyBind.Focus();
        this.isTrapping = true;
    }

    private void StopTrapping() => this.isTrapping = false;

    private void UpdateKeys()
    {
        this.txtKeyBind.Text = $"{this.keys} {TEXT_CHANGE_INSTRUCTION}";
        OptionsChanged?.Invoke(this, EventArgs.Empty);
    }

    private void CmbPressedState_SelectedIndexChanged(object sender, EventArgs e) => OptionsChanged?.Invoke(this, EventArgs.Empty);

    private void TxtKeyBind_MouseUp(object sender, MouseEventArgs e) => this.StartTrapping();
}
