using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using CommonServiceLocator;
using Key2Joy.Config;

namespace Key2Joy.Gui;

public partial class ConfigForm : Form
{
    private readonly ConfigState configState;

    public ConfigForm()
    {
        this.configState = ServiceLocator.Current
            .GetInstance<IConfigManager>()
            .GetConfigState();

        this.InitializeComponent();

        var configs = ConfigControlAttribute.GetAllProperties(typeof(ConfigState), new AttributeProvider());

        foreach (var kvp in configs)
        {
            var property = kvp.Key;
            var attribute = kvp.Value;

            Panel controlParent = new();
            var value = property.GetValue(this.configState);
            var control = this.MakeControl(attribute, value, controlParent);
            control.Tag = kvp;
            control.Dock = DockStyle.Bottom;

            controlParent.AutoSize = true;
            controlParent.Padding = new Padding(10, 10, 10, 0);
            controlParent.Controls.Add(control);

            this.pnlConfigurations.Controls.Add(controlParent);
            controlParent.Dock = DockStyle.Bottom;
        }

        this.PerformLayout();
    }

    private void BtnSave_Click(object sender, EventArgs e)
    {
        foreach (Panel parents in this.pnlConfigurations.Controls)
        {
            foreach (Control control in parents.Controls)
            {
                if (control.Tag == null)
                {
                    continue;
                }

                var kvp = (KeyValuePair<PropertyInfo, ConfigControlAttribute>)control.Tag;
                var property = kvp.Key;
                var attribute = kvp.Value;
                var value = this.GetControlValue(attribute, control);

                value = Convert.ChangeType(value, property.PropertyType);

                property.SetValue(this.configState, value);
            }
        }

        MessageBox.Show("Configurations successfully saved!", "Configurations saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
        this.Close();
    }

    private Control MakeControl(ConfigControlAttribute attribute, object value, Panel controlParent)
    {
        void CreateLabel()
        {
            Label label = new()
            {
                AutoSize = true,
                Dock = DockStyle.Bottom,
                Text = $"{attribute.Text}: "
            };
            controlParent.Controls.Add(label);
        }

        switch (attribute)
        {
            case BooleanConfigControlAttribute booleanConfigControlAttribute:
            {
                CheckBox control = new()
                {
                    Text = booleanConfigControlAttribute.Text,
                    Checked = (bool)value
                };

                return control;
            }
            case NumericConfigControlAttribute numericConfigControlAttribute:
            {
                CreateLabel();
                NumericUpDown control = new()
                {
                    Minimum = (decimal)numericConfigControlAttribute.Minimum,
                    Maximum = (decimal)numericConfigControlAttribute.Maximum,
                    Value = (decimal)Convert.ChangeType(value, typeof(decimal))
                };

                return control;
            }
            case TextConfigControlAttribute textConfigControlAttribute:
            {
                CreateLabel();
                TextBox control = new()
                {
                    Text = value.ToString(),
                    MaxLength = textConfigControlAttribute.MaxLength
                };

                return control;
            }
            case EnumConfigControlAttribute enumConfigControlAttribute:
            {
                CreateLabel();
                var enumValues = Enum.GetValues(enumConfigControlAttribute.EnumType);
                var selected = Enum.Parse(enumConfigControlAttribute.EnumType, value.ToString());
                ComboBox control = new()
                {
                    DropDownStyle = ComboBoxStyle.DropDownList,
                };

                foreach (var enumValue in enumValues)
                {
                    control.Items.Add(enumValue);
                }

                control.SelectedIndex = Array.IndexOf(enumValues, selected);

                return control;
            }

            default:
                break;
        }

        throw new NotImplementedException("ConfigControlAttribute type not implemented: " + attribute.GetType().Name);
    }

    private object GetControlValue(ConfigControlAttribute attribute, Control control)
    {
        switch (attribute)
        {
            case BooleanConfigControlAttribute:
            {
                var checkbox = (CheckBox)control;
                return checkbox.Checked;
            }
            case NumericConfigControlAttribute:
            {
                var numeric = (NumericUpDown)control;
                return numeric.Value;
            }
            case TextConfigControlAttribute:
            {
                var textbox = (TextBox)control;
                return textbox.Text;
            }
            case EnumConfigControlAttribute:
            {
                var combobox = (ComboBox)control;
                return combobox.SelectedItem;
            }

            default:
                break;
        }

        throw new NotImplementedException("ConfigControlAttribute type not implemented: " + attribute.GetType().Name);
    }
}
