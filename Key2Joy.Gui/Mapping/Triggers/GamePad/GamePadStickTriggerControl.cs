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
    ForType = typeof(GamePadStickTrigger),
    ImageResourceName = "joystick"
)]
public partial class GamePadStickTriggerControl : UserControl, ITriggerOptionsControl
{
    public event EventHandler OptionsChanged;

    public GamePadStickTriggerControl()
    {
        this.InitializeComponent();

        List<GamePadStickSide> sides = new();

        foreach (GamePadStickSide side in Enum.GetValues(typeof(GamePadStickSide)))
        {
            sides.Add(side);
        }

        this.cmbStickSide.DataSource = sides;
        this.nudGamePadIndex.Minimum = 0;
        this.nudGamePadIndex.Maximum = XInputService.MaxDevices - 1;

        this.nudDeadzoneX.Minimum = this.nudDeadzoneY.Minimum = -1;
        this.nudDeadzoneX.Maximum = this.nudDeadzoneY.Maximum = 1;

        this.UpdateDeadzoneEnabled();
    }

    public void Select(AbstractTrigger trigger)
    {
        var thisTrigger = (GamePadStickTrigger)trigger;

        this.nudGamePadIndex.Value = thisTrigger.GamePadIndex;
        this.cmbStickSide.SelectedItem = thisTrigger.StickSide;
        this.chkOverrideDeadzone.Checked = thisTrigger.DeltaMargin != null;
        this.nudDeadzoneX.Value = (decimal)(thisTrigger.DeltaMargin?.X ?? 0);
        this.nudDeadzoneY.Value = (decimal)(thisTrigger.DeltaMargin?.Y ?? 0);
    }

    public void Setup(AbstractTrigger trigger)
    {
        var thisTrigger = (GamePadStickTrigger)trigger;

        thisTrigger.GamePadIndex = (int)this.nudGamePadIndex.Value;
        thisTrigger.StickSide = (GamePadStickSide)this.cmbStickSide.SelectedItem;
        thisTrigger.DeltaMargin = this.chkOverrideDeadzone.Checked
            ? new ExactAxisDirection(
                (float)this.nudDeadzoneX.Value,
                (float)this.nudDeadzoneY.Value
            )
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

    private void chkOverrideDeadzone_CheckedChanged(object sender, EventArgs e)
    {
        OptionsChanged?.Invoke(this, EventArgs.Empty);
        this.UpdateDeadzoneEnabled();
    }

    private void nudDeadzoneX_ValueChanged(object sender, EventArgs e)
        => OptionsChanged?.Invoke(this, EventArgs.Empty);

    private void nudDeadzoneY_ValueChanged(object sender, EventArgs e)
        => OptionsChanged?.Invoke(this, EventArgs.Empty);

    private void cmbStickSide_SelectedIndexChanged(object sender, EventArgs e)
        => OptionsChanged?.Invoke(this, EventArgs.Empty);

    private void nudGamePadIndex_ValueChanged(object sender, EventArgs e)
        => OptionsChanged?.Invoke(this, EventArgs.Empty);
}
