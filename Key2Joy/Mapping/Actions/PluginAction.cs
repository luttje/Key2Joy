using Key2Joy.Contracts.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Mapping
{
    /// <summary>
    /// Wraps a plugin action to become fully formed.
    /// </summary>
    public class PluginAction : CoreAction
    {
        protected AbstractAction action;

        internal PluginAction(AbstractAction action)
            :base(action.Name)
        {
            this.action = action;
        }

        public static CoreAction GetFullyFormedAction(AbstractAction action)
        {
            if (action is CoreAction)
                return (CoreAction)action;

            return new PluginAction(action);
        }

        public override Task Execute(AbstractInputBag inputBag = null)
        {
            return action.Execute(inputBag);
        }

        public override string GetNameDisplay()
        {
            return action.GetNameDisplay();
        }
    }
}
