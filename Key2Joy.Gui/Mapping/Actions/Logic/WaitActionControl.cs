using System;
using System.Windows.Forms;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Mapping.Actions.Logic;

namespace Key2Joy.Gui.Mapping
{
    [MappingControl(
        ForType = typeof(WaitAction),
        ImageResourceName = "clock"
    )]
    public partial class WaitActionControl : UserControl, IActionOptionsControl
    {
        public event EventHandler OptionsChanged;

        public WaitActionControl()
        {
            this.InitializeComponent();

            this.nudWaitTimeInMs.Maximum = decimal.MaxValue;
        }

        public void Select(object action)
        {
            var thisAction = (WaitAction)action;

            this.nudWaitTimeInMs.Value = (decimal)thisAction.WaitTime.TotalMilliseconds;
        }

        public void Setup(object action)
        {
            var thisAction = (WaitAction)action;

            thisAction.WaitTime = TimeSpan.FromMilliseconds((double)this.nudWaitTimeInMs.Value);
        }

        public bool CanMappingSave(object action)
        {
            return true;
        }

        private void NudWaitTimeInMs_ValueChanged(object sender, EventArgs e)
        {
            OptionsChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
