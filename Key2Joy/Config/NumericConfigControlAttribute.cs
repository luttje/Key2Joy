using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Key2Joy.Config
{
    /// <summary>
    /// Only applied to <see cref="ConfigManager"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class NumericConfigControlAttribute : ConfigControlAttribute
    {
        public double Minimum { get; set; }
        public double Maximum { get; set; }

        public override object GetControlValue(Control control)
        {
            return (control as NumericUpDown).Value;
        }

        public override Control MakeControl(object value, Control controlParent)
        {
            var label = new Label();
            label.AutoSize = true;
            label.Dock = DockStyle.Top;
            label.Text = $"{Text}: ";
            controlParent.Controls.Add(label);

            var control = new NumericUpDown();
            control.Minimum = (decimal)Minimum;
            control.Maximum = (decimal)Maximum;
            control.Value = (decimal)Convert.ChangeType(value, typeof(decimal));

            return control;
        }
    }
}
