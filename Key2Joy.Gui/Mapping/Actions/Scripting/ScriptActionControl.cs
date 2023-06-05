﻿using Key2Joy.Contracts.Mapping;
using Key2Joy.Mapping;
using System;
using System.IO;
using System.Windows.Forms;

namespace Key2Joy.Gui.Mapping
{
    [MappingControl(
        ForTypes = new[]
        {
            typeof(Key2Joy.Mapping.JavascriptAction),
            typeof(Key2Joy.Mapping.LuaScriptAction),
        },
        ImageResourceName = "script_code"
    )]
    public partial class ScriptActionControl : UserControl, IActionOptionsControl
    {
        public event EventHandler OptionsChanged;

        public ScriptActionControl()
        {
            InitializeComponent();

            pnlFileInput.Visible = !chkDirectInput.Checked;
        }

        public void Select(AbstractAction action)
        {
            var thisAction = (BaseScriptAction)action;

            var mappingType = action.GetType();
            string languageName;

            // If it's LuaScriptAction or JavascriptAction change the typename to ScriptAction
            if (mappingType == typeof(LuaScriptAction))
            {
                languageName = "Lua";
            } else if (mappingType == typeof(JavascriptAction))
            {
                languageName = "Javascript";
            } else
            {
                languageName = "Unknown Language";
            }

            lblInfo.Text = $"{languageName} Script:";

            txtScript.Text = txtFilePath.Text = thisAction.Script;
            chkDirectInput.Checked = !thisAction.IsScriptPath;
            pnlFileInput.Visible = !chkDirectInput.Checked;
        }

        public void Setup(AbstractAction action)
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

        public bool CanMappingSave(AbstractAction action)
        {
            return MessageBox.Show(
                "Scripts can click and type like you do and therefor impersonate you. "
                + "Scripts could cause harm to your pc, you or else. "
                + "For that reason you should only run scripts that you trust!"
                + "\n\nDo you trust this script?",
                "Warning:! Scripts can be dangerous!",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Warning) == DialogResult.Yes;
        }

        private void txtScript_TextChanged(object sender, EventArgs e)
        {
            OptionsChanged?.Invoke(this, EventArgs.Empty);
        }

        private void txtFilePath_TextChanged(object sender, EventArgs e)
        {
            OptionsChanged?.Invoke(this, EventArgs.Empty);
        }

        private void chkDirectInput_CheckedChanged(object sender, EventArgs e)
        {
            txtScript.Visible = chkDirectInput.Checked;
            pnlFileInput.Visible = !chkDirectInput.Checked;
            OptionsChanged?.Invoke(this, EventArgs.Empty);
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
                OptionsChanged?.Invoke(this, EventArgs.Empty);
            }
            catch (IOException)
            {
                MessageBox.Show($"This file could not be loaded as a script.", "Invalid script file!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
