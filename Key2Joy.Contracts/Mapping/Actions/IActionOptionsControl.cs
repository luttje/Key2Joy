using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Contracts.Mapping
{
    public interface IActionOptionsControl
    {
        /// <summary>
        /// Called to setup the options panel with a action
        /// </summary>
        /// <param name="action"></param>
        void Select(AbstractAction action);

        /// <summary>
        /// Called when the options panel should modify a resulting action
        /// </summary>
        /// <param name="action"></param>
        void Setup(AbstractAction action);

        /// <summary>
        /// Called when the mapping is saving and can still be stopped
        /// </summary>
        bool CanMappingSave(AbstractAction action);

        /// <summary>
        /// Called when the options on an action change
        /// </summary>
        event EventHandler OptionsChanged;
    }
}
