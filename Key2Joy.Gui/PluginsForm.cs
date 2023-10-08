using Key2Joy.Plugins;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms;

namespace Key2Joy.Gui
{
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
            LoadState = loadState;

            AssemblyPath = LoadState.AssemblyPath;
            Name = LoadState.Name ?? "n/a";
            Author = LoadState.Author ?? "n/a";
            Website = LoadState.Website ?? "n/a";
            ActionTypes = new List<Type>();
            TriggerTypes = new List<Type>();
        }
    }

    public partial class PluginsForm : Form
    {
        public PluginsForm()
        {
            InitializeComponent();

            /**
             * For some reason the designer fails with:
             * 'Error using the dropdown: Could not load file or assembly 'Key2Joy.Core, version=...'
             * 
             * Additionally the designer seems to keep the copied dll in memory, so following builds fail.
             * 
             * Therefor we do this manually:
             */
            dgvPlugins.AutoGenerateColumns = false;
            dgvPlugins.Columns.Add(new DataGridViewButtonColumn()
            {
                Name = "dgvColumnToggleEnable",
                HeaderText = "Enable/Disable",
                DataPropertyName = "LoadState",
            });
            dgvPlugins.Columns.Add(new DataGridViewColumn(new DataGridViewTextBoxCell())
            {
                Name = "dgvColumnLoaded",
                HeaderText = "",
                DataPropertyName = "LoadState",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader,
                ToolTipText = "Whether the plugin loaded successfully (✔), failed to load (⚠) or is disabled (✘).",
                FillWeight = 1,
            });
            dgvPlugins.Columns.Add(new DataGridViewColumn(new DataGridViewTextBoxCell())
            {
                Name = "dgvColumnName",
                HeaderText = "Name",
                DataPropertyName = "Name",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
            });
            dgvPlugins.Columns.Add(new DataGridViewColumn(new DataGridViewTextBoxCell())
            {
                Name = "dgvColumnAuthor",
                HeaderText = "Author",
                DataPropertyName = "Author",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
            });
            dgvPlugins.Columns.Add(new DataGridViewColumn(new DataGridViewTextBoxCell())
            {
                Name = "dgvColumnWebsite",
                HeaderText = "Website",
                DataPropertyName = "Website",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
            });
            dgvPlugins.Columns.Add(new DataGridViewColumn(new DataGridViewTextBoxCell())
            {
                Name = "dgvColumnPath",
                HeaderText = "Path",
                DataPropertyName = "AssemblyPath",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            });

            dgvPlugins.CellFormatting += DgvPlugins_CellFormatting;

            RefreshPlugins();
        }

        private void RefreshPlugins()
        {
            dgvPlugins.DataSource = Program.Plugins.AllPluginLoadStates.Values.Select(
                pls => new PluginInfo(pls))
                .ToList();
        }

        private void DgvPlugins_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var column = dgvPlugins.Columns[e.ColumnIndex];

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

                var cell = dgvPlugins.Rows[e.RowIndex].Cells[e.ColumnIndex];
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
                    }
                }

                RefreshPlugins();
            }
        }
    }
}
