using System;
using System.Linq;
using System.Windows.Forms;
using CommonServiceLocator;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Contracts.Mapping.Triggers;
using Key2Joy.LowLevelInput;
using Key2Joy.LowLevelInput.XInput;
using Key2Joy.Mapping.Triggers;
using Key2Joy.Mapping.Triggers.GamePad;

namespace Key2Joy.Gui.Mapping;

[MappingControl(
    ForType = typeof(GamePadButtonTrigger),
    ImageResourceName = "joystick"
)]
public partial class GamePadButtonTriggerControl : UserControl, ITriggerOptionsControl
{
    private const string TEXT_CHANGE = "(press any button to select it as the trigger)";
    private const string TEXT_CHANGE_INSTRUCTION = "(click here, then press any button to set it as the trigger)";
    private const string TEXT_LAST_GAMEPAD = "Last GamePad used was #: {0}";

    public event EventHandler OptionsChanged;

    private readonly IXInputService xInputService;
    private GamePadButton button;

    public GamePadButtonTriggerControl()
    {
        this.InitializeComponent();

        this.xInputService = ServiceLocator.Current.GetInstance<IXInputService>();
        this.xInputService.StateChanged += this.XInputService_StateChanged;

        this.cmbPressState.DataSource = PressStates.ALL;
        this.cmbPressState.SelectedIndex = 0;

        this.nudGamePadIndex.Minimum = 0;
        this.nudGamePadIndex.Maximum = XInputService.MaxDevices - 1;

        // Relieve input capturing by this mapping form
        ControlRemoved += (s, e) => this.Dispose();
        this.Disposed += (s, e) =>
        {
            if (this.xInputService == null)
            {
                return;
            }

            this.xInputService.StateChanged -= this.XInputService_StateChanged;
            this.xInputService.StopPolling();
        };
    }

    private void XInputService_StateChanged(object sender, DeviceStateChangedEventArgs e)
    {
        var buttons = e.NewState.Gamepad.GetPressedButtonsList();

        if (buttons.Count == 0)
        {
            // May occur if the stick is moved without pressing any buttons.
            return;
        }

        this.button = buttons.First();

        this.Invoke((MethodInvoker)(() =>
        {
            // Commented because depending on when the device was plugged in,
            // it may change with relation to the other (virtual) gamepads.
            // this.nudGamePadIndex.Value = e.DeviceIndex;
            this.lblLastGamePadLabel.Text = string.Format(TEXT_LAST_GAMEPAD, e.DeviceIndex);
            this.UpdateKeys();
            this.StopTrapping();
        }));
    }

    public void Select(AbstractTrigger trigger)
    {
        var thisTrigger = (GamePadButtonTrigger)trigger;

        this.button = thisTrigger.Button;
        this.cmbPressState.SelectedItem = thisTrigger.PressState;
        this.UpdateKeys();
    }

    public void Setup(AbstractTrigger trigger)
    {
        var thisTrigger = (GamePadButtonTrigger)trigger;

        thisTrigger.Button = this.button;
        thisTrigger.PressState = (PressState)this.cmbPressState.SelectedItem;
    }

    public bool CanMappingSave(AbstractTrigger trigger) => true;

    private void StartTrapping()
    {
        this.txtButtonBind.Text = TEXT_CHANGE;
        this.txtButtonBind.Focus();
        this.xInputService.RecognizePhysicalDevices();
        this.xInputService.StartPolling();
    }

    private void StopTrapping()
        => this.xInputService.StopPolling();

    private void UpdateKeys()
    {
        this.txtButtonBind.Text = $"{this.button} {TEXT_CHANGE_INSTRUCTION}";
        OptionsChanged?.Invoke(this, EventArgs.Empty);
    }

    private void CmbPressedState_SelectedIndexChanged(object sender, EventArgs e)
        => OptionsChanged?.Invoke(this, EventArgs.Empty);

    private void TxtKeyBind_MouseUp(object sender, MouseEventArgs e)
        => this.StartTrapping();

    private void NudGamePadIndex_ValueChanged(object sender, EventArgs e)
        => OptionsChanged?.Invoke(this, EventArgs.Empty);
}
