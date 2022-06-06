using SimWinInput;
using System;
using System.Windows.Forms;
using Linearstar.Windows.RawInput;
using System.Drawing;
using KeyToJoy.Input;

namespace KeyToJoy
{
    public partial class MainForm : Form
    {
        private BindingPreset selectedPreset;

        private Image defaultControllerImage;
        private GlobalInputHook globalKeyboardHook;

        public MainForm()
        {
            InitializeComponent();

            defaultControllerImage = pctController.Image;

            Init();

            cmbPreset.DisplayMember = "Display";
            cmbPreset.DataSource = BindingPreset.All;
            
            ReloadSelectedPreset();
        }

        private void ReloadSelectedPreset()
        {
            selectedPreset = cmbPreset.SelectedItem as BindingPreset;
            dgvBinds.DataSource = selectedPreset.Bindings;
            txtPresetName.Text = selectedPreset.Name;
        }

        private void ChangeBinding(BindingOption bindingOption)
        {
            chkEnabled.Checked = false;

            new BindingForm(bindingOption).ShowDialog();

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
            var bindingOption = row.DataBoundItem as BindingOption;

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

            var bindingOption = row.DataBoundItem as BindingOption;

            if (bindingOption == null)
                return;

            if(bindingOption.Action.Image != null)
                pctController.Image = bindingOption.Action.Image;
        }

        private void DgvBinds_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var bindingOption = dgvBinds.Rows[e.RowIndex].DataBoundItem as BindingOption;

            dgvBinds.Rows[e.RowIndex].Cells["colControl"].Value = bindingOption.GetActionDisplay();
            dgvBinds.Rows[e.RowIndex].Cells["colBind"].Value = bindingOption.GetBindDisplay();
        }

        private void TimerAxisTimeout_Tick(object sender, EventArgs e)
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
            BindingPreset.All.ResetBindings();
        }

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            var preset = new BindingPreset(txtPresetName.Text, selectedPreset.Bindings);

            BindingPreset.Add(preset);
            cmbPreset.SelectedIndex = cmbPreset.Items.Count - 1;
        }

        private void BtnAbout_Click(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog();
        }
    }
}
