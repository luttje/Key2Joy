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
    public partial class SetTimeoutActionControl : UserControl, IActionOptionsControl
    {
        public event Action OptionsChanged;
        
        public SetTimeoutActionControl()
        {
            InitializeComponent();
        }

        public void Select(BaseAction action)
        {
            var thisAction = (SetTimeoutAction)action;

            nudWaitTime.Value = (decimal)thisAction.WaitTime.TotalMilliseconds;
        }

        public void Setup(BaseAction action)
        {
            var thisAction = (SetTimeoutAction)action;

            thisAction.WaitTime = TimeSpan.FromMilliseconds((double)nudWaitTime.Value);
        }

        private void txtKeyBind_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                SystemSounds.Hand.Play();
                return;
            }
        }

        private void nudWaitTime_ValueChanged(object sender, EventArgs e)
        {
            OptionsChanged?.Invoke();
        }
    }
}
