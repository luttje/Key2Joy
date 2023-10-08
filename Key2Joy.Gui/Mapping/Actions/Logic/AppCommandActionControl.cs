using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Mapping.Actions.Logic;

namespace Key2Joy.Gui.Mapping
{
    [MappingControl(
        ForType = typeof(AppCommandAction),
        ImageResourceName = "application_xp_terminal"
    )]
    public partial class AppCommandActionControl : UserControl, IActionOptionsControl
    {
        public event EventHandler OptionsChanged;

        public AppCommandActionControl()
        {
            this.InitializeComponent();

            List<AppCommand> appCommands = new();

            foreach (AppCommand command in Enum.GetValues(typeof(AppCommand)))
            {
                appCommands.Add(command);
            }

            this.cmbAppCommand.DataSource = appCommands;
        }

        public void Select(object action)
        {
            var thisAction = (AppCommandAction)action;

            this.cmbAppCommand.SelectedItem = thisAction.Command;
        }

        public void Setup(object action)
        {
            var thisAction = (AppCommandAction)action;

            thisAction.Command = (AppCommand)this.cmbAppCommand.SelectedItem;
        }
        public bool CanMappingSave(object action)
        {
            return true;
        }

        private void CmbAppCommand_SelectedIndexChanged(object sender, EventArgs e)
        {
            OptionsChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
