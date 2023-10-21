using System;
using System.Windows.Forms;
using Key2Joy.Contracts.Mapping.Triggers;

namespace Key2Joy.Gui.Mapping;

public partial class CombinedTriggerControlItem : UserControl
{
    public event EventHandler RequestedRemove;

    public event EventHandler TriggerChanged;

    public AbstractTrigger Trigger { get; private set; }

    public CombinedTriggerControlItem() => this.InitializeComponent();

    public CombinedTriggerControlItem(AbstractTrigger trigger)
        : this()
    {
        this.Trigger = trigger;

        this.triggerControl.SelectTrigger(trigger);
    }

    private void BtnRemove_Click(object sender, EventArgs e)
        => RequestedRemove?.Invoke(this, EventArgs.Empty);

    private void TriggerControl_TriggerChanged(object sender, TriggerChangedEventArgs e)
    {
        this.Trigger = e.Trigger;

        TriggerChanged?.Invoke(this, EventArgs.Empty);
    }
}
