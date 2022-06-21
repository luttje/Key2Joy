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
            dgvMappings.DataSource = selectedPreset.MappedOptions;
            txtPresetName.Text = selectedPreset.Name;
        }

        private void ChangeMappedOption(MappedOption mappedOption)
        {
            chkEnabled.Checked = false;

            // TODO: Open MappingForm relevant for this option.
            //new InputMappingForm(mappedOption).ShowDialog();
            //RefreshInputCaptures();

            //foreach (var option in selectedPreset.MappedOptions)
            //{
            //    if (option == mappedOption)
            //        continue;

            //    if (option.Trigger == mappedOption.Trigger)
            //    {
            //        MessageBox.Show($"This trigger is already in use for {option.Action}! Change {option.Action} to something else.");

            //        selectedPreset.PruneCacheKey(option.Trigger.GetUniqueKey());
            //        selectedPreset.CacheLookup(mappedOption);
            //        dgvMappings.Update();

            //        ChangeMappedOption(option);

            //        return;
            //    }
            //}

            //selectedPreset.PruneCacheKey(mappedOption.Trigger.GetUniqueKey());
            //selectedPreset.CacheLookup(mappedOption);
            //dgvMappings.Update();
            //selectedPreset.Save();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            dgvMappings.ClearSelection();
            pctController.Image = defaultControllerImage;
        }

        private void CmbPreset_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReloadSelectedPreset();
        }

        private void DgvMappings_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var row = dgvMappings.Rows[e.RowIndex];
            var mappedOption = row.DataBoundItem as MappedOption;

            if (mappedOption == null)
                return;

            ChangeMappedOption(mappedOption);
        }

        private void DgvMappings_SelectionChanged(object sender, EventArgs e)
        {
            var rowsCount = dgvMappings.SelectedRows.Count;

            if (rowsCount == 0 || rowsCount > 1) 
                return;

            var row = dgvMappings.SelectedRows[0];

            if (!row.Selected)
            {
                pctController.Image = defaultControllerImage;
                return;
            }

            var mappedOption = row.DataBoundItem as MappedOption;

            if (mappedOption == null)
                return;

            var image = mappedOption.Action.GetImage();
            if (image != null)
                pctController.Image = image;
        }

        private void DgvMappings_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var mappedOption = dgvMappings.Rows[e.RowIndex].DataBoundItem as MappedOption;

            dgvMappings.Rows[e.RowIndex].Cells[colContext.Name].Value = mappedOption.GetContextDisplay();
            dgvMappings.Rows[e.RowIndex].Cells[colControl.Name].Value = mappedOption.GetActionDisplay();
            dgvMappings.Rows[e.RowIndex].Cells[colTrigger.Name].Value = mappedOption.GetTriggerDisplay();
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
            var preset = new MappingPreset(txtPresetName.Text, selectedPreset.MappedOptions);

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

        private void btnAddAction_Click(object sender, EventArgs e)
        {
            (new MappingForm()).ShowDialog();
        }
    }
}
