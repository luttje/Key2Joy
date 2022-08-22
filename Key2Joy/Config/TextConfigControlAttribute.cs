using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Key2Joy.Config
{
    /// <summary>
    /// Only applied to <see cref="ConfigManager"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class TextConfigControlAttribute : ConfigControlAttribute
    {
        public int MaxLength { get; set; }

        public override object GetControlValue(Control control)
        {
            return (control as TextBox).Text;
        }

        public override Control MakeControl(object value, Control controlParent)
        {
            var label = new Label();
            label.AutoSize = true;
            label.Dock = DockStyle.Top;
            label.Text = $"{Text}: ";
            controlParent.Controls.Add(label);

            var control = new TextBox();
            control.Text = value.ToString();
            control.MaxLength = MaxLength;
            
            return control;
        }
    }
}
