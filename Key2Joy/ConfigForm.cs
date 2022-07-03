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

                var control = Activator.CreateInstance(attribute.ControlType) as Control;
                control.Tag = kvp;
                control.Dock = DockStyle.Top;

                var controlParent = new Panel();
                controlParent.AutoSize = true;
                controlParent.Padding = new Padding(10, 10, 10, 0);
                controlParent.Controls.Add(control);

                if (control is TextBox
                    || control is NumericUpDown)
                {
                    var label = new Label();
                    label.AutoSize = true;
                    label.Dock = DockStyle.Top;
                    label.Text = $"{attribute.Text}: ";
                    controlParent.Controls.Add(label);

                    var value = property.GetValue(Config.Instance);
                    control.Text = value.ToString();
                    
                    if(control is NumericUpDown numericUpDown)
                    {
                        // TODO: Make it so the ConfigControlAttribute can configure this
                        numericUpDown.Minimum = 0;
                        numericUpDown.Maximum = 10000;
                    }
                }
                else if(control is CheckBox checkBox)
                {
                    control.Text = attribute.Text;

                    var value = property.GetValue(Config.Instance);
                    checkBox.Checked = (bool) value == true;
                }
                else throw new NotImplementedException("Config support not yet supported");

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
                    object value;

                    if (control is TextBox)
                        value = control.Text;
                    else if (control is NumericUpDown numericUpDown)
                        value = numericUpDown.Value;
                    else if (control is CheckBox checkBox)
                        value = checkBox.Checked;
                    else throw new NotImplementedException("Config support not yet supported");

                    value = Convert.ChangeType(value, property.PropertyType);

                    property.SetValue(Config.Instance, value);
                }
            }

            MessageBox.Show("Configurations successfully saved!", "Configurations saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }
    }
}
