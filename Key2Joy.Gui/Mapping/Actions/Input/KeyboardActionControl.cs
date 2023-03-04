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
    public partial class KeyboardActionControl : UserControl, IActionOptionsControl
    {
        public event EventHandler OptionsChanged;
                
        public KeyboardActionControl()
        {
            InitializeComponent();

            cmbKeyboard.DataSource = KeyboardAction.GetAllKeys();
            cmbPressState.DataSource = LegacyPressStateConverter.GetPressStatesWithoutLegacy();
            cmbPressState.SelectedIndex = 0;
        }

        public void Select(BaseAction action)
        {
            var thisAction = (KeyboardAction)action;

            cmbKeyboard.SelectedItem = thisAction.Key;
            cmbPressState.SelectedItem = thisAction.PressState;
        }

        public void Setup(BaseAction action)
        {
            var thisAction = (KeyboardAction)action;

            thisAction.Key = (KeyboardKey) cmbKeyboard.SelectedItem;
            thisAction.PressState = (PressState) cmbPressState.SelectedItem;
        }

        public bool CanMappingSave(BaseAction action)
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
