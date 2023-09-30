using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Key2Joy.Gui
{
    public partial class PluginPermissionsForm : Form
    {
        private PluginPermissionsForm()
        {
            InitializeComponent();
        }
        
        public PluginPermissionsForm(string[] relevantPermissionDescriptions)
            :this()
        {
            pnlPermissions.Controls.Clear();

            foreach (var permissionDescription in relevantPermissionDescriptions)
            {
                var permissionControl = new PluginPermissionControl(permissionDescription)
                {
                    Dock = DockStyle.Top,
                    AutoSize = true,
                };
                pnlPermissions.Controls.Add(permissionControl);
            }

            pnlPermissions.Invalidate(true);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnEnablePlugin_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
