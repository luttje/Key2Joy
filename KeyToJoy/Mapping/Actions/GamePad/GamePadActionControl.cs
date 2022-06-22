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
    public partial class GamePadActionControl : UserControl, ISelectAndSetupAction
    {
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
        }

        public void Setup(BaseAction action)
        {
            var thisAction = (GamePadAction)action;

            thisAction.Control = (SimWinInput.GamePadControl)cmbGamePad.SelectedItem;
        }
    }
}
