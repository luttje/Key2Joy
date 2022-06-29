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
using KeyToJoy.Util;
using System.Reflection;
using System.Threading.Tasks;
using System.IO;
using BrightIdeasSoftware;

namespace KeyToJoy
{
    public partial class MainForm : Form, IAcceptAppCommands
    {
        private MappingPreset selectedPreset;
        private List<TriggerListener> wndProcListeners = new List<TriggerListener>();

        public MainForm()
        {
            InitializeComponent();
            
            lblStatusActive.Visible = chkEnabled.Checked;

            var items = new MenuItem[]{
                new MenuItem("Show", (s, e) => {
                    Show();
                }),
                new MenuItem("Exit", exitProgramToolStripMenuItem_Click)
            };

            ntfIndicator.ContextMenu = new ContextMenu(items);

            var allAttributes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.GetCustomAttribute(typeof(ObjectListViewGroupAttribute), false) != null)
                .Select(t => t.GetCustomAttribute(typeof(ObjectListViewGroupAttribute), false) as ObjectListViewGroupAttribute);
            
            var imageList = new ImageList();
            
            foreach (var attribute in allAttributes)
            {
                if(!imageList.Images.ContainsKey(attribute.Image))
                    imageList.Images.Add(attribute.Image, (Bitmap)Resources.ResourceManager.GetObject(attribute.Image));
            }

            olvMappings.GroupImageList = imageList;

            olvColumnAction.GroupKeyGetter = olvMappings_GroupKeyGetter;
            olvColumnAction.GroupKeyToTitleConverter = olvMappings_GroupKeyToTitleConverter;

            olvColumnTrigger.AspectToStringConverter = delegate (object obj) {
                var trigger = obj as BaseTrigger;

                if(trigger == null)
                    return "(no trigger mapped)";

                return trigger.ToString();
            };
        }

        private void SetSelectedPreset(MappingPreset preset)
        {
            selectedPreset = preset;
            Config.Instance.LastLoadedPreset = preset.FilePath;
                
            olvMappings.SetObjects(preset.MappedOptions);
            olvMappings.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            txtPresetName.Text = preset.Name;
        }

        private void btnAddAction_Click(object sender, EventArgs e)
        {
            if (selectedPreset == null)
                CreateNewPreset();

            EditMappedOption();
        }

        private void CreateNewPreset()
        {
            var preset = new MappingPreset(txtPresetName.Text, selectedPreset?.MappedOptions);

            SetSelectedPreset(preset);
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

            selectedPreset.Save();
            SetSelectedPreset(selectedPreset);
        }

        private void RemoveSelectedMappings()
        {
            var selectedCount = olvMappings.SelectedItems.Count;

            if (selectedCount == 0)
                return;

            if(selectedCount > 1)
                if (MessageBox.Show($"Are you sure you want to remove the {selectedCount} selected mappings?", $"Remove {selectedCount} Mappings", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

            foreach (OLVListItem listItem in olvMappings.SelectedItems)
                selectedPreset.RemoveMapping((MappedOption)listItem.RowObject);

            selectedPreset.Save();
            SetSelectedPreset(selectedPreset);
        }

        private void ArmMappings()
        {
            var listeners = new List<TriggerListener>();
            var allActions = selectedPreset.MappedOptions.Select(m => m.Action).ToList();

            foreach (var mappedOption in selectedPreset.MappedOptions)
            {
                if (mappedOption.Trigger == null)
                    continue;

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
                if (mappedOption.Trigger == null)
                    continue;
                
                var listener = mappedOption.Trigger.GetTriggerListener();
                mappedOption.Action.OnStopListening(listener);

                if (!listeners.Contains(listener))
                    listeners.Add(listener);
            }

            foreach (var listener in listeners)
                listener.StopListening();
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

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            foreach (var wndProcListener in wndProcListeners)
            {
                wndProcListener.WndProc(ref m);
            }
        }

        private void olvMappings_CellClick(object sender, BrightIdeasSoftware.CellClickEventArgs e)
        {
            if (e.ClickCount < 2 || e.Item == null)
                return;

            var mappedOption = olvMappings.SelectedObject as MappedOption;

            if (mappedOption == null)
                return;

            EditMappedOption(mappedOption);
        }

        private object olvMappings_GroupKeyGetter(object rowObject)
        {
            var option = (MappedOption)rowObject;
            var groupType = option.Action.GetType();
            var groupAttributes = (ObjectListViewGroupAttribute[])groupType.GetCustomAttributes(typeof(ObjectListViewGroupAttribute), true);

            if (groupAttributes.Length > 0)
                return groupAttributes[0];

            return null;
        }

        private string olvMappings_GroupKeyToTitleConverter(object groupKey)
        {
            if (groupKey == null)
                return null;

            var groupAttribute = (ObjectListViewGroupAttribute)groupKey;

            return groupAttribute.Name;
        }

        private void olvMappings_AboutToCreateGroups(object sender, BrightIdeasSoftware.CreateGroupsEventArgs e)
        {
            foreach (var group in e.Groups)
            {
                if (group.Key == null)
                    continue;

                var groupAttribute = (ObjectListViewGroupAttribute)group.Key;
                group.TitleImage = groupAttribute.Image;
            }
        }

        private void olvMappings_CellRightClick(object sender, BrightIdeasSoftware.CellRightClickEventArgs e)
        {
            var menu = new ContextMenuStrip();

            var addItem = menu.Items.Add("Add New Mapping");
            addItem.Click += (s, _) =>
            {
                EditMappedOption();
            };

            var selectedCount = olvMappings.SelectedItems.Count;
            
            if (selectedCount > 1)
            {
                var removeItems = menu.Items.Add($"Remove {selectedCount} Mappings");
                removeItems.Click += (s, _) =>
                {
                    RemoveSelectedMappings();
                };
            }
            else if (e.Model is MappedOption mappedOption)
            {
                var removeItem = menu.Items.Add("Remove Mapping");
                removeItem.Click += (s, _) =>
                {
                    selectedPreset.RemoveMapping(mappedOption);
                    selectedPreset.Save();
                    SetSelectedPreset(selectedPreset);
                };
            }

            e.MenuStrip = menu;
        }

        private void olvMappings_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Delete)
                return;

            RemoveSelectedMappings();
        }

