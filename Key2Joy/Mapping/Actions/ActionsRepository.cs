using Key2Joy.Contracts.Mapping;
using Key2Joy.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Mapping
{
    public static class ActionsRepository
    {
        private static Dictionary<string, MappingTypeFactory<AbstractAction>> actions;

        /// <summary>
        /// Loads all actions in the assembly, optionally merging it with additional action types.
        /// </summary>
        /// <param name="additionalActionTypeFactories"></param>
        public static void Buffer(IReadOnlyList<MappingTypeFactory<AbstractAction>> additionalActionTypeFactories = null)
        {
            static TypeExposedMethod MethodInfoToTypeExposed(MethodInfo m, ExposesScriptingMethodAttribute a)
            {
                return new TypeExposedMethod(a.FunctionName, m.Name, m.DeclaringType);
            }

            actions = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.GetCustomAttribute(typeof(ActionAttribute), false) != null)
                .ToDictionary(
                    t => t.FullName,
                    t => new MappingTypeFactory<AbstractAction>(
                        t.FullName,
                        t.GetCustomAttribute<ActionAttribute>(),
                        t.GetMethods()
                            .Where(m => m.GetCustomAttribute(typeof(ExposesScriptingMethodAttribute), false) != null)
                            .SelectMany(m => m.GetCustomAttributes<ExposesScriptingMethodAttribute>()
                                .Select(a => MethodInfoToTypeExposed(m, a))
                            )
                    )
                );

            if (additionalActionTypeFactories == null)
            {
                return;
            }

            foreach (var actionTypeFactory in additionalActionTypeFactories)
            {
                if (actions.ContainsKey(actionTypeFactory.FullTypeName))
                {
                    Console.WriteLine("Action {0} already exists in the action buffer. Overwriting.", actionTypeFactory.FullTypeName);
                }

                actions.Add(actionTypeFactory.FullTypeName, actionTypeFactory);
            }
        }

        /// <summary>
        /// Gets all action types and their attribute annotations
        /// </summary>
        /// <param name="forTopLevel"></param>
        /// <returns></returns>
        public static Dictionary<string, MappingTypeFactory<AbstractAction>> GetAllActions()
        {
            return actions;
        }

        /// <summary>
        /// Gets a specific action factory by its type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal static MappingTypeFactory<AbstractAction> GetAction(Type type)
        {
            return actions[type.FullName];
        }

        /// <summary>
        /// Gets all action types and their attribute annotations depending on the specified visibility
        /// </summary>
        /// <param name="forTopLevel"></param>
        /// <returns></returns>
        public static SortedDictionary<ActionAttribute, MappingTypeFactory<AbstractAction>> GetAllActions(bool forTopLevel)
        {
            return new SortedDictionary<ActionAttribute, MappingTypeFactory<AbstractAction>>(
                actions
                    .Where(kvp =>
                    {
                        var actionAttribute = kvp.Value.Attribute as ActionAttribute;

                        if (actionAttribute == null
                        || actionAttribute.Visibility == MappingMenuVisibility.Never)
                            return false;

                        if (forTopLevel)
                            return actionAttribute.Visibility == MappingMenuVisibility.Always
                                || actionAttribute.Visibility == MappingMenuVisibility.OnlyTopLevel;

                        return actionAttribute.Visibility == MappingMenuVisibility.Always || actionAttribute.Visibility == MappingMenuVisibility.UnlessTopLevel;
                    })
                    .ToDictionary(t => t.Value.Attribute as ActionAttribute, t => t.Value)
                );
        }
    }
}
