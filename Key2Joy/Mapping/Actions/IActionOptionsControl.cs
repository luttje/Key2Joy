using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Mapping
{
    internal interface IActionOptionsControl
    {
        /// <summary>
        /// Called to setup the options panel with a action
        /// </summary>
        /// <param name="action"></param>
        void Select(BaseAction action);

        /// <summary>
        /// Called when the options panel should modify a resulting action
        /// </summary>
        /// <param name="action"></param>
        void Setup(BaseAction action);


        /// <summary>
        /// Called when the options on an action change
        /// </summary>
        event Action OptionsChanged;
    }
}
