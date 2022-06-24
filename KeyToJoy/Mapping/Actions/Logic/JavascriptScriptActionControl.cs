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
    public partial class JavascriptActionControl : UserControl, IActionOptionsControl
    {
        public event Action OptionsChanged;
        
        public JavascriptActionControl()
        {
            InitializeComponent();
        }

        public void Select(BaseAction action)
        {
            var thisAction = (JavascriptAction)action;

            txtScript.Text = thisAction.Script;
        }

        public void Setup(BaseAction action)
        {
            var thisAction = (JavascriptAction)action;

            thisAction.Script = txtScript.Text;
        }

        private void txtScript_TextChanged(object sender, EventArgs e)
        {
            OptionsChanged?.Invoke();
        }
    }
}
