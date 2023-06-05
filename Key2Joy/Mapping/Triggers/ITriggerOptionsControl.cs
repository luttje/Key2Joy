using Key2Joy.Contracts.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Mapping
{
    public interface ITriggerOptionsControl
    {
        /// <summary>
        /// Called to setup the options panel with a trigger
        /// </summary>
        /// <param name="trigger"></param>
        void Select(AbstractTrigger trigger);

        /// <summary>
        /// Called when the options panel should modify a resulting trigger
        /// </summary>
        /// <param name="trigger"></param>
        void Setup(AbstractTrigger trigger);

        /// <summary>
        /// Called when the options on a trigger change
        /// </summary>
        event EventHandler OptionsChanged;
    }
}
