using System;
using System.Linq;
using System.Windows.Forms;
using CommonServiceLocator;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.LowLevelInput.SimulatedGamePad;
using Key2Joy.Mapping.Actions.Input;
using Key2Joy.Mapping.Triggers.GamePad;

namespace Key2Joy.Gui.Mapping;

[MappingControl(
    ForType = typeof(GamePadStickAction),
    ImageResourceName = "joystick"
)]
public partial class GamePadStickActionControl : UserControl, IActionOptionsControl
{
    public event EventHandler OptionsChanged;

    public GamePadStickActionControl()
    {
        this.InitializeComponent();

        var gamePadService = ServiceLocator.Current.GetInstance<ISimulatedGamePadService>();
        var allGamePads = gamePadService.GetAllGamePads();
        var allGamePadIndices = allGamePads.Select(gp => gp.Index).ToArray();

        this.cmbSide.DataSource = Enum.GetValues(typeof(GamePadSide));

        this.nudTriggerInputScaleX.Minimum = this.nudTriggerInputScaleY.Minimum = short.MinValue;
        this.nudTriggerInputScaleX.Maximum = this.nudTriggerInputScaleY.Maximum = short.MaxValue;
        this.nudTriggerInputScaleX.DecimalPlaces = this.nudTriggerInputScaleY.DecimalPlaces = 4;

        this.nudExactDeltaX.DecimalPlaces = this.nudExactDeltaY.DecimalPlaces = 2;

        this.nudExactDeltaX.Minimum = this.nudExactDeltaY.Minimum = short.MinValue;
        this.nudExactDeltaX.Maximum = this.nudExactDeltaY.Maximum = short.MaxValue;

        this.cmbGamePadIndex.DataSource = allGamePadIndices;
        this.cmbGamePadIndex.SelectedIndex = 0;

        this.chkDeltaFromInput.Checked = true;
    }

    public void Select(object action)
    {
        var thisAction = (GamePadStickAction)action;

        this.chkDeltaFromInput.Checked = thisAction.DeltaX == null || thisAction.DeltaY == null;
        this.nudTriggerInputScaleX.Value = (decimal)thisAction.InputScaleX;
        this.nudTriggerInputScaleY.Value = (decimal)thisAction.InputScaleY;
        this.cmbSide.SelectedItem = thisAction.Side;
        this.nudExactDeltaX.Value = thisAction.DeltaX ?? 0;
        this.nudExactDeltaY.Value = thisAction.DeltaY ?? 0;
        this.cmbGamePadIndex.SelectedItem = thisAction.GamePadIndex;
    }

    public void Setup(object action)
    {
        var thisAction = (GamePadStickAction)action;

        var deltaFromInput = this.chkDeltaFromInput.Checked;

        thisAction.InputScaleX = (float)this.nudTriggerInputScaleX.Value;
        thisAction.InputScaleY = (float)this.nudTriggerInputScaleY.Value;
        thisAction.Side = (GamePadSide)this.cmbSide.SelectedItem;
        thisAction.DeltaX = deltaFromInput ? null : (short)this.nudExactDeltaX.Value;
        thisAction.DeltaY = deltaFromInput ? null : (short)this.nudExactDeltaY.Value;
        thisAction.GamePadIndex = (int)this.cmbGamePadIndex.SelectedItem;
    }

    private void MakeChildrenEnabled(Control parent, bool enabled)
    {
        foreach (Control child in parent.Controls)
        {
            child.Enabled = enabled;
        }
    }

    private void UpdateDeltaFromTrigger()
    {
        this.MakeChildrenEnabled(this.pnlTriggerInputScale, this.chkDeltaFromInput.Checked);
        this.MakeChildrenEnabled(this.pnlDeltaConfig, !this.chkDeltaFromInput.Checked);
    }

    private void ChkDeltaFromInput_CheckedChanged(object sender, EventArgs e)
    {
        OptionsChanged?.Invoke(this, EventArgs.Empty);
        this.UpdateDeltaFromTrigger();
    }

    public bool CanMappingSave(object action) => true;

    private void CmbGamePad_SelectedIndexChanged(object sender, EventArgs e)
        => OptionsChanged?.Invoke(this, EventArgs.Empty);

    private void CmbPressState_SelectedIndexChanged(object sender, EventArgs e)
        => OptionsChanged?.Invoke(this, EventArgs.Empty);

    private void CmbGamePadIndex_SelectedIndexChanged(object sender, EventArgs e)
        => OptionsChanged?.Invoke(this, EventArgs.Empty);

    private void NudExactDeltaX_ValueChanged(object sender, EventArgs e)
        => OptionsChanged?.Invoke(this, EventArgs.Empty);

    private void NudExactDeltaY_ValueChanged(object sender, EventArgs e)
        => OptionsChanged?.Invoke(this, EventArgs.Empty);

    private void NudTriggerInputScaleX_ValueChanged(object sender, EventArgs e)
        => OptionsChanged?.Invoke(this, EventArgs.Empty);

    private void NudTriggerInputScaleY_ValueChanged(object sender, EventArgs e)
        => OptionsChanged?.Invoke(this, EventArgs.Empty);
}
