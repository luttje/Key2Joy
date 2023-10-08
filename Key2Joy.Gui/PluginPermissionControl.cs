using System.Windows.Forms;

namespace Key2Joy.Gui
{
    public partial class PluginPermissionControl : UserControl
    {
        public PluginPermissionControl()
        {
            InitializeComponent();
        }

        public PluginPermissionControl(string permissionDescription)
            : this()
        {
            lblPermission.Text = permissionDescription;
        }
    }
}
