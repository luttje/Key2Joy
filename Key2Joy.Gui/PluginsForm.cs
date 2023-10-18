using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms;
using Key2Joy.PluginHost;
using Key2Joy.Plugins;

namespace Key2Joy.Gui;

public struct PluginInfo
{
    public PluginLoadState LoadState { get; private set; }
    public string AssemblyPath { get; private set; }
    public string Name { get; private set; }
    public string Author { get; private set; }
    public string Website { get; private set; }
    public IReadOnlyList<Type> ActionTypes { get; private set; }
    public IReadOnlyList<Type> TriggerTypes { get; private set; }

    public PluginInfo(PluginLoadState loadState)
    {
        this.LoadState = loadState;

        this.AssemblyPath = this.LoadState.AssemblyPath;
        this.Name = this.LoadState.Name ?? "n/a";
        this.Author = this.LoadState.Author ?? "n/a";
        this.Website = this.LoadState.Website ?? "n/a";
        this.ActionTypes = new List<Type>();
        this.TriggerTypes = new List<Type>();
    }

    public static PluginInfo FromLoadState(PluginLoadState loadState)
        => new(loadState);
}

public partial class PluginsForm : Form
{
    public PluginsForm()
    {
        this.InitializeComponent();
        this.InitializeDgvPlugins();
        this.RefreshPlugins();
    }

    private void InitializeDgvPlugins()
    {
        this.dgvPlugins.AutoGenerateColumns = false;
        this.AddColumn("dgvColumnToggleEnable", "Enable/Disable", "LoadState", new DataGridViewButtonColumn());
        this.AddColumn("dgvColumnLoaded", "", "LoadState", null, DataGridViewAutoSizeColumnMode.AllCellsExceptHeader, "Whether the plugin loaded successfully (✔), failed to load (⚠) or is disabled (✘).");
        this.AddColumn("dgvColumnName", "Name", "Name");
        this.AddColumn("dgvColumnAuthor", "Author", "Author");
        this.AddColumn("dgvColumnWebsite", "Website", "Website");
        this.AddColumn("dgvColumnPath", "Path", "AssemblyPath", null, DataGridViewAutoSizeColumnMode.Fill);

        this.dgvPlugins.CellFormatting += this.DgvPlugins_CellFormatting;
        this.dgvPlugins.CellContentClick += this.DgvPlugins_CellContentClick;
    }

    private void DgvPlugins_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
        var column = this.dgvPlugins.Columns[e.ColumnIndex];

        if (column.Name == "dgvColumnToggleEnable")
        {
            this.FormatToggleEnableColumn(e);
        }

        if (column.Name == "dgvColumnLoaded")
        {
            this.FormatLoadedColumn(e);
        }

        if (column.Name is "dgvColumnActions" or "dgvColumnTriggers")
        {
            this.FormatActionsOrTriggersColumn(e);
        }
    }

    private void DgvPlugins_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
        var senderGrid = (DataGridView)sender;

        if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
        {
            this.HandlePluginToggleAction(senderGrid.Rows[e.RowIndex]);
        }
    }

    private void AddColumn(
        string name,
        string headerText,
        string dataPropertyName,
        DataGridViewColumn column = null,
        DataGridViewAutoSizeColumnMode? autoSizeMode = null,
        string toolTipText = null
    )
    {
        column ??= new DataGridViewColumn(new DataGridViewTextBoxCell());
        column.Name = name;
        column.HeaderText = headerText;
        column.DataPropertyName = dataPropertyName;

        if (autoSizeMode.HasValue)
        {
            column.AutoSizeMode = autoSizeMode.Value;
        }

        if (!string.IsNullOrEmpty(toolTipText))
        {
            column.ToolTipText = toolTipText;
        }

        this.dgvPlugins.Columns.Add(column);
    }

    private void RefreshPlugins()
        => this.dgvPlugins.DataSource = Program.Plugins.AllPluginLoadStates.Values
            .Select(PluginInfo.FromLoadState)
            .ToList();

    private void FormatToggleEnableColumn(DataGridViewCellFormattingEventArgs e)
    {
        var pluginLoadState = (PluginLoadState)e.Value;
        e.Value = pluginLoadState.LoadState == PluginLoadStates.Loaded ? "Disable" : "Enable";
    }

    private void FormatLoadedColumn(DataGridViewCellFormattingEventArgs e)
    {
        var pluginLoadState = (PluginLoadState)e.Value;
        e.Value = pluginLoadState.LoadState == PluginLoadStates.Loaded ? "✔" :
            (pluginLoadState.LoadState == PluginLoadStates.NotLoaded ? "✘" : "⚠");

        var cell = this.dgvPlugins.Rows[e.RowIndex].Cells[e.ColumnIndex];
        cell.ToolTipText = pluginLoadState.LoadErrorMessage;
    }

    private void FormatActionsOrTriggersColumn(DataGridViewCellFormattingEventArgs e)
    {
        var types = (IReadOnlyList<Type>)e.Value;
        e.Value = types.Count.ToString();
    }

    private void HandlePluginToggleAction(DataGridViewRow rowItem)
    {
        var pluginInfo = (PluginInfo)rowItem.DataBoundItem;

        if (pluginInfo.LoadState.LoadState == PluginLoadStates.Loaded)
        {
            this.DisablePlugin(pluginInfo.AssemblyPath);
        }
        else
        {
            this.LoadOrRequestPermission(pluginInfo);
        }

        this.RefreshPlugins();
    }

    private void DisablePlugin(string assemblyPath)
    {
        Program.Plugins.DisablePlugin(assemblyPath);
        MessageBox.Show("When disabling loaded plugins you have to restart the application for these changes to take effect.");
    }

    private void LoadOrRequestPermission(PluginInfo pluginInfo)
    {
        var pluginPermissionsWithDescriptions = AllowedPermissionsWithDescriptions
                .GetAllowedPermissionsWithDescriptions();
        var permissionsXml = PluginHost.PluginHost.GetAdditionalPermissionsXml(pluginInfo.AssemblyPath);
        var allPermissionsGranted = true;

        if (permissionsXml != null)
        {
            allPermissionsGranted = this.CheckAndPromptForPermissions(permissionsXml, pluginPermissionsWithDescriptions);
        }

        if (allPermissionsGranted)
        {
            Program.Plugins.LoadPlugin(pluginInfo.AssemblyPath);
            Program.Plugins.RefreshPluginTypes();
        }
    }

    private bool CheckAndPromptForPermissions(string permissionsXml, AllowedPermissionsWithDescriptions pluginPermissionsWithDescriptions)
    {
        PermissionSet additionalPermissions = new(PermissionState.None);
        additionalPermissions.FromXml(SecurityElement.FromString(permissionsXml));

        if (additionalPermissions.Count > 0)
        {
            return this.PromptForAdditionalPermissions(pluginPermissionsWithDescriptions, additionalPermissions);
        }

        return true;
    }

    private bool PromptForAdditionalPermissions(AllowedPermissionsWithDescriptions descriptions, PermissionSet additionalPermissions)
    {
        var relevantDescriptions = AllowedPermissionsWithDescriptions.GetRelevantPermissions(descriptions, additionalPermissions);
        PluginPermissionsForm confirmationDialog = new(relevantDescriptions);
        return confirmationDialog.ShowDialog() == DialogResult.OK;
    }
}
