using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms;
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
}

public partial class PluginsForm : Form
{
    public PluginsForm()
    {
        this.InitializeComponent();

        /**
         * For some reason the designer fails with:
         * 'Error using the dropdown: Could not load file or assembly 'Key2Joy.Core, version=...'
         *
         * Additionally the designer seems to keep the copied dll in memory, so following builds fail.
         *
         * Therefor we do this manually:
         */
        this.dgvPlugins.AutoGenerateColumns = false;
        this.dgvPlugins.Columns.Add(new DataGridViewButtonColumn()
        {
            Name = "dgvColumnToggleEnable",
            HeaderText = "Enable/Disable",
            DataPropertyName = "LoadState",
        });
        this.dgvPlugins.Columns.Add(new DataGridViewColumn(new DataGridViewTextBoxCell())
        {
            Name = "dgvColumnLoaded",
            HeaderText = "",
            DataPropertyName = "LoadState",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader,
            ToolTipText = "Whether the plugin loaded successfully (✔), failed to load (⚠) or is disabled (✘).",
            FillWeight = 1,
        });
        this.dgvPlugins.Columns.Add(new DataGridViewColumn(new DataGridViewTextBoxCell())
        {
            Name = "dgvColumnName",
            HeaderText = "Name",
            DataPropertyName = "Name",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
        });
        this.dgvPlugins.Columns.Add(new DataGridViewColumn(new DataGridViewTextBoxCell())
        {
            Name = "dgvColumnAuthor",
            HeaderText = "Author",
            DataPropertyName = "Author",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
        });
        this.dgvPlugins.Columns.Add(new DataGridViewColumn(new DataGridViewTextBoxCell())
        {
            Name = "dgvColumnWebsite",
            HeaderText = "Website",
            DataPropertyName = "Website",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
        });
        this.dgvPlugins.Columns.Add(new DataGridViewColumn(new DataGridViewTextBoxCell())
        {
            Name = "dgvColumnPath",
            HeaderText = "Path",
            DataPropertyName = "AssemblyPath",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
        });

        this.dgvPlugins.CellFormatting += this.DgvPlugins_CellFormatting;

        this.RefreshPlugins();
    }

    private void RefreshPlugins() => this.dgvPlugins.DataSource = Program.Plugins.AllPluginLoadStates.Values.Select(
            pls => new PluginInfo(pls))
            .ToList();

    private void DgvPlugins_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
        var column = this.dgvPlugins.Columns[e.ColumnIndex];

        if (column.Name == "dgvColumnToggleEnable")
        {
            var pluginLoadState = (PluginLoadState)e.Value;
            e.Value = pluginLoadState.LoadState == PluginLoadStates.Loaded ? "Disable" : "Enable";
        }

        if (column.Name == "dgvColumnLoaded")
        {
            var pluginLoadState = (PluginLoadState)e.Value;
            e.Value = pluginLoadState.LoadState == PluginLoadStates.Loaded ? "✔" :
                (pluginLoadState.LoadState == PluginLoadStates.NotLoaded ? "✘" : "⚠");

            var cell = this.dgvPlugins.Rows[e.RowIndex].Cells[e.ColumnIndex];
            cell.ToolTipText = pluginLoadState.LoadErrorMessage;
        }

        if (column.Name is "dgvColumnActions" or "dgvColumnTriggers")
        {
            var types = (IReadOnlyList<Type>)e.Value;
            e.Value = types.Count.ToString();
        }
    }

    private void DgvPlugins_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
        var senderGrid = (DataGridView)sender;

        if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
            e.RowIndex >= 0)
        {
            var rowItem = senderGrid.Rows[e.RowIndex];
            var pluginInfo = (PluginInfo)rowItem.DataBoundItem;

            if (pluginInfo.LoadState.LoadState == PluginLoadStates.Loaded)
            {
                Program.Plugins.DisablePlugin(pluginInfo.AssemblyPath);
                System.Windows.Forms.MessageBox.Show(
                    "When disabling loaded plugins you have to restart the application for these changes to take effect."
                );
            }
            else
            {
                var pluginPermissionsWithDescriptions = PluginHost.PluginHost.GetAllowedPermissionsWithDescriptions();
                var permissionsXml = PluginHost.PluginHost.GetAdditionalPermissionsXml(pluginInfo.AssemblyPath);
                var allPermissionsGranted = true;

                if (permissionsXml != null)
                {
                    PermissionSet additionalPermissions = new(PermissionState.None);
                    additionalPermissions.FromXml(SecurityElement.FromString(permissionsXml));

                    if (additionalPermissions.Count > 0)
                    {
                        // Get all the indexes of pluginPermissionsWithDescriptions.AllowedPermissions that occur in additionalPermissions
                        var relevantDescriptions = new string[additionalPermissions.Count];

                        var index = 0;
                        foreach (var permission in pluginPermissionsWithDescriptions.AllowedPermissions)
                        {
                            foreach (var additionalPermission in additionalPermissions)
                            {
                                if (permission.Equals(additionalPermission))
                                {
                                    relevantDescriptions[index] = pluginPermissionsWithDescriptions.Descriptions[index];
                                    break;
                                }
                            }
                        }

                        PluginPermissionsForm confirmationDialog = new(relevantDescriptions);

                        if (confirmationDialog.ShowDialog() != DialogResult.OK)
                        {
                            allPermissionsGranted = false;
                        }
                    }
                }

                if (allPermissionsGranted)
                {
                    Program.Plugins.LoadPlugin(pluginInfo.AssemblyPath);
                    Program.Plugins.RefreshPluginTypes();
                }
            }

            this.RefreshPlugins();
        }
    }
}
