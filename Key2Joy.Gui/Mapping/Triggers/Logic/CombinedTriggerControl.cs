using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Triggers;
using Key2Joy.Mapping.Triggers;
using Key2Joy.Mapping.Triggers.Logic;

namespace Key2Joy.Gui.Mapping;

[MappingControl(
    ForType = typeof(CombinedTrigger),
    ImageResourceName = "link"
)]
public partial class CombinedTriggerControl : UserControl, ITriggerOptionsControl
{
    public event EventHandler OptionsChanged;

    public CombinedTriggerControl()
        => this.InitializeComponent();

    private CombinedTriggerControlItem AddTriggerControl(AbstractTrigger trigger = null)
    {
        CombinedTriggerControlItem triggerControl = new(trigger)
        {
            AutoSize = true,
            Dock = DockStyle.Top
        };
        triggerControl.RequestedRemove += (s, _) =>
        {
            var control = s as CombinedTriggerControlItem;
            this.pnlTriggers.Controls.Remove(control);
            control.Dispose();
            this.PerformLayout();
            this.OptionsChanged?.Invoke(this, EventArgs.Empty);
        };
        triggerControl.TriggerChanged += (s, _)
            => this.OptionsChanged?.Invoke(this, EventArgs.Empty);
        this.pnlTriggers.Controls.Add(triggerControl);
        this.PerformLayout();

        return triggerControl;
    }

    private void BtnAddTrigger_Click(object sender, EventArgs e)
        => this.AddTriggerControl().BringToFront();

    public void Select(AbstractTrigger combinedTrigger)
    {
        var thisTrigger = (CombinedTrigger)combinedTrigger;

        if (thisTrigger.Triggers != null)
        {
            foreach (var trigger in thisTrigger.Triggers)
            {
                this.AddTriggerControl(trigger);
            }
        }
    }

    public void Setup(AbstractTrigger trigger)
    {
        var thisTrigger = (CombinedTrigger)trigger;

        thisTrigger.Triggers = new List<AbstractTrigger>();

        foreach (var triggerControl in this.pnlTriggers.Controls)
        {
            thisTrigger.Triggers.Add((triggerControl as CombinedTriggerControlItem).Trigger);
        }
    }

    public bool CanMappingSave(AbstractTrigger trigger) => true;
}
