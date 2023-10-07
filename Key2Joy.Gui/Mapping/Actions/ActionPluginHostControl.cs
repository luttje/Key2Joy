using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Plugins;
using Key2Joy.Plugins;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace Key2Joy.Gui.Mapping
{
    public partial class ActionPluginHostControl : UserControl, IActionOptionsControl
    {
        public event EventHandler OptionsChanged;
        
        private IActionOptionsControl pluginControlWithOptions;

        public ActionPluginHostControl()
        {
            InitializeComponent();
        }

        public ActionPluginHostControl(ElementHostProxy pluginUserControl)
            : this()
        {
            this.pluginControlWithOptions = pluginUserControl;

            Padding = new Padding(0, 5, 0, 5);
            var desiredHeight = 50; // TODO: mappingControl.DesiredHeight
            Height = desiredHeight + this.Padding.Vertical;

            this.Controls.Add(pluginUserControl);
            pluginUserControl.Dock = DockStyle.Fill;
            pluginUserControl.PerformLayout();

            var listener = new ActionOptionsChangeListener(pluginControlWithOptions);
            listener.OptionsChanged += (s, e) => 
                OptionsChanged?.Invoke(s, e);
        }

        public bool CanMappingSave(object action)
        {
            return pluginControlWithOptions.CanMappingSave(action);
        }

        public void Select(object action)
        {
            pluginControlWithOptions.Select(action);
        }

        public void Setup(object action)
        {
            pluginControlWithOptions.Setup(action);
        }
    }
}
