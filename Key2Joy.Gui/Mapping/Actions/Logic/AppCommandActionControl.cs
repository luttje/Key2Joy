using Key2Joy.Contracts.Mapping;
using Key2Joy.Mapping;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Key2Joy.Gui.Mapping
{
    [MappingControl(
        ForType = typeof(Key2Joy.Mapping.AppCommandAction),
        ImageResourceName = "application_xp_terminal"
    )]
    public partial class AppCommandActionControl : UserControl, IActionOptionsControl
    {
        public event EventHandler OptionsChanged;

        public AppCommandActionControl()
        {
            InitializeComponent();

            List<AppCommand> appCommands = new();

            foreach (AppCommand command in Enum.GetValues(typeof(AppCommand)))
            {
                appCommands.Add(command);
            }

            cmbAppCommand.DataSource = appCommands;
        }

        public void Select(object action)
        {
            var thisAction = (AppCommandAction)action;

            cmbAppCommand.SelectedItem = thisAction.Command;
        }

        public void Setup(object action)
        {
            var thisAction = (AppCommandAction)action;

            thisAction.Command = (AppCommand)cmbAppCommand.SelectedItem;
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
