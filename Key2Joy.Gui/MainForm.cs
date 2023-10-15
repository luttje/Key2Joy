using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using BrightIdeasSoftware;
using Key2Joy.Config;
using Key2Joy.Contracts;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Contracts.Mapping.Triggers;
using Key2Joy.Gui.Properties;
using Key2Joy.LowLevelInput;
using Key2Joy.Mapping;
using Key2Joy.Mapping.Actions;
using Key2Joy.Mapping.Actions.Input;
using Key2Joy.Mapping.Actions.Logic;
using Key2Joy.Mapping.Triggers;

namespace Key2Joy.Gui;

public partial class MainForm : Form, IAcceptAppCommands, IHaveHandleAndInvoke
{
    private readonly IDictionary<string, CachedMappingGroup> cachedMappingGroups;
    private MappingProfile selectedProfile;

    public MainForm(bool shouldStartMinimized = false)
    {
        this.cachedMappingGroups = new Dictionary<string, CachedMappingGroup>();

        this.InitializeComponent();

        if (shouldStartMinimized)
        {
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
        }

        this.lblStatusActive.Visible = this.chkEnabled.Checked;

        var items = new MenuItem[]{
            new MenuItem("Show", (s, e) => {
                this.Show();
                this.BringToFront();

                if (this.WindowState == FormWindowState.Minimized) { this.WindowState = FormWindowState.Normal; } }),
            new MenuItem("Exit", this.ExitProgramToolStripMenuItem_Click)
        };

        this.ntfIndicator.ContextMenu = new ContextMenu(items);

        var allAttributes = ActionsRepository.GetAllActionAttributes();
        ImageList imageList = new();

        foreach (var attribute in allAttributes)
        {
            if (attribute.GroupImage != null && !imageList.Images.ContainsKey(attribute.GroupImage))
            {
                imageList.Images.Add(attribute.GroupImage, (Bitmap)Resources.ResourceManager.GetObject(attribute.GroupImage));
            }
        }

        this.olvMappings.GroupImageList = imageList;

        this.olvColumnAction.GroupKeyGetter += this.OlvMappings_GroupKeyGetter;
        this.olvColumnAction.GroupKeyToTitleConverter += this.OlvMappings_GroupKeyToTitleConverter;
        this.olvMappings.BeforeCreatingGroups += this.OlvMappings_BeforeCreatingGroups;

        this.olvColumnTrigger.AspectToStringConverter = delegate (object obj)
        {
            var trigger = obj as CoreTrigger;

            if (trigger == null)
            {
                return "(no trigger mapped)";
            }

            return trigger.ToString();
        };
    }

    private void SetSelectedProfile(MappingProfile profile)
    {
        this.selectedProfile = profile;
        ConfigManager.Config.LastLoadedProfile = profile.FilePath;

        this.olvMappings.SetObjects(profile.MappedOptions);
        this.olvMappings.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        this.olvMappings.Sort(this.olvColumnTrigger, SortOrder.Ascending);

        this.UpdateSelectedProfileName();
    }

    private void UpdateSelectedProfileName() => this.txtProfileName.Text = this.selectedProfile.Name;

    private void SetStatusView(bool isEnabled)
    {
        this.chkEnabled.CheckedChanged -= this.ChkEnabled_CheckedChanged;
        this.chkEnabled.Checked = isEnabled;
        this.chkEnabled.CheckedChanged += this.ChkEnabled_CheckedChanged;

        this.lblStatusActive.Visible = isEnabled;
        this.lblStatusInactive.Visible = !isEnabled;
    }

    private MappingProfile CreateNewProfile(string nameSuffix = default)
    {
        MappingProfile profile = new($"{this.txtProfileName.Text}{nameSuffix}", this.selectedProfile?.MappedOptions);

        this.SetSelectedProfile(profile);

        return profile;
    }

    private void EditMappedOption(MappedOption existingMappedOption = null)
    {
        this.chkEnabled.Checked = false;
        MappingForm mappingForm = new(existingMappedOption);
        var result = mappingForm.ShowDialog();

        if (result == DialogResult.Cancel)
        {
            return;
        }

        var mappedOption = mappingForm.MappedOption;

        if (existingMappedOption == null)
        {
            this.selectedProfile.AddMapping(mappedOption);
        }

        this.selectedProfile.Save();

        if (existingMappedOption == null)
        {
            this.olvMappings.AddObject(mappedOption);
        }
        else
        {
            this.olvMappings.UpdateObject(mappedOption);
        }
    }

