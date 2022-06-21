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
    public partial class GamePadActionControl : UserControl
    {
        public GamePadActionControl()
        {
            InitializeComponent();

            var names = Enum.GetNames(typeof(SimWinInput.GamePadControl));

            cmbGamePad.DataSource = names;
        }
    }
}
