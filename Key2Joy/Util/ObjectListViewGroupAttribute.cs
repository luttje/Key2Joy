using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Util
{
    [AttributeUsage(AttributeTargets.Class)]
    internal class ObjectListViewGroupAttribute : Attribute
    {
        /// <summary>
        /// Display name for the image group
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Resource name of the image
        /// </summary>
        public string Image { get; set; }
    }
}
