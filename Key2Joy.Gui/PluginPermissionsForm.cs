using System;
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
            : this()
        {
            pnlPermissions.Controls.Clear();

            foreach (var permissionDescription in relevantPermissionDescriptions)
            {
                PluginPermissionControl permissionControl = new(permissionDescription)
                {
                    Dock = DockStyle.Top,
                    AutoSize = true,
                };
                pnlPermissions.Controls.Add(permissionControl);
            }

            pnlPermissions.Invalidate(true);
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void BtnEnablePlugin_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
