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

namespace Key2Joy.Gui
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
                var control = MakeControl(attribute, value, controlParent);
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
                    var value = GetControlValue(attribute, control);

                    value = Convert.ChangeType(value, property.PropertyType);

                    property.SetValue(ConfigManager.Instance, value);
                }
            }

            MessageBox.Show("Configurations successfully saved!", "Configurations saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }

        private Control MakeControl(ConfigControlAttribute attribute, object value, Panel controlParent)
        {
            switch (attribute)
            {
                case BooleanConfigControlAttribute booleanConfigControlAttribute:
                    {
                        var control = new CheckBox();
                        control.Text = booleanConfigControlAttribute.Text;
                        control.Checked = (bool)value == true;

                        return control;
                    }
                case NumericConfigControlAttribute numericConfigControlAttribute:
                    {
                        var label = new Label();
                        label.AutoSize = true;
                        label.Dock = DockStyle.Top;
                        label.Text = $"{Text}: ";
                        controlParent.Controls.Add(label);

                        var control = new NumericUpDown();
                        control.Minimum = (decimal)numericConfigControlAttribute.Minimum;
                        control.Maximum = (decimal)numericConfigControlAttribute.Maximum;
                        control.Value = (decimal)Convert.ChangeType(value, typeof(decimal));

                        return control;
                    }
                case TextConfigControlAttribute textConfigControlAttribute:
                    {
                        var label = new Label();
                        label.AutoSize = true;
                        label.Dock = DockStyle.Top;
                        label.Text = $"{Text}: ";
                        controlParent.Controls.Add(label);

                        var control = new TextBox();
                        control.Text = value.ToString();
                        control.MaxLength = textConfigControlAttribute.MaxLength;

                        return control;
                    }
            }

            throw new NotImplementedException("ConfigControlAttribute type not implemented: " + attribute.GetType().Name);
        }

        private object GetControlValue(ConfigControlAttribute attribute, Control control)
        {
            switch (attribute)
            {
                case BooleanConfigControlAttribute booleanConfigControlAttribute:
                    {
                        var checkbox = (CheckBox)control;
                        return checkbox.Checked;
                    }
                case NumericConfigControlAttribute numericConfigControlAttribute:
                    {
                        var numeric = (NumericUpDown)control;
                        return numeric.Value;
                    }
                case TextConfigControlAttribute textConfigControlAttribute:
                    {
                        var textbox = (TextBox)control;
                        return textbox.Text;
                    }
            }

            throw new NotImplementedException("ConfigControlAttribute type not implemented: " + attribute.GetType().Name);
        }
    }
}
