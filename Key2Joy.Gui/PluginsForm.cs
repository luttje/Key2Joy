using Key2Joy.Plugins;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

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

            AssemblyPath = LoadState.Assembly.Location;
            Name = LoadState.Plugin?.Name ?? "n/a";
            Author = LoadState.Plugin?.Author ?? "n/a";
            Website = LoadState.Plugin?.Website ?? "n/a";
            ActionTypes = LoadState.Plugin?.ActionTypes ?? new List<Type>();
            TriggerTypes = LoadState.Plugin?.TriggerTypes ?? new List<Type>();
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
            dgvPlugins.Columns.Add(new DataGridViewColumn(new DataGridViewTextBoxCell())
            {
                Name = "dgvColumnLoaded",
                HeaderText = "",
                DataPropertyName = "LoadState",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader,
                ToolTipText = "Whether the plugin loaded successfully (✔) or failed to load (✘).",
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
                Name = "dgvColumnActions",
                HeaderText = "Action Count",
                DataPropertyName = "ActionTypes",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
            });
            dgvPlugins.Columns.Add(new DataGridViewColumn(new DataGridViewTextBoxCell())
            {
                Name = "dgvColumnTriggers",
                HeaderText = "Trigger Count",
                DataPropertyName = "TriggerTypes",
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

            dgvPlugins.DataSource = Program.Plugins.AllPluginLoadStates.Select(
                pls => new PluginInfo(pls))
                .ToList();
        }

        private void DgvPlugins_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var column = dgvPlugins.Columns[e.ColumnIndex];
            
            if (column.Name == "dgvColumnLoaded")
            {
                var pluginLoadState = (PluginLoadState)e.Value;
                e.Value = pluginLoadState.LoadState == PluginLoadStates.Loaded ? "✔" : 
                    (pluginLoadState.LoadState == PluginLoadStates.NotLoaded ? " " : "✘");
            }

            if (column.Name == "dgvColumnActions" || column.Name == "dgvColumnTriggers")
            {
                var types = (IReadOnlyList<Type>)e.Value;
                e.Value = types.Count.ToString();
            }
        }
    }
}
