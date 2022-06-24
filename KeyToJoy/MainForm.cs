using SimWinInput;
using System;
using System.Windows.Forms;
using Linearstar.Windows.RawInput;
using System.Drawing;
using KeyToJoy.Input;
using System.Diagnostics;
using KeyToJoy.Mapping;
using KeyToJoy.Properties;
using System.Collections.Generic;
using System.Linq;

namespace KeyToJoy
{
    public partial class MainForm : Form, IAcceptAppCommands
    {
        private MappingPreset selectedPreset;

        private Image defaultControllerImage;
        private List<TriggerListener> wndProcListeners = new List<TriggerListener>();

        public MainForm()
        {
            InitializeComponent();

            defaultControllerImage = pctController.Image;

            cmbPreset.DisplayMember = "Display";
            cmbPreset.DataSource = MappingPreset.All;
            
            ReloadSelectedPreset();
        }

        private void ReloadSelectedPreset()
        {
            selectedPreset = cmbPreset.SelectedItem as MappingPreset;

            if (selectedPreset == null)
                return;
            
            dgvMappings.DataSource = selectedPreset.MappedOptions;
            txtPresetName.Text = selectedPreset.Name;
        }

        private void btnAddAction_Click(object sender, EventArgs e)
        {
            if (selectedPreset == null)
            {
                CreateNewPreset();
            }

            EditMappedOption();
        }

        private void CreateNewPreset()
        {
            var preset = new MappingPreset(txtPresetName.Text, selectedPreset?.MappedOptions);

            MappingPreset.Add(preset);

            cmbPreset.SelectedIndex = cmbPreset.Items.Count - 1;
            ReloadSelectedPreset();
        }
        
        private void EditMappedOption(MappedOption existingMappedOption = null)
        {
            chkEnabled.Checked = false;
            var mappingForm = new MappingForm(existingMappedOption);
            var result = mappingForm.ShowDialog();

            if (result == DialogResult.Cancel)
                return;

            var mappedOption = mappingForm.MappedOption;
            bool createNewMapping = true;

            if(selectedPreset.TryGetMappedOption(mappedOption.Trigger, out var otherMappedOption))
            {
                if (existingMappedOption == null)
                {
                    MessageBox.Show($"This trigger is already mapped to {otherMappedOption.Action.GetNameDisplay()}. Remap that first!", "Trigger already in use.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (existingMappedOption.Equals(otherMappedOption))
                    createNewMapping = false;
            }

            if (createNewMapping)
                selectedPreset.AddMapping(mappedOption);

            dgvMappings.Update();
            selectedPreset.Save();
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

            EditMappedOption(mappedOption);
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

            if (!(row.DataBoundItem is MappedOption mappedOption))
                return;

            var image = mappedOption.Action.GetImage();
            pctController.Image = image ?? Resources.NoImage;
        }

        private void DgvMappings_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var mappedOption = dgvMappings.Rows[e.RowIndex].DataBoundItem as MappedOption;

            dgvMappings.Rows[e.RowIndex].Cells[colControl.Name].Value = mappedOption.GetActionDisplay();
            dgvMappings.Rows[e.RowIndex].Cells[colTrigger.Name].Value = mappedOption.GetTriggerDisplay();
        }

        private void BtnOpenTest_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://devicetests.com/controller-tester");
        }

        private void ChkEnabled_CheckedChanged(object sender, EventArgs e)
        {
            bool isEnabled = chkEnabled.Checked;

            if (isEnabled)
                ArmMappings();
            else
                DisarmMappings();
        }

        private void ArmMappings()
        {
            var listeners = new List<TriggerListener>();
            var allActions = selectedPreset.MappedOptions.Select(m => m.Action).ToList();

            foreach (var mappedOption in selectedPreset.MappedOptions)
            {
                var listener = mappedOption.Trigger.GetTriggerListener();
                
                if(!listeners.Contains(listener))
                    listeners.Add(listener);

                if (listener.HasWndProcHandle)
                {
                    listener.Handle = Handle;
                    wndProcListeners.Add(listener);
                }

                mappedOption.Action.OnStartListening(listener, ref allActions);
                listener.AddMappedOption(mappedOption);
            }

            foreach (var listener in listeners)
                listener.StartListening();
        }

        private void DisarmMappings()
        {
            var listeners = new List<TriggerListener>();
            wndProcListeners.Clear();

            foreach (var mappedOption in selectedPreset.MappedOptions)
            {
                var listener = mappedOption.Trigger.GetTriggerListener();
                mappedOption.Action.OnStopListening(listener);

                if (!listeners.Contains(listener))
                    listeners.Add(listener);
            }

            foreach (var listener in listeners)
                listener.StopListening();
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            foreach (var wndProcListener in wndProcListeners)
            {
                wndProcListener.WndProc(ref m);
            }
        }

        private void TxtPresetName_TextChanged(object sender, EventArgs e)
        {
            selectedPreset.Name = txtPresetName.Text;
            selectedPreset.Save();
            MappingPreset.All.ResetBindings();
        }

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            CreateNewPreset();
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
                    if (InvokeRequired)
                        Invoke(new Action(() => chkEnabled.Checked = false));
                    else
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
