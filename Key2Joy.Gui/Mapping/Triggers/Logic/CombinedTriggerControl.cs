using Key2Joy.Contracts.Mapping;
using Key2Joy.Config;
using Key2Joy.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Key2Joy.Gui.Mapping
{
    [MappingControl(
        ForType = typeof(Key2Joy.Mapping.CombinedTrigger),
        ImageResourceName = "link"
    )]
    public partial class CombinedTriggerControl : UserControl, ITriggerOptionsControl
    {
        public event EventHandler OptionsChanged;

        public CombinedTriggerControl()
        {
            InitializeComponent();
        }

        private CombinedTriggerControlItem AddTriggerControl(AbstractTrigger trigger = null)
        {
            var triggerControl = new CombinedTriggerControlItem(trigger);
            triggerControl.AutoSize = true;
            triggerControl.Dock = DockStyle.Top;
            triggerControl.RequestedRemove += (s, _) =>
            {
                var control = s as CombinedTriggerControlItem;
                pnlTriggers.Controls.Remove(control);
                control.Dispose();
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

        public void Select(AbstractTrigger combinedTrigger)
        {
            var thisTrigger = (CombinedTrigger)combinedTrigger;

            if(thisTrigger.Triggers != null)
            {
                foreach(var trigger in thisTrigger.Triggers)
                {
                    AddTriggerControl(trigger);
                }
            }
        }

        public void Setup(AbstractTrigger trigger)
        {
            var thisTrigger = (CombinedTrigger)trigger;

            thisTrigger.Triggers = new List<AbstractTrigger>();

            foreach (var triggerControl in pnlTriggers.Controls)
            {
                thisTrigger.Triggers.Add((triggerControl as CombinedTriggerControlItem).Trigger);
            }
        }
    }
}
