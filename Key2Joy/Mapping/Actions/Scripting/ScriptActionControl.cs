using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Key2Joy.Mapping
{
    public partial class ScriptActionControl : UserControl, IActionOptionsControl
    {
        public event Action OptionsChanged;

        private string languageName;


        public ScriptActionControl(string languageName)
        {
            this.languageName = languageName;
            InitializeComponent();

            pnlFileInput.Visible = !chkDirectInput.Checked;
            lblInfo.Text = $"{languageName} Script:";
        }

        public void Select(BaseAction action)
        {
            var thisAction = (BaseScriptAction)action;

            txtScript.Text = txtFilePath.Text = thisAction.Script;
            chkDirectInput.Checked = !thisAction.IsScriptPath;
            pnlFileInput.Visible = !chkDirectInput.Checked;
        }

        public void Setup(BaseAction action)
        {
            var thisAction = (BaseScriptAction)action;

            thisAction.IsScriptPath = !chkDirectInput.Checked;

            if (thisAction.IsScriptPath)
            {
                thisAction.Script = txtFilePath.Text;
                return;
            }
                
            thisAction.Script = txtScript.Text;
        }

        private void txtScript_TextChanged(object sender, EventArgs e)
        {
            OptionsChanged?.Invoke();
        }

        private void chkDirectInput_CheckedChanged(object sender, EventArgs e)
        {
            txtScript.Visible = chkDirectInput.Checked;
            pnlFileInput.Visible = !chkDirectInput.Checked;
            OptionsChanged?.Invoke();
            PerformLayout();
        }

        private void btnBrowseFile_Click(object sender, EventArgs e)
        {
            var filePicker = new OpenFileDialog();
            
            if (filePicker.ShowDialog() != DialogResult.OK)
                return;
            
            string file = filePicker.FileName;
            try
            {
                File.ReadAllText(file);
                txtFilePath.Text = file;
                OptionsChanged?.Invoke();
            }
            catch (IOException)
            {
                MessageBox.Show($"This file could not be loaded as a {languageName} script.", "Invalid script file!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
