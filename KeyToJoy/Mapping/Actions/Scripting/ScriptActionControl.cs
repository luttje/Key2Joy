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
    public partial class ScriptActionControl : UserControl, IActionOptionsControl
    {
        public event Action OptionsChanged;
        
        public ScriptActionControl(string languageName)
        {
            InitializeComponent();

            lblInfo.Text = $"{languageName} Script:";
        }

        public void Select(BaseAction action)
        {
            var thisAction = (BaseScriptAction)action;

            txtScript.Text = thisAction.Script;
        }

        public void Setup(BaseAction action)
        {
            var thisAction = (BaseScriptAction)action;

            thisAction.Script = txtScript.Text;
        }

        private void txtScript_TextChanged(object sender, EventArgs e)
        {
            OptionsChanged?.Invoke();
        }
    }
}
