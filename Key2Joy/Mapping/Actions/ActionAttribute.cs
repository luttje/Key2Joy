using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Mapping
{
    public class ActionAttribute : MappingAttribute
    {
        private static Dictionary<Type, ActionAttribute> actions;
        
        /// <summary>
        /// Loads all actions in the assembly, optionally merging it with additional action types.
        /// </summary>
        /// <param name="additionalActions"></param>
        public static void BufferActions(IReadOnlyList<Type> additionalActions = null)
        {
            actions = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.GetCustomAttribute(typeof(ActionAttribute), false) != null)
                .ToDictionary(t => t, t => t.GetCustomAttribute(typeof(ActionAttribute), false) as ActionAttribute);

            if(additionalActions == null)
            {
                return;
            }

            foreach (var action in additionalActions)
            {
                if (actions.ContainsKey(action))
                {
                    Console.WriteLine("Action {0} already exists in the action buffer. Overwriting.", action.Name);
                }

                actions.Add(action, action.GetCustomAttribute(typeof(ActionAttribute), false) as ActionAttribute);
            }
        }
        
        /// <summary>
        /// Gets all action types and their attribute annotations
        /// </summary>
        /// <param name="forTopLevel"></param>
        /// <returns></returns>
        public static Dictionary<Type, ActionAttribute> GetAllActions()
        {
            return actions;
        }
        
        /// <summary>
        /// Gets all action types and their attribute annotations depending on the specified visibility
        /// </summary>
        /// <param name="forTopLevel"></param>
        /// <returns></returns>
        public static SortedDictionary<ActionAttribute, Type> GetAllActions(bool forTopLevel)
        {
            return new SortedDictionary<ActionAttribute, Type>(
                actions
                    .Where(kvp =>
                    {
                        var actionAttribute = kvp.Value;

                        if (actionAttribute == null
                        || actionAttribute.Visibility == MappingMenuVisibility.Never)
                            return false;

                        if (forTopLevel)
                            return actionAttribute.Visibility == MappingMenuVisibility.Always
                                || actionAttribute.Visibility == MappingMenuVisibility.OnlyTopLevel;

                        return actionAttribute.Visibility == MappingMenuVisibility.Always || actionAttribute.Visibility == MappingMenuVisibility.UnlessTopLevel;
                    })
                    .ToDictionary(t => t.Value, t => t.Key)
                );
        }
    }
}