    private void RemoveMapping(MappedOption mappedOption)
    {
        this.selectedProfile.RemoveMapping(mappedOption);
        this.olvMappings.RemoveObject(mappedOption);
    }

    private void RemoveSelectedMappings()
    {
        var selectedCount = this.olvMappings.SelectedItems.Count;

        if (selectedCount == 0)
        {
            return;
        }

        if (selectedCount > 1)
        {
            if (MessageBox.Show($"Are you sure you want to remove the {selectedCount} selected mappings?", $"Remove {selectedCount} Mappings", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }
        }

        foreach (OLVListItem listItem in this.olvMappings.SelectedItems)
        {
            this.RemoveMapping((MappedOption)listItem.RowObject);
        }

        this.selectedProfile.Save();
    }

    public bool RunAppCommand(AppCommand command)
    {
        switch (command)
        {
            case AppCommand.Abort:
                this.BeginInvoke(new MethodInvoker(delegate
                {
                    this.chkEnabled.Checked = false;
                }));

                return true;

            case AppCommand.ResetScriptEnvironment:
                break;

            default:
                break;
        }

        return false;
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
        var lastLoadedProfile = MappingProfile.RestoreLastLoaded();

        if (lastLoadedProfile != null)
        {
            this.SetSelectedProfile(lastLoadedProfile);
        }

        // Ensure the manager knows which window handle catches all inputs
        Key2JoyManager.Instance.SetHandlerWithInvoke(this);
        Key2JoyManager.Instance.StatusChanged += (s, ev) =>
        {
            this.SetStatusView(ev.IsEnabled);

            if (ev.Profile != null)
            {
                this.SetSelectedProfile(ev.Profile);
            }
        };
    }

    private void BtnCreateMapping_Click(object sender, EventArgs e)
    {
        if (this.selectedProfile == null)
        {
            this.CreateNewProfile();
        }

        this.EditMappedOption();
    }

    private void OlvMappings_CellClick(object sender, CellClickEventArgs e)
    {
        if (e.ClickCount < 2 || e.Item == null)
        {
            return;
        }

        if (this.olvMappings.SelectedObject is not MappedOption mappedOption)
        {
            return;
        }

        this.EditMappedOption(mappedOption);
    }

    private CachedMappingGroup GetGroupOrCreateInCache(ActionAttribute attribute)
    {
        var uniqueId = attribute.GroupName + attribute.GroupImage;

        if (!this.cachedMappingGroups.TryGetValue(uniqueId, out var mapping))
        {
            this.cachedMappingGroups.Add(uniqueId, mapping = new CachedMappingGroup
            {
                Name = attribute.GroupName,
                Image = attribute.GroupImage,
            });
        }

        return mapping;
    }

    private object OlvMappings_GroupKeyGetter(object rowObject)
    {
        var option = (AbstractMappedOption)rowObject;
        var actionAttribute = ActionsRepository.GetAttributeForAction(option.Action);

        if (actionAttribute != null)
        {
            return this.GetGroupOrCreateInCache(actionAttribute);
        }

        return null;
    }

    private string OlvMappings_GroupKeyToTitleConverter(object groupKey)
    {
        if (groupKey == null)
        {
            return null;
        }

        var groupAttribute = (CachedMappingGroup)groupKey;

        return groupAttribute.Name;
    }

    private void OlvMappings_BeforeCreatingGroups(object sender, CreateGroupsEventArgs e)
    {
        e.Parameters.GroupComparer = new MappingGroupComparer();
        e.Parameters.ItemComparer = new MappingGroupItemComparer(e.Parameters.PrimarySort, e.Parameters.PrimarySortOrder);
    }

    private void OlvMappings_AboutToCreateGroups(object sender, CreateGroupsEventArgs e)
    {
        foreach (var group in e.Groups)
        {
            if (group.Key == null)
            {
                continue;
            }

            var groupAttribute = (CachedMappingGroup)group.Key;
            group.TitleImage = groupAttribute.Image;
        }
    }

    private void OlvMappings_CellRightClick(object sender, CellRightClickEventArgs e)
    {
        ContextMenuStrip menu = new();

        var addItem = menu.Items.Add("Add New Mapping");
        addItem.Click += (s, _) => this.EditMappedOption();

        var selectedCount = this.olvMappings.SelectedItems.Count;

        if (selectedCount > 1)
        {
            var removeItems = menu.Items.Add($"Remove {selectedCount} Mappings");
            removeItems.Click += (s, _) =>
            {
                this.RemoveSelectedMappings();
                this.selectedProfile.Save();
            };
        }
        else if (e.Model is MappedOption mappedOption)
        {
            var removeItem = menu.Items.Add("Remove Mapping");
            removeItem.Click += (s, _) => this.RemoveMapping(mappedOption);
        }

        e.MenuStrip = menu;
    }

    private void OlvMappings_KeyUp(object sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Delete)
        {
            return;
        }

        this.RemoveSelectedMappings();
    }

