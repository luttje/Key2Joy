using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyToJoy.Mapping
{
    internal class ActionAttribute : MappingAttribute
    {
        /// <summary>
        /// String pointing to an optional image in resources
        /// </summary>
        public string ImagePath { get; set; } = null;
        
        /// <summary>
        /// Whether this action is visible in the top level UI. 
        /// 
        /// If false then you should provide access to it somewhere in a UserControl.
        /// </summary>
        public bool IsTopLevel { get; set; } = true;
    }
}
