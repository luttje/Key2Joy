using SimWinInput;
using System;
using System.Windows.Forms;
using Linearstar.Windows.RawInput;
using System.Drawing;
using KeyToJoy.Input;
using System.Diagnostics;
using KeyToJoy.Mapping;

namespace KeyToJoy
{
    public partial class MainForm : Form, IAcceptAppCommands
    {
        private MappingPreset selectedPreset;

        private Image defaultControllerImage;
        private GlobalInputHook globalKeyboardHook;

        public MainForm()
        {
            InitializeComponent();

            defaultControllerImage = pctController.Image;

            RefreshInputCaptures();

            cmbPreset.DisplayMember = "Display";
            cmbPreset.DataSource = MappingPreset.All;
            
            ReloadSelectedPreset();
        }

        private void ReloadSelectedPreset()
        {
            selectedPreset = cmbPreset.SelectedItem as MappingPreset;
            dgvBinds.DataSource = selectedPreset.Bindings;
            txtPresetName.Text = selectedPreset.Name;
        }

        private void ChangeBinding(MappedOption bindingOption)
        {
            chkEnabled.Checked = false;

            new BindingForm(bindingOption).ShowDialog();
            RefreshInputCaptures();

            foreach (var option in selectedPreset.Bindings)
            {
                if (option == bindingOption)
                    continue;

                if (option.Binding == bindingOption.Binding)
                {
                    MessageBox.Show($"This binding is already in use for {option.Action}! Change {option.Action} to something else.");

                    selectedPreset.PruneCacheKey(option.Binding.GetUniqueBindingKey());
                    selectedPreset.CacheLookup(bindingOption);
                    dgvBinds.Update();

                    ChangeBinding(option);

                    return;
                }
            }

            selectedPreset.PruneCacheKey(bindingOption.Binding.GetUniqueBindingKey());
            selectedPreset.CacheLookup(bindingOption);
            dgvBinds.Update();
            selectedPreset.Save();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            dgvBinds.ClearSelection();
            pctController.Image = defaultControllerImage;
        }

        private void CmbPreset_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReloadSelectedPreset();
        }

        private void DgvBinds_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var row = dgvBinds.Rows[e.RowIndex];
            var bindingOption = row.DataBoundItem as MappedOption;

            if (bindingOption == null)
                return;

            ChangeBinding(bindingOption);
        }

        private void DgvBinds_SelectionChanged(object sender, EventArgs e)
        {
            var rowsCount = dgvBinds.SelectedRows.Count;

            if (rowsCount == 0 || rowsCount > 1) 
                return;

            var row = dgvBinds.SelectedRows[0];

            if (!row.Selected)
            {
                pctController.Image = defaultControllerImage;
                return;
            }

            var bindingOption = row.DataBoundItem as MappedOption;

            if (bindingOption == null)
                return;

            if(bindingOption.Action.Image != null)
                pctController.Image = bindingOption.Action.Image;
        }

        private void DgvBinds_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var bindingOption = dgvBinds.Rows[e.RowIndex].DataBoundItem as MappedOption;

            dgvBinds.Rows[e.RowIndex].Cells["colControl"].Value = bindingOption.GetActionDisplay();
            dgvBinds.Rows[e.RowIndex].Cells["colBind"].Value = bindingOption.GetBindDisplay();
        }

        private void tmrAxisTimeout_Tick(object sender, EventArgs e)
        {
            var controllerId = 0;
            var state = SimGamePad.Instance.State[controllerId];
            state.RightStickX = 0;
            state.RightStickY = 0;
            SimGamePad.Instance.Update(controllerId);
        }

        private void BtnOpenTest_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://devicetests.com/controller-tester");
        }

        private void ChkEnabled_CheckedChanged(object sender, EventArgs e)
        {
            bool isEnabled = chkEnabled.Checked;

            if(isEnabled)
                SimGamePad.Instance.PlugIn();
            else
                SimGamePad.Instance.Unplug();
        }

        private void TxtPresetName_TextChanged(object sender, EventArgs e)
        {
            selectedPreset.Name = txtPresetName.Text;
            selectedPreset.Save();
            MappingPreset.All.ResetBindings();
        }

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            var preset = new MappingPreset(txtPresetName.Text, selectedPreset.Bindings);

            MappingPreset.Add(preset);
            cmbPreset.SelectedIndex = cmbPreset.Items.Count - 1;
        }

        private void BtnAbout_Click(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog();
        }

        public bool RunAppCommand(string command)
        {
            switch (command)
            {
                case "abort":
                    chkEnabled.Checked = false;
                    return true;
                default:
                    return false;
            }
        }

        private void btnOpenFolder_Click(object sender, EventArgs e)
        {
            Process.Start(MappingPreset.GetSaveDirectory());
        }
    }
}
