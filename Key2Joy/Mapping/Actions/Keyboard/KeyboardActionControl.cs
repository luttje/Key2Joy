using Key2Joy.LowLevelInput;
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
    public partial class KeyboardActionControl : UserControl, IActionOptionsControl
    {
        public event Action OptionsChanged;
                
        public KeyboardActionControl()
        {
            InitializeComponent();

            cmbKeyboard.DataSource = KeyboardAction.GetAllKeys();
            cmbPressState.DataSource = Enum.GetValues(typeof(PressState));
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

        private void cmbKeyboard_SelectedIndexChanged(object sender, EventArgs e)
        {
            OptionsChanged?.Invoke();
        }

        private void cmbPressState_SelectedIndexChanged(object sender, EventArgs e)
        {
            OptionsChanged?.Invoke();
        }
    }
}
