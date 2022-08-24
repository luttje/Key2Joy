using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Mapping
{
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class MappingAttribute : Attribute, IComparable<MappingAttribute>
    {
        /// <summary>
        /// Customizable name format for the action/trigger
        /// </summary>
        public string NameFormat { get; set; }

        /// <summary>
        /// Description for the action/trigger
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Which UserControl to load into the MappingForm for the user to set options.
        /// </summary>
        public Type OptionsUserControl { get; set; }

        /// <summary>
        /// Which parameters to provide to the constructor of the user control
        /// </summary>
        public object[] OptionsUserControlParams { get; set; }

        /// <summary>
        /// When this action should be visibile in menu's.
        /// </summary>
        public MappingMenuVisibility Visibility { get; set; } = MappingMenuVisibility.Always;

        public override string ToString()
        {
            return Description;
        }

        public override int GetHashCode()
        {
            return Description.GetHashCode();
        }

        public int CompareTo(MappingAttribute other)
        {
            return Description.CompareTo(other.Description);
        }
    }
}
