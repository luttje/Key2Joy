using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyToJoy.Mapping
{
    public class ActionAttribute : MappingAttribute
    {
        /// <summary>
        /// When this action should be visibile in menu's.
        /// </summary>
        public ActionVisibility Visibility { get; set; } = ActionVisibility.Always;
    }
}
