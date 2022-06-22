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

            var controls = Enum.GetValues(typeof(SimWinInput.GamePadControl));

            cmbGamePad.DataSource = controls;
        }

        public void Select(BaseAction action)
        {
            var thisAction = (GamePadAction)action;

            cmbGamePad.SelectedItem = thisAction.Control;
            chkDown.Checked = thisAction.PressDown;
        }

        public void Setup(BaseAction action)
        {
            var thisAction = (GamePadAction)action;

            thisAction.Control = (SimWinInput.GamePadControl)cmbGamePad.SelectedItem;
            thisAction.PressDown = chkDown.Checked;
        }

        private void cmbGamePad_SelectedIndexChanged(object sender, EventArgs e)
        {
            OptionsChanged?.Invoke();
        }

        private void chkDown_CheckedChanged(object sender, EventArgs e)
        {
            OptionsChanged?.Invoke();
        }
    }
}
