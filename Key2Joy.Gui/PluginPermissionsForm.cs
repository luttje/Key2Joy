using System;
using System.Windows.Forms;

namespace Key2Joy.Gui
{
    public partial class PluginPermissionsForm : Form
    {
        private PluginPermissionsForm()
        {
            this.InitializeComponent();
        }

        public PluginPermissionsForm(string[] relevantPermissionDescriptions)
            : this()
        {
            this.pnlPermissions.Controls.Clear();

            foreach (var permissionDescription in relevantPermissionDescriptions)
            {
                PluginPermissionControl permissionControl = new(permissionDescription)
                {
                    Dock = DockStyle.Top,
                    AutoSize = true,
                };
                this.pnlPermissions.Controls.Add(permissionControl);
            }

            this.pnlPermissions.Invalidate(true);
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void BtnEnablePlugin_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
