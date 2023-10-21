using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Windows.Forms;
using BrightIdeasSoftware;
using CommandLine;
using CommonServiceLocator;
using Key2Joy.Config;
using Key2Joy.Contracts;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Gui.Properties;
using Key2Joy.Gui.Util;
using Key2Joy.LowLevelInput;
using Key2Joy.Mapping;
using Key2Joy.Mapping.Actions;
using Key2Joy.Mapping.Actions.Input;
using Key2Joy.Mapping.Actions.Logic;
using Key2Joy.Mapping.Triggers;

namespace Key2Joy.Gui;

public partial class MainForm : Form, IAcceptAppCommands, IHaveHandleAndInvoke
{
    private readonly IDictionary<string, CachedMappingGroup> cachedMappingGroups = new Dictionary<string, CachedMappingGroup>();
    private readonly ConfigState configState;

    private MappingProfile selectedProfile;

    public MainForm(bool shouldStartMinimized = false)
    {
        this.configState = ServiceLocator.Current
            .GetInstance<IConfigManager>()
            .GetConfigState();

        this.InitializeComponent();

        this.ApplyMinimizedStateIfNeeded(shouldStartMinimized);
        this.ConfigureStatusLabels();
        this.SetupNotificationIndicator();
        this.PopulateGroupImages();
        this.RegisterListViewEvents();
        this.ConfigureTriggerColumn();
    }

    /// <summary>
    /// Shows a notification banner at the top of the app.
    /// </summary>
    /// <param name="banner"></param>
    private void ShowNotification(NotificationBannerControl banner)
    {
        banner.Dock = DockStyle.Top;
        this.pnlNotificationsParent.Controls.Add(banner);
        this.pnlNotificationsParent.PerformLayout();
    }

    /// <summary>
    /// Refresh the listed mappings, their sorting and formatting.
    /// Call this after making a change to the mapped options.
    /// </summary>
    private void RefreshMappings()
        => this.olvMappings.SetObjects(this.selectedProfile.MappedOptions);

    private void ApplyMinimizedStateIfNeeded(bool shouldMinimize)
    {
        this.WindowState = shouldMinimize ? FormWindowState.Minimized : FormWindowState.Normal;
        this.ShowInTaskbar = !shouldMinimize;
    }

    private void ConfigureStatusLabels()
        => this.lblStatusActive.Visible = this.chkArmed.Checked;

    private void SetupNotificationIndicator()
    {
        var items = new MenuItem[]{
            new MenuItem("Show", (s, e) => {
                this.Show();
                this.BringToFront();

                if (this.WindowState == FormWindowState.Minimized) { this.WindowState = FormWindowState.Normal; } }),
            new MenuItem("Exit", this.ExitProgramToolStripMenuItem_Click)
        };

        this.ntfIndicator.ContextMenu = new ContextMenu(items);
    }

    private void PopulateGroupImages()
    {
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
    }

    private void RegisterListViewEvents()
    {
        this.olvColumnAction.GroupKeyGetter += this.OlvMappings_GroupKeyGetter;
        this.olvColumnAction.GroupKeyToTitleConverter += this.OlvMappings_GroupKeyToTitleConverter;

        this.olvMappings.BeforeCreatingGroups += this.OlvMappings_BeforeCreatingGroups;
        this.olvMappings.AboutToCreateGroups += this.OlvMappings_AboutToCreateGroups;

        this.olvMappings.CellClick += this.OlvMappings_CellClick;
        this.olvMappings.CellRightClick += this.OlvMappings_CellRightClick;
        this.olvMappings.FormatRow += this.OlvMappings_FormatRow;
        this.olvMappings.FormatCell += this.OlvMappings_FormatCell;
        this.olvMappings.KeyUp += this.OlvMappings_KeyUp;
    }

    private void ConfigureTriggerColumn()
        => this.olvColumnTrigger.AspectToStringConverter = delegate (object obj)
        {
            var trigger = obj as CoreTrigger;

            if (trigger == null)
            {
                return "(no trigger mapped)";
            }

            return trigger.ToString();
        };

    private void RefreshColumnWidths()
    {
        this.olvColumnAction.MaximumWidth = this.olvMappings.Width - this.olvColumnTrigger.Width - 25;
        this.olvColumnAction.Width = Math.Max(this.olvColumnAction.Width, this.olvColumnAction.MaximumWidth);
    }

    private void SetSelectedProfile(MappingProfile profile)
    {
        this.selectedProfile = profile;
        this.configState.LastLoadedProfile = profile.FilePath;

        this.RefreshMappings();
        this.olvMappings.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        this.olvMappings.Sort(this.olvColumnTrigger, SortOrder.Ascending);

        this.UpdateSelectedProfileName();
    }

