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
    public partial class AppCommandActionControl : UserControl, IActionOptionsControl
    {
        public event Action OptionsChanged;
        
        public AppCommandActionControl()
        {
            InitializeComponent();

            var appCommands = new List<AppCommand>();

            foreach (AppCommand command in Enum.GetValues(typeof(AppCommand)))
            {
                appCommands.Add(command);
            }

            cmbAppCommand.DataSource = appCommands;
        }

        public void Select(BaseAction action)
        {
            var thisAction = (AppCommandAction)action;

            cmbAppCommand.SelectedItem = thisAction.Command;
        }

        public void Setup(BaseAction action)
        { 
            var thisAction = (AppCommandAction)action;

            thisAction.Command = (AppCommand)cmbAppCommand.SelectedItem;
        }

        private void cmbAppCommand_SelectedIndexChanged(object sender, EventArgs e)
        {
            OptionsChanged?.Invoke();
        }
    }
}
