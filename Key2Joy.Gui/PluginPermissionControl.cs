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
    public partial class PluginPermissionControl : UserControl
    {
        public PluginPermissionControl()
        {
            InitializeComponent();
        }
        
        public PluginPermissionControl(string permissionDescription)
            :this()
        {
            lblPermission.Text = permissionDescription;
        }
    }
}
