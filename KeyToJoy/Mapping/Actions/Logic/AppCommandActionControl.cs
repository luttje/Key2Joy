using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyToJoy.Mapping
{
    public partial class AppCommandActionControl : UserControl, ISelectAndSetupAction
    {
        private Dictionary<string,string> appCommands;
        
        public AppCommandActionControl()
        {
            InitializeComponent();

            appCommands = new Dictionary<string, string>();
            appCommands.Add("abort", "abort - Stops mapping keys to actions.");

            cmbAppCommand.DataSource = new BindingSource(appCommands, null);
            cmbAppCommand.DisplayMember = "Value";
            cmbAppCommand.ValueMember = "Key";
        }

        public void Select(BaseAction action)
        {
            var thisAction = (AppCommandAction)action;

            cmbAppCommand.SelectedValue = thisAction.Command;
        }

        public void Setup(BaseAction action)
        {
            var thisAction = (AppCommandAction)action;

            thisAction.Command = cmbAppCommand.SelectedValue.ToString();
        }
    }
}