    private void UpdateSelectedProfileName() => this.txtProfileName.Text = this.selectedProfile.Name;

    private void SetStatusView(bool isEnabled)
    {
        this.chkArmed.CheckedChanged -= this.ChkEnabled_CheckedChanged;
        this.chkArmed.Checked = isEnabled;
        this.chkArmed.CheckedChanged += this.ChkEnabled_CheckedChanged;

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
        this.chkArmed.Checked = false;
        MappingForm mappingForm = new(existingMappedOption);
        var result = mappingForm.ShowDialog();

        if (result == DialogResult.Cancel)
        {
            return;
        }

        if (existingMappedOption == null)
        {
            this.selectedProfile.AddMapping(mappingForm.MappedOption);
        }

        if (mappingForm.MappedOptionReverse != null)
        {
            this.selectedProfile.AddMapping(mappingForm.MappedOptionReverse);
        }

        this.selectedProfile.Save();
        this.RefreshMappings();
    }

    private void RemoveMappings(IList<MappedOption> mappedOptions)
    {
        var children = mappedOptions.SelectMany(x => x.Children).ToList();

        if (children.Any())
        {
            var introText = mappedOptions.Count == 1 ? "This mapped option has" : "These mapped options have a total of";
            var shouldRemove = MessageBox.Show(
                $"{introText} {children.Count} child mapping{(children.Count != 1 ? "s" : "")}. Do you want to remove them as well?",
                "Remove child mappings?",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes;

            if (shouldRemove)
            {
                foreach (var child in children)
                {
                    this.selectedProfile.RemoveMapping(child);
                }
            }
            else
            {
                // Otherwise remove their parent
                foreach (var child in children)
                {
                    child.SetParent(null);
                }
            }
        }

        foreach (var mappedOption in mappedOptions)
        {
            this.selectedProfile.RemoveMapping(mappedOption);
        }

        this.selectedProfile.Save();
        this.RefreshMappings();
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

        var mappedOptions = new List<MappedOption>();

        foreach (var item in this.olvMappings.SelectedObjects)
        {
            mappedOptions.Add(item as MappedOption);
        }

        this.RemoveMappings(mappedOptions);
        this.selectedProfile.Save();
    }

    private void MakeMappingParentless(MappedOption childOption)
    {
        childOption.SetParent(null);
        this.selectedProfile.Save();
        this.RefreshMappings();
    }

    private void ChooseNewParent(MappedOption child, MappedOption targetParent)
    {
        child.SetParent(targetParent);
        this.selectedProfile.Save();
        this.RefreshMappings();
        SystemSounds.Beep.Play();

        return;
    }

    public bool RunAppCommand(AppCommand command)
    {
        switch (command)
        {
            case AppCommand.Abort:
                this.BeginInvoke(new MethodInvoker(delegate
                {
                    this.chkArmed.Checked = false;
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

        this.RefreshColumnWidths();
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
        e.Parameters.ItemComparer = new MappingGroupItemComparer(
            e.Parameters.PrimarySort,
            e.Parameters.PrimarySortOrder
        );
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
        var builder = new MappingContextMenuBuilder(this.olvMappings.SelectedItems);
        builder.SelectEditMapping += (s, e) => this.EditMappedOption(e.MappedOption);
        builder.SelectMakeMappingParentless += (s, e) => this.MakeMappingParentless(e.MappedOption);
        builder.SelectChooseNewParent += (s, e) => this.ChooseNewParent(e.MappedOption, e.NewParent);
        builder.SelectMultiEditMapping += this.Builder_SelectMultiEditMapping;
        builder.SelectRemoveMappings += (s, e) =>
        {
            this.RemoveSelectedMappings();
            this.selectedProfile.Save();
        };
        e.MenuStrip = builder.Build();
    }

    private void Builder_SelectMultiEditMapping(object sender, SelectMultiEditMappingEventArgs e)
    {
        // Apply, save and refresh
        foreach (var mappingAspect in e.MappingAspects)
        {
            e.Property.SetValue(mappingAspect, e.Value);
        }

        this.selectedProfile.Save();
        this.RefreshMappings();
    }

    private void OlvMappings_KeyUp(object sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Delete)
        {
            return;
        }

        this.RemoveSelectedMappings();
    }

    private void OlvMappings_FormatRow(object sender, FormatRowEventArgs e)
    {
        if (e.Model is not MappedOption mappedOption)
        {
            return;
        }

        if (mappedOption.IsChild)
        {
            e.Item.CellPadding = new Rectangle(
                (e.ListView.CellPadding?.Left ?? 0) + 10,
                e.ListView.CellPadding?.Top ?? 0,
                e.ListView.CellPadding?.Right ?? 0,
                e.ListView.CellPadding?.Bottom ?? 0
            );
            e.Item.ForeColor = Color.Gray;
        }
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
        var isArmed = this.chkArmed.Checked;

        this.SetStatusView(isArmed);

        if (isArmed)
        {
            try
            {
                Key2JoyManager.Instance.ArmMappings(this.selectedProfile);
            }
            catch (MappingArmingFailedException ex)
            {
                this.chkArmed.Checked = false;
                MessageBox.Show(
                    this,
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
        else
        {
            Key2JoyManager.Instance.DisarmMappings();
        }

        this.deviceListControl.RefreshDevices();
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
            if (this.configState.ShouldCloseButtonMinimize)
            {
                e.Cancel = true;
                this.Hide();
                return;
            }
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

    private void SaveProfileToolStripMenuItem_Click(object sender, EventArgs e)
        => MessageBox.Show("When you make changes to a profile, changes are automatically saved. This button is only here to explain that feature to you.", "Profile already saved!", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
        range.AddRange(GamePadButtonAction.GetAllButtonActions(PressState.Press));
        range.AddRange(GamePadButtonAction.GetAllButtonActions(PressState.Release));

        this.selectedProfile.AddMappingRange(range);
        this.selectedProfile.Save();
        this.RefreshMappings();
    }

    private void GamePadPressToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var range = GamePadButtonAction.GetAllButtonActions(PressState.Press);
        this.selectedProfile.AddMappingRange(range);
        this.selectedProfile.Save();
        this.RefreshMappings();
    }

    private void GamePadReleaseToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var range = GamePadButtonAction.GetAllButtonActions(PressState.Release);
        this.selectedProfile.AddMappingRange(range);
        this.selectedProfile.Save();
        this.RefreshMappings();
    }

    private void KeyboardPressAndReleaseToolStripMenuItem_Click(object sender, EventArgs e)
    {
        List<MappedOption> range = new();
        range.AddRange(KeyboardAction.GetAllButtonActions(PressState.Press));
        range.AddRange(KeyboardAction.GetAllButtonActions(PressState.Release));

        this.selectedProfile.AddMappingRange(range);
        this.selectedProfile.Save();
        this.RefreshMappings();
    }

    private void KeyboardPressToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var range = KeyboardAction.GetAllButtonActions(PressState.Press);
        this.selectedProfile.AddMappingRange(range);
        this.selectedProfile.Save();
        this.RefreshMappings();
    }

    private void KeyboardReleaseToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var range = KeyboardAction.GetAllButtonActions(PressState.Release);
        this.selectedProfile.AddMappingRange(range);
        this.selectedProfile.Save();
        this.RefreshMappings();
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

    private void GenerateReverseMappingsToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var selectedCount = this.olvMappings.SelectedItems.Count;

        if (selectedCount == 0)
        {
            return;
        }

        if (selectedCount > 1
            && DialogUtilities.Confirm(
                $"Are you sure you want to create reverse mappings for all {selectedCount} selected mappings? Each type of action and trigger will configure their own useful reverse if possible.\n\n"
                + $"An example of an reverse mapping is how new 'Release' mappings will be created for each 'Press' and vice versa.",
                $"Generate {selectedCount} reverse mappings"
            ) == DialogResult.No)
        {
            return;
        }

        var selectedMappings = this.olvMappings.SelectedItems
            .Cast<OLVListItem>()
            .Select(item => (MappedOption)item.RowObject)
            .ToList();

        var newOptions = MappedOption.GenerateReverseMappings(selectedMappings);

        foreach (var option in newOptions)
        {
            this.selectedProfile.MappedOptions.Add(option);
        }

        this.selectedProfile.Save();
        this.RefreshMappings();
    }

    private void TxtFilter_TextChanged(object sender, EventArgs e)
        => this.olvMappings.ModelFilter = new ModelFilter(
            x =>
            {
                var mappedOption = (MappedOption)x;
                var filterText = this.txtFilter.Text;

                bool containsFilterText(string text)
                    => text.IndexOf(filterText, StringComparison.OrdinalIgnoreCase) > -1;

                var isMatch = containsFilterText(mappedOption.Action.ToString())
                           || containsFilterText(mappedOption.Trigger.ToString());

                return isMatch;
            }
        );

    private void MainForm_SizeChanged(object sender, EventArgs e)
        => this.RefreshColumnWidths();
}
