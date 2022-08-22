using Key2Joy.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Key2Joy
{
    public partial class ConfigForm : Form
    {
        public ConfigForm()
        {
            InitializeComponent();

            var configs = ConfigControlAttribute.GetAllProperties();

            foreach (var kvp in configs)
            {
                var property = kvp.Key;
                var attribute = kvp.Value;

                var controlParent = new Panel();
                var value = property.GetValue(ConfigManager.Instance);
                var control = attribute.MakeControl(value, controlParent);
                control.Tag = kvp;
                control.Dock = DockStyle.Top;

                controlParent.AutoSize = true;
                controlParent.Padding = new Padding(10, 10, 10, 0);
                controlParent.Controls.Add(control);

                pnlConfigurations.Controls.Add(controlParent);
                controlParent.Dock = DockStyle.Top;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            foreach (Panel parents in pnlConfigurations.Controls)
            {
                foreach (Control control in parents.Controls)
                {
                    if (control.Tag == null)
                        continue;

                    var kvp = (KeyValuePair<PropertyInfo, ConfigControlAttribute>)control.Tag;
                    var property = kvp.Key;
                    var attribute = kvp.Value;
                    var value = attribute.GetControlValue(control);

                    value = Convert.ChangeType(value, property.PropertyType);

                    property.SetValue(ConfigManager.Instance, value);
                }
            }

            MessageBox.Show("Configurations successfully saved!", "Configurations saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }
    }
}
