using System;
using System.Windows.Forms;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.LowLevelInput;
using Key2Joy.Mapping.Actions.Input;

namespace Key2Joy.Gui.Mapping
{
    [MappingControl(
        ForType = typeof(KeyboardAction),
        ImageResourceName = "keyboard"
    )]
    public partial class KeyboardActionControl : UserControl, IActionOptionsControl
    {
        public event EventHandler OptionsChanged;

        public KeyboardActionControl()
        {
            this.InitializeComponent();

            this.cmbKeyboard.DataSource = KeyboardAction.GetAllKeys();
            this.cmbPressState.DataSource = PressStates.ALL;
            this.cmbPressState.SelectedIndex = 0;
        }

        public void Select(object action)
        {
            var thisAction = (KeyboardAction)action;

            this.cmbKeyboard.SelectedItem = thisAction.Key;
            this.cmbPressState.SelectedItem = thisAction.PressState;
        }

        public void Setup(object action)
        {
            var thisAction = (KeyboardAction)action;

            thisAction.Key = (KeyboardKey)this.cmbKeyboard.SelectedItem;
            thisAction.PressState = (PressState)this.cmbPressState.SelectedItem;
        }

        public bool CanMappingSave(object action)
        {
            return true;
        }

        private void CmbKeyboard_SelectedIndexChanged(object sender, EventArgs e)
        {
            OptionsChanged?.Invoke(this, EventArgs.Empty);
        }

        private void CmbPressState_SelectedIndexChanged(object sender, EventArgs e)
        {
            OptionsChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
