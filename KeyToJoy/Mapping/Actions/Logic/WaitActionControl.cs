using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyToJoy.Mapping
{
    public partial class WaitActionControl : UserControl
    {
        public WaitActionControl()
        {
            InitializeComponent();
        }

        private void txtKeyBind_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                SystemSounds.Hand.Play();
            }
        }
    }
}
