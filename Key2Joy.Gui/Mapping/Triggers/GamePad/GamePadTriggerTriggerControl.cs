using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Triggers;
using Key2Joy.LowLevelInput.XInput;
using Key2Joy.Mapping;
using Key2Joy.Mapping.Triggers;
using Key2Joy.Mapping.Triggers.GamePad;

namespace Key2Joy.Gui.Mapping;

[MappingControl(
    ForType = typeof(GamePadTriggerTrigger),
    ImageResourceName = "joystick"
)]
public partial class GamePadTriggerTriggerControl : UserControl, ITriggerOptionsControl
{
    public event EventHandler OptionsChanged;

    public GamePadTriggerTriggerControl()
    {
        this.InitializeComponent();

        List<GamePadSide> sides = new();

        foreach (GamePadSide side in Enum.GetValues(typeof(GamePadSide)))
        {
            sides.Add(side);
        }

        this.cmbStickSide.DataSource = sides;
        this.nudGamePadIndex.Minimum = 0;
        this.nudGamePadIndex.Maximum = XInputService.MaxDevices - 1;

        this.nudDeadzone.Minimum = 0;
        this.nudDeadzone.Maximum = 1;

        this.UpdateDeadzoneEnabled();
    }

    public void Select(AbstractTrigger trigger)
    {
        var thisTrigger = (GamePadTriggerTrigger)trigger;

        this.nudGamePadIndex.Value = thisTrigger.GamePadIndex;
        this.cmbStickSide.SelectedItem = thisTrigger.TriggerSide;
        this.chkOverrideDeadzone.Checked = thisTrigger.DeltaMargin != null;
        this.nudDeadzone.Value = (decimal)(thisTrigger.DeltaMargin ?? 0);
    }

    public void Setup(AbstractTrigger trigger)
    {
        var thisTrigger = (GamePadTriggerTrigger)trigger;

        thisTrigger.GamePadIndex = (int)this.nudGamePadIndex.Value;
        thisTrigger.TriggerSide = (GamePadSide)this.cmbStickSide.SelectedItem;
        thisTrigger.DeltaMargin = this.chkOverrideDeadzone.Checked
            ? (float)this.nudDeadzone.Value
            : null;
    }

    private void CmbMouseDirection_SelectedIndexChanged(object sender, EventArgs e)
        => OptionsChanged?.Invoke(this, EventArgs.Empty);

    private void UpdateDeadzoneEnabled()
    {
        this.pnlDeadzoneConfig.Enabled = this.chkOverrideDeadzone.Enabled;

        foreach (Control control in this.pnlDeadzoneConfig.Controls)
        {
            control.Enabled = this.chkOverrideDeadzone.Checked;

            if (control is NumericUpDown numericUpDown)
            {
                numericUpDown.ReadOnly = !this.chkOverrideDeadzone.Checked;
            }
        }
    }

    private void ChkOverrideDeadzone_CheckedChanged(object sender, EventArgs e)
    {
        OptionsChanged?.Invoke(this, EventArgs.Empty);
        this.UpdateDeadzoneEnabled();
    }

    private void NudDeadzoneX_ValueChanged(object sender, EventArgs e)
        => OptionsChanged?.Invoke(this, EventArgs.Empty);

    private void NudDeadzoneY_ValueChanged(object sender, EventArgs e)
        => OptionsChanged?.Invoke(this, EventArgs.Empty);

    private void CmbStickSide_SelectedIndexChanged(object sender, EventArgs e)
        => OptionsChanged?.Invoke(this, EventArgs.Empty);

    private void NudGamePadIndex_ValueChanged(object sender, EventArgs e)
        => OptionsChanged?.Invoke(this, EventArgs.Empty);
}
