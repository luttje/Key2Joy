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
    ForType = typeof(GamePadTriggerAction),
    ImageResourceName = "joystick"
)]
public partial class GamePadTriggerActionControl : UserControl, IActionOptionsControl
{
    public event EventHandler OptionsChanged;

    public GamePadTriggerActionControl()
    {
        this.InitializeComponent();

        var gamePadService = ServiceLocator.Current.GetInstance<ISimulatedGamePadService>();
        var allGamePads = gamePadService.GetAllGamePads(false);
        var allGamePadIndices = allGamePads.Select(gp => gp.Index).ToArray();

        this.cmbSide.DataSource = Enum.GetValues(typeof(GamePadSide));

        this.nudTriggerInputScale.Minimum = short.MinValue;
        this.nudTriggerInputScale.Maximum = short.MaxValue;
        this.nudTriggerInputScale.DecimalPlaces = 4;

        this.nudExact.DecimalPlaces = 2;
        this.nudExact.Minimum = short.MinValue;
        this.nudExact.Maximum = short.MaxValue;

        this.cmbGamePadIndex.DataSource = allGamePadIndices;
        this.cmbGamePadIndex.SelectedIndex = 0;

        this.chkDeltaFromInput.Checked = true;
    }

    public void Select(AbstractAction action)
    {
        var thisAction = (GamePadTriggerAction)action;

        this.chkDeltaFromInput.Checked = thisAction.Delta == null;
        this.nudTriggerInputScale.Value = (decimal)thisAction.InputScale;
        this.cmbSide.SelectedItem = thisAction.Side;
        this.nudExact.Value = (decimal)(thisAction.Delta ?? 0);
        this.cmbGamePadIndex.SelectedItem = thisAction.GamePadIndex;
    }

    public void Setup(AbstractAction action)
    {
        var thisAction = (GamePadTriggerAction)action;

        var deltaFromInput = this.chkDeltaFromInput.Checked;

        thisAction.InputScale = (float)this.nudTriggerInputScale.Value;
        thisAction.Side = (GamePadSide)this.cmbSide.SelectedItem;
        thisAction.Delta = deltaFromInput ? null : (float)this.nudExact.Value;
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

    public bool CanMappingSave(AbstractAction action) => true;

    private void CmbGamePad_SelectedIndexChanged(object sender, EventArgs e)
        => OptionsChanged?.Invoke(this, EventArgs.Empty);

    private void CmbGamePadIndex_SelectedIndexChanged(object sender, EventArgs e)
        => OptionsChanged?.Invoke(this, EventArgs.Empty);

    private void NudExactDelta_ValueChanged(object sender, EventArgs e)
        => OptionsChanged?.Invoke(this, EventArgs.Empty);

    private void NudTriggerInputScale_ValueChanged(object sender, EventArgs e)
        => OptionsChanged?.Invoke(this, EventArgs.Empty);
}
