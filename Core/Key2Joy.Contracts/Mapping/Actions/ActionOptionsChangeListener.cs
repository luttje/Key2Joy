using System;

namespace Key2Joy.Contracts.Mapping.Actions
{
    /// <summary>
    /// This listener is used as a proxy, so we can listen to the event across the AppDomain boundary.
    /// It works because both AppDomains need to know the class in which the event is defined.
    /// 
    /// Source: https://stackoverflow.com/a/5871944
    /// </summary>
    public class ActionOptionsChangeListener : MarshalByRefObject
    {
        /// <summary>
        /// Called when the options on an action change
        /// </summary>
        public event EventHandler OptionsChanged;

        public ActionOptionsChangeListener(IActionOptionsControl optionsControl)
        {
            optionsControl.OptionsChanged += new EventHandler((sender, e) => OptionsChanged?.Invoke(this, EventArgs.Empty));
        }
    }
}
