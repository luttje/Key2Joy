using KeyToJoy.Input;
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
    public partial class GamePadActionControl : UserControl, IActionOptionsControl
    {
        public event Action OptionsChanged;
        
        public GamePadActionControl()
        {
            InitializeComponent();

            cmbGamePad.DataSource = GamePadAction.GetAllButtons();
            cmbPressState.DataSource = Enum.GetValues(typeof(PressState));
            cmbPressState.SelectedIndex = 0;
        }

        public void Select(BaseAction action)
        {
            var thisAction = (GamePadAction)action;

            cmbGamePad.SelectedItem = thisAction.Control;
            cmbPressState.SelectedItem = thisAction.PressState;
        }

        public void Setup(BaseAction action)
        {
            var thisAction = (GamePadAction)action;

            thisAction.Control = (SimWinInput.GamePadControl)cmbGamePad.SelectedItem;
            thisAction.PressState = (PressState) cmbPressState.SelectedItem;
        }

        private void cmbGamePad_SelectedIndexChanged(object sender, EventArgs e)
        {
            OptionsChanged?.Invoke();
        }

        private void cmbPressState_SelectedIndexChanged(object sender, EventArgs e)
        {
            OptionsChanged?.Invoke();
        }
    }
}
