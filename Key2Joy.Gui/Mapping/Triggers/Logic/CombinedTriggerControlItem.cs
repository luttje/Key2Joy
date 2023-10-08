using Key2Joy.Contracts.Mapping;
using System;
using System.Windows.Forms;

namespace Key2Joy.Gui.Mapping
{
    public partial class CombinedTriggerControlItem : UserControl
    {
        public event EventHandler RequestedRemove;
        public event EventHandler TriggerChanged;
        public AbstractTrigger Trigger { get; private set; }

        public CombinedTriggerControlItem()
        {
            InitializeComponent();
        }

        public CombinedTriggerControlItem(AbstractTrigger trigger)
            : this()
        {
            Trigger = trigger;

            triggerControl.SelectTrigger(trigger);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            RequestedRemove?.Invoke(this, EventArgs.Empty);
        }

        private void triggerControl_TriggerChanged(object sender, TriggerChangedEventArgs e)
        {
            Trigger = e.Trigger;

            TriggerChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
