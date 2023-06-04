using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Plugins
{
    public abstract class Plugin
    {
        private List<Type> actionTypes;
        public IReadOnlyList<Type> ActionTypes => actionTypes;
        
        private List<Type> triggerTypes;
        public IReadOnlyList<Type> TriggerTypes => triggerTypes;

        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract string Author { get; }
        public abstract string Website { get; }

        public Plugin()
        {
            actionTypes = new();
            triggerTypes = new();
        }

        internal void AddActionType(Type type)
        {
            actionTypes.Add(type);
        }

        internal void AddTriggerType(Type type)
        {
            triggerTypes.Add(type);
        }
    }
}
