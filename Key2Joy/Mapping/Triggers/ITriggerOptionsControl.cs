using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Mapping
{
    internal interface ITriggerOptionsControl
    {
        /// <summary>
        /// Called to setup the options panel with a trigger
        /// </summary>
        /// <param name="trigger"></param>
        void Select(BaseTrigger trigger);

        /// <summary>
        /// Called when the options panel should modify a resulting trigger
        /// </summary>
        /// <param name="trigger"></param>
        void Setup(BaseTrigger trigger);

        /// <summary>
        /// Called when the options on a trigger change
        /// </summary>
        event EventHandler OptionsChanged;
    }
}
