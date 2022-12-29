﻿using SimWinInput;
using System;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using Key2Joy.Mapping;
using Key2Joy.Gui.Properties;
using System.Collections.Generic;
using System.Linq;
using Key2Joy.Util;
using System.Reflection;
using System.Threading.Tasks;
using System.IO;
using BrightIdeasSoftware;
using Key2Joy.LowLevelInput;
using Key2Joy.Config;

namespace Key2Joy.Gui
{
    public partial class MainForm : Form, IAcceptAppCommands
    {
        private MappingProfile selectedProfile;

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

        private void SetSelectedProfile(MappingProfile profile)
        {
            selectedProfile = profile;
            ConfigManager.Instance.LastLoadedProfile = profile.FilePath;
                
            olvMappings.SetObjects(profile.MappedOptions);
            olvMappings.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            olvMappings.Sort(olvColumnTrigger, SortOrder.Ascending);
            
            UpdateSelectedProfileName();
        }

        private void UpdateSelectedProfileName()
        {
            txtProfileName.Text = selectedProfile.Name;
        }

        private void SetStatusView(bool isEnabled)
        {
            chkEnabled.CheckedChanged -= ChkEnabled_CheckedChanged;
            chkEnabled.Checked = isEnabled;
            chkEnabled.CheckedChanged += ChkEnabled_CheckedChanged;

            lblStatusActive.Visible = isEnabled;
            lblStatusInactive.Visible = !isEnabled;
        }

        private MappingProfile CreateNewProfile(string nameSuffix = default)
        {
            var profile = new MappingProfile($"{txtProfileName.Text}{nameSuffix}", selectedProfile?.MappedOptions);

            SetSelectedProfile(profile);

            return profile;
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
                selectedProfile.AddMapping(mappedOption);

            selectedProfile.Save();

            if (existingMappedOption == null)
                olvMappings.AddObject(mappedOption);
            else
                olvMappings.UpdateObject(mappedOption);
        }

        private void RemoveMapping(MappedOption mappedOption)
        {
            selectedProfile.RemoveMapping(mappedOption);
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

            selectedProfile.Save();
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

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Ensure the manager knows which window handle catches all inputs
            Key2JoyManager.Instance.SetMainForm(this);
            Key2JoyManager.Instance.StatusChanged += (s, ev) =>
            {
                SetStatusView(ev.IsEnabled);

                if(ev.Profile != null)
                    SetSelectedProfile(ev.Profile);
            };

            var lastLoadedProfile = MappingProfile.RestoreLastLoaded();

            if (lastLoadedProfile != null)
                SetSelectedProfile(lastLoadedProfile);
        }

        private void btnCreateMapping_Click(object sender, EventArgs e)
        {
            if (selectedProfile == null)
                CreateNewProfile();

            EditMappedOption();
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
                    selectedProfile.Save();
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
            var isEnabled = chkEnabled.Checked;

            SetStatusView(isEnabled);

            if (isEnabled)
                Key2JoyManager.Instance.ArmMappings(selectedProfile);
            else
                Key2JoyManager.Instance.DisarmMappings();
        }

        private void TxtProfileName_TextChanged(object sender, EventArgs e)
        {
            selectedProfile.Name = txtProfileName.Text;
            selectedProfile.Save();
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

        private void newProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateNewProfile(" - Copy");
        }

        private void loadProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Load a profile from the file system
            var dialog = new OpenFileDialog();
            dialog.Filter = "Key2Joy Profiles|*" + MappingProfile.EXTENSION;
            dialog.Title = "Load Profile";
            dialog.InitialDirectory = MappingProfile.GetSaveDirectory();
            dialog.RestoreDirectory = true;
            dialog.CheckFileExists = true;
            dialog.CheckPathExists = true;
            dialog.ShowReadOnly = false;

            if (dialog.ShowDialog() != DialogResult.OK)
                return;
            
            var profile = MappingProfile.Load(dialog.FileName);
                
            if (profile == null)
            {
                MessageBox.Show("The selected profile was corrupt! If you did not modify the profile file this could be a bug.\n\nPlease help us by reporting the bug on GitHub: https://github.com/luttje/Key2Joy.", "Failed to load profile!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SetSelectedProfile(profile);
        }

        private void saveProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("When you make changes to a profile, changes are automatically saved. This button is only here to explain that feature to you.", "Profile already saved!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void openProfileFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedProfile == null)
            {
                Process.Start(MappingProfile.GetSaveDirectory());
                return;
            }

            var argument = "/select, \"" + selectedProfile.FilePath + "\"";
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

            selectedProfile.AddMappingRange(range);
            selectedProfile.Save();
            olvMappings.AddObjects(range);
        }

        private void gamePadPressToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var range = GamePadAction.GetAllButtonActions(PressState.Press);
            selectedProfile.AddMappingRange(range);
            selectedProfile.Save();
            olvMappings.AddObjects(range);
        }

        private void gamePadReleaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var range = GamePadAction.GetAllButtonActions(PressState.Release);
            selectedProfile.AddMappingRange(range);
            selectedProfile.Save();
            olvMappings.AddObjects(range);
        }

        private void keyboardPressAndReleaseToolStripMenuItem(object sender, EventArgs e)
        {
            var range = new List<MappedOption>();
            range.AddRange(KeyboardAction.GetAllButtonActions(PressState.Press));
            range.AddRange(KeyboardAction.GetAllButtonActions(PressState.Release));

            selectedProfile.AddMappingRange(range);
            selectedProfile.Save();
            olvMappings.AddObjects(range);
        }

        private void keyboardPressToolStripMenuItem(object sender, EventArgs e)
        {
            var range = KeyboardAction.GetAllButtonActions(PressState.Press);
            selectedProfile.AddMappingRange(range);
            selectedProfile.Save();
            olvMappings.AddObjects(range);
        }

        private void keyboardReleaseToolStripMenuItem(object sender, EventArgs e)
        {
            var range = KeyboardAction.GetAllButtonActions(PressState.Release);
            selectedProfile.AddMappingRange(range);
            selectedProfile.Save();
            olvMappings.AddObjects(range);
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

        private void devicetestscomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://devicetests.com/controller-tester");
        }

        private void gamepadtestercomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://gamepad-tester.com/");
        }
    }
}