using System;
using System.IO;
using System.Windows.Forms;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Mapping.Actions.Scripting;

namespace Key2Joy.Gui.Mapping;

[MappingControl(
    ForTypes = new[]
    {
        typeof(JavascriptAction),
        typeof(LuaScriptAction),
    },
    ImageResourceName = "script_code"
)]
public partial class ScriptActionControl : UserControl, IActionOptionsControl
{
    public event EventHandler OptionsChanged;

    public ScriptActionControl()
    {
        this.InitializeComponent();

        this.pnlFileInput.Visible = !this.chkDirectInput.Checked;
    }

    public void Select(object action)
    {
        var thisAction = (BaseScriptAction)action;

        var mappingType = action.GetType();
        string languageName;

        // If it's LuaScriptAction or JavascriptAction change the typename to ScriptAction
        if (mappingType == typeof(LuaScriptAction))
        {
            languageName = "Lua";
        }
        else if (mappingType == typeof(JavascriptAction))
        {
            languageName = "Javascript";
        }
        else
        {
            languageName = "Unknown Language";
        }

        this.lblInfo.Text = $"{languageName} Script:";

        this.txtScript.Text = this.txtFilePath.Text = thisAction.Script;
        this.chkDirectInput.Checked = !thisAction.IsScriptPath;
        this.pnlFileInput.Visible = !this.chkDirectInput.Checked;
    }

    public void Setup(object action)
    {
        var thisAction = (BaseScriptAction)action;

        thisAction.IsScriptPath = !this.chkDirectInput.Checked;

        if (thisAction.IsScriptPath)
        {
            thisAction.Script = this.txtFilePath.Text;
            return;
        }

        thisAction.Script = this.txtScript.Text;
    }

    public bool CanMappingSave(object action) => MessageBox.Show(
            "Scripts can click and type like you do and therefor impersonate you. "
            + "Scripts could cause harm to your pc, you or else. "
            + "For that reason you should only run scripts that you trust!"
            + "\n\nDo you trust this script?",
            "Warning:! Scripts can be dangerous!",
            MessageBoxButtons.YesNoCancel,
            MessageBoxIcon.Warning) == DialogResult.Yes;

    private void TxtScript_TextChanged(object sender, EventArgs e) => OptionsChanged?.Invoke(this, EventArgs.Empty);

    private void TxtFilePath_TextChanged(object sender, EventArgs e) => OptionsChanged?.Invoke(this, EventArgs.Empty);

    private void ChkDirectInput_CheckedChanged(object sender, EventArgs e)
    {
        this.txtScript.Visible = this.chkDirectInput.Checked;
        this.pnlFileInput.Visible = !this.chkDirectInput.Checked;
        OptionsChanged?.Invoke(this, EventArgs.Empty);
        this.PerformLayout();
    }

    private void BtnBrowseFile_Click(object sender, EventArgs e)
    {
        OpenFileDialog filePicker = new();

        if (filePicker.ShowDialog() != DialogResult.OK)
        {
            return;
        }

        var file = filePicker.FileName;
        try
        {
            File.ReadAllText(file);
            this.txtFilePath.Text = file;
            OptionsChanged?.Invoke(this, EventArgs.Empty);
        }
        catch (IOException)
        {
            MessageBox.Show($"This file could not be loaded as a script.", "Invalid script file!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