    private void OlvMappings_FormatCell(object sender, FormatCellEventArgs e)
    {
        if (e.CellValue != null)
        {
            return;
        }

        e.SubItem.ForeColor = SystemColors.GrayText;
        e.SubItem.Font = new Font(e.SubItem.Font, FontStyle.Italic);
    }

    private void ChkEnabled_CheckedChanged(object sender, EventArgs e)
    {
        var isEnabled = this.chkEnabled.Checked;

        this.SetStatusView(isEnabled);

        if (isEnabled)
        {
            Key2JoyManager.Instance.ArmMappings(this.selectedProfile);
        }
        else
        {
            Key2JoyManager.Instance.DisarmMappings();
        }
    }

    private void TxtProfileName_TextChanged(object sender, EventArgs e)
    {
        this.selectedProfile.Name = this.txtProfileName.Text;
        this.selectedProfile.Save();
    }

    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (e.CloseReason == CloseReason.UserClosing)
        {
            e.Cancel = true;
            this.Hide();

            if (ConfigManager.Config.MuteCloseExitMessage)
            {
                return;
            }

            var result = MessageBox.Show("Closing this window minimizes it to the notification tray in your taskbar. You can shut down Key2Joy through File > Exit Program.\n\nContinue showing this message?", "Minimizing to notification tray.", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            ConfigManager.Config.MuteCloseExitMessage = result != DialogResult.Yes;
        }
    }

    private void NewProfileToolStripMenuItem_Click(object sender, EventArgs e) => this.CreateNewProfile(" - Copy");

    private void LoadProfileToolStripMenuItem_Click(object sender, EventArgs e)
    {
        // Load a profile from the file system
        OpenFileDialog dialog = new()
        {
            Filter = "Key2Joy Profiles|*" + MappingProfile.EXTENSION,
            Title = "Load Profile",
            InitialDirectory = MappingProfile.GetSaveDirectory(),
            RestoreDirectory = true,
            CheckFileExists = true,
            CheckPathExists = true,
            ShowReadOnly = false
        };

        if (dialog.ShowDialog() != DialogResult.OK)
        {
            return;
        }

        var profile = MappingProfile.Load(dialog.FileName);

        if (profile == null)
        {
            MessageBox.Show("The selected profile was corrupt! If you did not modify the profile file this could be a bug.\n\nPlease help us by reporting the bug on GitHub: https://github.com/luttje/Key2Joy.", "Failed to load profile!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        this.SetSelectedProfile(profile);
    }

    private void SaveProfileToolStripMenuItem_Click(object sender, EventArgs e) => MessageBox.Show("When you make changes to a profile, changes are automatically saved. This button is only here to explain that feature to you.", "Profile already saved!", MessageBoxButtons.OK, MessageBoxIcon.Information);

    private void OpenProfileFolderToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (this.selectedProfile == null)
        {
            Process.Start(MappingProfile.GetSaveDirectory());
            return;
        }

        var argument = "/select, \"" + this.selectedProfile.FilePath + "\"";
        Process.Start("explorer.exe", argument);
    }

    private void ExitProgramToolStripMenuItem_Click(object sender, EventArgs e) => Application.Exit();

    private void CreateNewMappingToolStripMenuItem_Click(object sender, EventArgs e) => this.BtnCreateMapping_Click(sender, e);

    private void GamePadPressAndReleaseToolStripMenuItem_Click(object sender, EventArgs e)
    {
        List<MappedOption> range = new();
        range.AddRange(GamePadAction.GetAllButtonActions(PressState.Press));
        range.AddRange(GamePadAction.GetAllButtonActions(PressState.Release));

        this.selectedProfile.AddMappingRange(range);
        this.selectedProfile.Save();
        this.olvMappings.AddObjects(range);
    }

    private void GamePadPressToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var range = GamePadAction.GetAllButtonActions(PressState.Press);
        this.selectedProfile.AddMappingRange(range);
        this.selectedProfile.Save();
        this.olvMappings.AddObjects(range);
    }

