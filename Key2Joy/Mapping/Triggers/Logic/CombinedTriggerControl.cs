using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Key2Joy.Mapping
{
    public partial class CombinedTriggerControl : UserControl
    {
        public event EventHandler RequestedRemove;
        public event EventHandler TriggerChanged;
        public BaseTrigger Trigger { get; private set; }

        public CombinedTriggerControl()
        {
            InitializeComponent();
        }

        public CombinedTriggerControl(BaseTrigger trigger)
            :this()
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
