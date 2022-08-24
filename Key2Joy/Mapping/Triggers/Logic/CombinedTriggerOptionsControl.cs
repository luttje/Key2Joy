using Key2Joy.Config;
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
    public partial class CombinedTriggerOptionsControl : UserControl, ITriggerOptionsControl
    {
        public event EventHandler OptionsChanged;

        public CombinedTriggerOptionsControl()
        {
            InitializeComponent();

            nudTimeout.Value = ConfigManager.Instance.DefaultCombinedTriggerTimeout;
        }

        private CombinedTriggerControl AddTriggerControl(BaseTrigger trigger = null)
        {
            var triggerControl = new CombinedTriggerControl(trigger);
            triggerControl.AutoSize = true;
            triggerControl.Dock = DockStyle.Top;
            triggerControl.RequestedRemove += (s, _) =>
            {
                pnlTriggers.Controls.Remove(s as CombinedTriggerControl);
                PerformLayout();
            };
            triggerControl.TriggerChanged += (s, _) => this.OptionsChanged?.Invoke(this, EventArgs.Empty);
            pnlTriggers.Controls.Add(triggerControl);
            PerformLayout();

            return triggerControl;
        }

        private void btnAddTrigger_Click(object sender, EventArgs e)
        {
            AddTriggerControl().BringToFront();
        }

        private void nudTimeout_ValueChanged(object sender, EventArgs e)
        {
            OptionsChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Select(BaseTrigger combinedTrigger)
        {
            var thisTrigger = (CombinedTrigger)combinedTrigger;

            nudTimeout.Value = thisTrigger.Timeout;

            if(thisTrigger.Triggers != null)
            {
                foreach(var trigger in thisTrigger.Triggers)
                {
                    AddTriggerControl(trigger);
                }
            }
        }

        public void Setup(BaseTrigger trigger)
        {
            var thisTrigger = (CombinedTrigger)trigger;

            thisTrigger.Timeout = (int) nudTimeout.Value;
            thisTrigger.Triggers = new List<BaseTrigger>();

            foreach (var triggerControl in pnlTriggers.Controls)
            {
                thisTrigger.Triggers.Add((triggerControl as CombinedTriggerControl).Trigger);
            }
        }
    }
}