    private void GamePadReleaseToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var range = GamePadAction.GetAllButtonActions(PressState.Release);
        this.selectedProfile.AddMappingRange(range);
        this.selectedProfile.Save();
        this.olvMappings.AddObjects(range);
    }

    private void KeyboardPressAndReleaseToolStripMenuItem_Click(object sender, EventArgs e)
    {
        List<MappedOption> range = new();
        range.AddRange(KeyboardAction.GetAllButtonActions(PressState.Press));
        range.AddRange(KeyboardAction.GetAllButtonActions(PressState.Release));

        this.selectedProfile.AddMappingRange(range);
        this.selectedProfile.Save();
        this.olvMappings.AddObjects(range);
    }

    private void KeyboardPressToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var range = KeyboardAction.GetAllButtonActions(PressState.Press);
        this.selectedProfile.AddMappingRange(range);
        this.selectedProfile.Save();
        this.olvMappings.AddObjects(range);
    }

    private void KeyboardReleaseToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var range = KeyboardAction.GetAllButtonActions(PressState.Release);
        this.selectedProfile.AddMappingRange(range);
        this.selectedProfile.Save();
        this.olvMappings.AddObjects(range);
    }

    private void TestKeyboardToolStripMenuItem_Click(object sender, EventArgs e) => Process.Start("https://devicetests.com/keyboard-tester");

    private void TestMouseToolStripMenuItem_Click(object sender, EventArgs e) => Process.Start("https://devicetests.com/mouse-test");

    private void UserConfigurationsToolStripMenuItem_Click(object sender, EventArgs e) => new ConfigForm().ShowDialog();

    private void ReportAProblemToolStripMenuItem_Click(object sender, EventArgs e) => Process.Start("https://github.com/luttje/Key2Joy/issues");

    private void ViewSourceCodeToolStripMenuItem_Click(object sender, EventArgs e) => Process.Start("https://github.com/luttje/Key2Joy");

    private void AboutToolStripMenuItem_Click(object sender, EventArgs e) => new AboutForm().ShowDialog();

    private void NtfIndicator_MouseDoubleClick(object sender, MouseEventArgs e)
    {
        this.WindowState = FormWindowState.Normal;
        this.ShowInTaskbar = true;
        this.Show();
    }

    private void CloseToolStripMenuItem_Click(object sender, EventArgs e) => this.Close();

    private void ViewLogFileToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var logFile = Output.GetLogPath();

        if (!File.Exists(logFile))
        {
            MessageBox.Show("The log file does not exist yet. Please wait for the program to finish writing to it.", "Log file not found!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        Process.Start(logFile);
    }

    private void ViewEventViewerToolStripMenuItem_Click(object sender, EventArgs e) => Process.Start("eventvwr.exe", "/c:Application");

    private void DevicetestscomToolStripMenuItem_Click(object sender, EventArgs e) => Process.Start("https://devicetests.com/controller-tester");

    private void GamepadtestercomToolStripMenuItem_Click(object sender, EventArgs e) => Process.Start("https://gamepad-tester.com/");

    private void OpenPluginsFolderToolStripMenuItem_Click(object sender, EventArgs e) => Process.Start(Program.Plugins.PluginsFolder);

    private void ManagePluginsToolStripMenuItem_Click(object sender, EventArgs e) => new PluginsForm().ShowDialog();

    private void GenerateOppositePressStateMappingsToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var selectedCount = this.olvMappings.SelectedItems.Count;

        if (selectedCount == 0)
        {
            return;
        }

        if (selectedCount > 1)
        {
            if (MessageBox.Show($"Are you sure you want to create opposite press state mappings for all {selectedCount} selected mappings? New 'Release' mappings will be created for each 'Press' and vice versa.", $"Generate {selectedCount} opposite press state mappings", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }
        }

        List<MappedOption> newOptions = new();

        foreach (OLVListItem listItem in this.olvMappings.SelectedItems)
        {
            var pressVariant = (MappedOption)listItem.RowObject;
            var actionCopy = (AbstractAction)pressVariant.Action.Clone();
            var triggerCopy = (AbstractTrigger)pressVariant.Trigger.Clone();

            if (actionCopy is IPressState actionWithPressState)
            {
                actionWithPressState.PressState = actionWithPressState.PressState == PressState.Press ? PressState.Release : PressState.Press;
            }

            if (triggerCopy is IPressState triggerWithPressState)
            {
                triggerWithPressState.PressState = triggerWithPressState.PressState == PressState.Press ? PressState.Release : PressState.Press;
            }

            MappedOption variantOption = new()
            {
                Action = actionCopy,
                Trigger = triggerCopy,
            };
            newOptions.Add(variantOption);
            this.selectedProfile.MappedOptions.Add(variantOption);
        }

        this.selectedProfile.Save();
        this.olvMappings.AddObjects(newOptions);
    }
}
