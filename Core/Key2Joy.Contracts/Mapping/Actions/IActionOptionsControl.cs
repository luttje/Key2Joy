using System;

namespace Key2Joy.Contracts.Mapping
{
    public interface IActionOptionsControl
    {
        /// <summary>
        /// Called to setup the options panel with a action
        /// </summary>
        /// <param name="action"></param>
        void Select(object action);

        /// <summary>
        /// Called when the options panel should modify a resulting action
        /// </summary>
        /// <param name="action"></param>
        void Setup(object action);

        /// <summary>
        /// Called when the mapping is saving and can still be stopped
        /// </summary>
        bool CanMappingSave(object action);

        /// <summary>
        /// Called when the options on an action change
        /// </summary>
        event EventHandler OptionsChanged;
    }
}
