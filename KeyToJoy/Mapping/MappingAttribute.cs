using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyToJoy.Mapping
{
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class MappingAttribute : Attribute
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

        public override string ToString()
        {
            return Description;
        }
    }
}
