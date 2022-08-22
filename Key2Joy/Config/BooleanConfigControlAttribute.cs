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
    public class BooleanConfigControlAttribute : ConfigControlAttribute
    {
        public Type ControlType => typeof(System.Windows.Forms.CheckBox);

        public override object GetControlValue(Control control)
        {
            return (control as CheckBox).Checked;
        }

        public override Control MakeControl(object value, Control controlParent)
        {
            var control = new CheckBox();
            control.Text = Text;
            control.Checked = (bool)value == true;
            
            return control;
        }
    }
}
