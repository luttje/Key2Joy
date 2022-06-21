using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyToJoy.Mapping
{
    [AttributeUsage(AttributeTargets.Class)]
    internal abstract class MappingAttribute : Attribute
    {
        /// <summary>
        /// Name for the action (must be unique and constant)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Which UserControl to load into the MappingForm for the user to set options.
        /// </summary>
        public Type OptionsUserControl { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
