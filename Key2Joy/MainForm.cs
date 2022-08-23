using SimWinInput;
using System;
using System.Windows.Forms;
using Linearstar.Windows.RawInput;
using System.Drawing;
using System.Diagnostics;
using Key2Joy.Mapping;
using Key2Joy.Properties;
using System.Collections.Generic;
using System.Linq;
using Key2Joy.Util;
using System.Reflection;
using System.Threading.Tasks;
using System.IO;
using BrightIdeasSoftware;
using Key2Joy.LowLevelInput;
using Key2Joy.Config;

namespace Key2Joy
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
                    BringToFront();

                    if (WindowState == FormWindowState.Minimized)
                        WindowState = FormWindowState.Normal;
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

            olvColumnAction.GroupKeyGetter += olvMappings_GroupKeyGetter;
            olvColumnAction.GroupKeyToTitleConverter += olvMappings_GroupKeyToTitleConverter;
            olvMappings.BeforeCreatingGroups += olvMappings_BeforeCreatingGroups;

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
            ConfigManager.Instance.LastLoadedPreset = preset.FilePath;
                
            olvMappings.SetObjects(preset.MappedOptions);
            olvMappings.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            olvMappings.Sort(olvColumnTrigger, SortOrder.Ascending);
            
            UpdateSelectedPresetName();
        }

        private void UpdateSelectedPresetName()
        {
            txtPresetName.Text = selectedPreset.Name;
        }

        private void btnCreateMapping_Click(object sender, EventArgs e)
        {
            if (selectedPreset == null)
                CreateNewPreset();

            EditMappedOption();
        }

        private MappingPreset CreateNewPreset(string nameSuffix = default)
        {
            var preset = new MappingPreset($"{txtPresetName.Text}{nameSuffix}", selectedPreset?.MappedOptions);

            SetSelectedPreset(preset);

            return preset;
        }
        
        private void EditMappedOption(MappedOption existingMappedOption = null)
        {
            chkEnabled.Checked = false;
            var mappingForm = new MappingForm(existingMappedOption);
            var result = mappingForm.ShowDialog();

            if (result == DialogResult.Cancel)
                return;

            var mappedOption = mappingForm.MappedOption;
            
            if (existingMappedOption == null)
                selectedPreset.AddMapping(mappedOption);

            selectedPreset.Save();
            olvMappings.UpdateObject(mappedOption);
        }

        private void RemoveMapping(MappedOption mappedOption)
        {
            selectedPreset.RemoveMapping(mappedOption);
            olvMappings.RemoveObject(mappedOption);
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
                RemoveMapping((MappedOption)listItem.RowObject);

            selectedPreset.Save();
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

        public bool RunAppCommand(AppCommand command)
        {

            switch (command)
            {
                case AppCommand.Abort:
                    BeginInvoke(new MethodInvoker(delegate
                    {
                        chkEnabled.Checked = false;
                    }));

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
            var groupAttribute = groupType.GetCustomAttribute<ObjectListViewGroupAttribute>(true);

            if (groupAttribute != null)
                return groupAttribute;

            return null;
        }

        private string olvMappings_GroupKeyToTitleConverter(object groupKey)
        {
            if (groupKey == null)
                return null;

            var groupAttribute = (ObjectListViewGroupAttribute)groupKey;

            return groupAttribute.Name;
        }

        private void olvMappings_BeforeCreatingGroups(object sender, CreateGroupsEventArgs e)
        {
            e.Parameters.GroupComparer = new MappingGroupComparer();
            e.Parameters.ItemComparer = new MappingGroupItemComparer(e.Parameters.PrimarySort, e.Parameters.PrimarySortOrder);
        }
        
        private void olvMappings_AboutToCreateGroups(object sender, CreateGroupsEventArgs e)
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
                    selectedPreset.Save();
                };
            }
            else if (e.Model is MappedOption mappedOption)
            {
                var removeItem = menu.Items.Add("Remove Mapping");
                removeItem.Click += (s, _) =>
                {
                    RemoveMapping(mappedOption);
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

        private void olvMappings_FormatCell(object sender, FormatCellEventArgs e)
        {
            if (e.CellValue != null)
                return;

            e.SubItem.ForeColor = SystemColors.GrayText;
            e.SubItem.Font = new Font(e.SubItem.Font, FontStyle.Italic);
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

        private void MainForm_Load(object sender, EventArgs e)
        {
            var lastLoadedPreset = MappingPreset.RestoreLastLoaded();

            if (lastLoadedPreset != null)
                SetSelectedPreset(lastLoadedPreset);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();

                if (ConfigManager.Instance.MuteCloseExitMessage)
                    return;

                var result = MessageBox.Show("Closing this window minimizes it to the notification tray in your taskbar. You can shut down Key2Joy through File > Exit Program.\n\nContinue showing this message?", "Minimizing to notification tray.", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                ConfigManager.Instance.MuteCloseExitMessage = result != DialogResult.Yes;
            }
        }

        private void newPresetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var copy = CreateNewPreset(" - Copy");
        }

        private void loadPresetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Load a preset from the file system
            var dialog = new OpenFileDialog();
            dialog.Filter = "Key2Joy Presets|*" + MappingPreset.EXTENSION;
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
                MessageBox.Show("The selected preset was corrupt! If you did not modify the preset file this could be a bug.\n\nPlease help us by reporting the bug on GitHub: https://github.com/luttje/Key2Joy.", "Failed to load preset!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SetSelectedPreset(preset);
        }

        private void savePresetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("When you make changes to a preset, changes are automatically saved. This button is only here to explain that feature to you.", "Preset already saved!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void openPresetFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedPreset == null)
            {
                Process.Start(MappingPreset.GetSaveDirectory());
                return;
            }

            var argument = "/select, \"" + selectedPreset.FilePath + "\"";
            Process.Start("explorer.exe", argument);
        }

        private void exitProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void createNewMappingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnCreateMapping_Click(sender, e);
        }

        private void gamePadPressAndReleaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var range = new List<MappedOption>();
            range.AddRange(GamePadAction.GetAllButtonActions(PressState.Press));
            range.AddRange(GamePadAction.GetAllButtonActions(PressState.Release));

            selectedPreset.AddMappingRange(range);
            selectedPreset.Save();
            olvMappings.AddObjects(range);
        }

        private void gamePadPressToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var range = GamePadAction.GetAllButtonActions(PressState.Press);
            selectedPreset.AddMappingRange(range);
            selectedPreset.Save();
            olvMappings.AddObjects(range);
        }

        private void gamePadReleaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var range = GamePadAction.GetAllButtonActions(PressState.Release);
            selectedPreset.AddMappingRange(range);
            selectedPreset.Save();
            olvMappings.AddObjects(range);
        }

        private void keyboardPressAndReleaseToolStripMenuItem(object sender, EventArgs e)
        {
            var range = new List<MappedOption>();
            range.AddRange(KeyboardAction.GetAllButtonActions(PressState.Press));
            range.AddRange(KeyboardAction.GetAllButtonActions(PressState.Release));

            selectedPreset.AddMappingRange(range);
            selectedPreset.Save();
            olvMappings.AddObjects(range);
        }

        private void keyboardPressToolStripMenuItem(object sender, EventArgs e)
        {
            var range = KeyboardAction.GetAllButtonActions(PressState.Press);
            selectedPreset.AddMappingRange(range);
            selectedPreset.Save();
            olvMappings.AddObjects(range);
        }

        private void keyboardReleaseToolStripMenuItem(object sender, EventArgs e)
        {
            var range = KeyboardAction.GetAllButtonActions(PressState.Release);
            selectedPreset.AddMappingRange(range);
            selectedPreset.Save();
            olvMappings.AddObjects(range);
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

        private void userConfigurationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ConfigForm().ShowDialog();
        }

        private void reportAProblemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/luttje/Key2Joy/issues");
        }

        private void viewSourceCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/luttje/Key2Joy");
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

        private void viewLogFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var logFile = Output.GetLogPath();

            if (!File.Exists(logFile))
            {
                MessageBox.Show("The log file does not exist yet. Please wait for the program to finish writing to it.", "Log file not found!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Process.Start(logFile);
        }

        private void viewEventViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("eventvwr.exe", "/c:Application");
        }
    }
}