        private void BtnOpenTest_Click(object sender, EventArgs e)
        {
        }

        private void ChkEnabled_CheckedChanged(object sender, EventArgs e)
        {
            bool isEnabled = chkEnabled.Checked;

            lblStatusActive.Visible = isEnabled;
            lblStatusInactive.Visible = !isEnabled;

            if (isEnabled)
                ArmMappings();
            else
                DisarmMappings();
        }

        private void TxtPresetName_TextChanged(object sender, EventArgs e)
        {
            selectedPreset.Name = txtPresetName.Text;
            selectedPreset.Save();
        }

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            CreateNewPreset();
        }
        
        private void MainForm_Load(object sender, EventArgs e)
        {
            var lastLoadedPreset = MappingPreset.RestoreLastLoaded();

            if (lastLoadedPreset != null)
                SetSelectedPreset(lastLoadedPreset);
        }

        private void newPresetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateNewPreset();
        }

        private void loadPresetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Load a preset from the file system
            var dialog = new OpenFileDialog();
            dialog.Filter = "KeyToJoy Presets|*" + MappingPreset.EXTENSION;
            dialog.Title = "Load Preset";
            dialog.InitialDirectory = MappingPreset.GetSaveDirectory();
            dialog.RestoreDirectory = true;
            dialog.CheckFileExists = true;
            dialog.CheckPathExists = true;
            dialog.ShowReadOnly = false;

            if (dialog.ShowDialog() != DialogResult.OK)
                return;
            
            var preset = MappingPreset.Load(dialog.FileName);
                
            if (preset == null)
            {
                MessageBox.Show("The selected preset was corrupt! If you did not modify the preset file this could be a bug.\n\nPlease help us by reporting the bug on GitHub: https://github.com/luttje/KeyToJoy.", "Failed to load preset!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SetSelectedPreset(preset);
        }

        private void savePresetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("When you make changes to a preset, changes are automatically saved. This button is only here to explain that feature to you.", "Preset already saved!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void savePresetCopyAsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void openPresetFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(MappingPreset.GetSaveDirectory());
        }

        private void gamePadPressAndReleaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectedPreset.AddMappingRange(GamePadAction.GetAllButtonActions(PressState.PressAndRelease));
            selectedPreset.Save();
            SetSelectedPreset(selectedPreset);
        }

        private void gamePadPressToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectedPreset.AddMappingRange(GamePadAction.GetAllButtonActions(PressState.Press));
            selectedPreset.Save();
            SetSelectedPreset(selectedPreset);
        }

        private void gamePadReleaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectedPreset.AddMappingRange(GamePadAction.GetAllButtonActions(PressState.Release));
            selectedPreset.Save();
            SetSelectedPreset(selectedPreset);
        }

        private void keyboardPressAndReleaseToolStripMenuItem(object sender, EventArgs e)
        {
            selectedPreset.AddMappingRange(KeyboardAction.GetAllButtonActions(PressState.PressAndRelease));
            selectedPreset.Save();
            SetSelectedPreset(selectedPreset);
        }

        private void keyboardPressToolStripMenuItem(object sender, EventArgs e)
        {
            selectedPreset.AddMappingRange(KeyboardAction.GetAllButtonActions(PressState.Press));
            selectedPreset.Save();
            SetSelectedPreset(selectedPreset);
        }

        private void keyboardReleaseToolStripMenuItem(object sender, EventArgs e)
        {
            selectedPreset.AddMappingRange(KeyboardAction.GetAllButtonActions(PressState.Release));
            selectedPreset.Save();
            SetSelectedPreset(selectedPreset);
        }

        private void testGamePadJoystickToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://devicetests.com/controller-tester");
        }

        private void testKeyboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://devicetests.com/keyboard-tester");
        }

        private void testMouseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://devicetests.com/mouse-test");
        }

        private void reportAProblemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/luttje/KeyToJoy/issues");
        }

        private void viewSourceCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/luttje/KeyToJoy");
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog();
        }
        
        private void ntfIndicator_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();

                if (Config.Instance.MuteCloseExitMessage)
                    return;

                var result = MessageBox.Show("Closing this window minimizes it to the notification tray in your taskbar. You can shut down KeyToJoy through File > Exit Program.\n\nContinue showing this message?", "Minimizing to notification tray.", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                
                Config.Instance.MuteCloseExitMessage = result != DialogResult.Yes;
            }
        }

        private void exitProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void olvMappings_FormatCell(object sender, FormatCellEventArgs e)
        {
            if (e.CellValue != null)
                return;

            e.SubItem.ForeColor = SystemColors.GrayText;
            e.SubItem.Font = new Font(e.SubItem.Font, FontStyle.Italic);
        }
    }
}
