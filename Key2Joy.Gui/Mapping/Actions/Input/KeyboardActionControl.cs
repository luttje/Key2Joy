using Key2Joy.Contracts.Mapping;
using Key2Joy.LowLevelInput;
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
        ForType = typeof(Key2Joy.Mapping.KeyboardAction),
        ImageResourceName = "keyboard"
    )]
    public partial class KeyboardActionControl : UserControl, IActionOptionsControl
    {
        public event EventHandler OptionsChanged;
                
        public KeyboardActionControl()
        {
            InitializeComponent();

            cmbKeyboard.DataSource = KeyboardAction.GetAllKeys();
            cmbPressState.DataSource = PressStates.ALL;
            cmbPressState.SelectedIndex = 0;
        }

        public void Select(object action)
        {
            var thisAction = (KeyboardAction)action;

            cmbKeyboard.SelectedItem = thisAction.Key;
            cmbPressState.SelectedItem = thisAction.PressState;
        }

        public void Setup(object action)
        {
            var thisAction = (KeyboardAction)action;

            thisAction.Key = (KeyboardKey) cmbKeyboard.SelectedItem;
            thisAction.PressState = (PressState) cmbPressState.SelectedItem;
        }

        public bool CanMappingSave(object action)
        {
            return true;
        }

        private void cmbKeyboard_SelectedIndexChanged(object sender, EventArgs e)
        {
            OptionsChanged?.Invoke(this, EventArgs.Empty);
        }

        private void cmbPressState_SelectedIndexChanged(object sender, EventArgs e)
        {
            OptionsChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
