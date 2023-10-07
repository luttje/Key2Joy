using Key2Joy.Contracts.Mapping;
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
        ForType = typeof(Key2Joy.Mapping.WaitAction),
        ImageResourceName = "clock"
    )]
    public partial class WaitActionControl : UserControl, IActionOptionsControl
    {
        public event EventHandler OptionsChanged;
        
        public WaitActionControl()
        {
            InitializeComponent();

            nudWaitTimeInMs.Maximum = decimal.MaxValue;
        }

        public void Select(object action)
        {
            var thisAction = (WaitAction)action;

            nudWaitTimeInMs.Value = (decimal)thisAction.WaitTime.TotalMilliseconds;
        }

        public void Setup(object action)
        { 
            var thisAction = (WaitAction)action;

            thisAction.WaitTime = TimeSpan.FromMilliseconds((double)nudWaitTimeInMs.Value);
        }
        
        public bool CanMappingSave(object action)
        {
            return true;
        }

        private void nudWaitTimeInMs_ValueChanged(object sender, EventArgs e)
        {
            OptionsChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
