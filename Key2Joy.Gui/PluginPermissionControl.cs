using System.Windows.Forms;

namespace Key2Joy.Gui;

public partial class PluginPermissionControl : UserControl
{
    public PluginPermissionControl() => this.InitializeComponent();

    public PluginPermissionControl(string permissionDescription)
        : this() => this.lblPermission.Text = permissionDescription;
}
