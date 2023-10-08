using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Plugins;

namespace Key2Joy.Mapping.Actions;

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
                        .Where(m => m.GetCustomAttributes(typeof(ExposesScriptingMethodAttribute), false).Length > 0)
                        .SelectMany(m => m.GetCustomAttributes<ExposesScriptingMethodAttribute>()
                            .Select(a => MethodInfoToTypeExposed(m, a))
                        )
                )
            );

        if (additionalActionTypeFactories == null)
        {
            return;
        }

        foreach (var actionFactory in additionalActionTypeFactories)
        {
            if (actions.ContainsKey(actionFactory.FullTypeName))
            {
                Console.WriteLine("Action {0} already exists in the action buffer. Overwriting.", actionFactory.FullTypeName);
            }

            actions.Add(actionFactory.FullTypeName, actionFactory);
        }
    }

    /// <summary>
    /// Gets all action type factories
    /// </summary>
    /// <returns></returns>
    public static IDictionary<string, MappingTypeFactory<AbstractAction>> GetAllActions() => actions;

    /// <summary>
    /// Gets all action attributes
    /// </summary>
    /// <returns></returns>
    public static IList<ActionAttribute> GetAllActionAttributes() => actions.Values.Select(x => x.Attribute as ActionAttribute).ToList();

    /// <summary>
    /// Gets the attribute for the provided action
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public static ActionAttribute GetAttributeForAction(AbstractAction action)
    {
        var realTypeName = MappingTypeHelper.GetTypeFullName(actions, action);
        realTypeName = MappingTypeHelper.EnsureSimpleTypeName(realTypeName);
        return actions[realTypeName].Attribute as ActionAttribute;
    }

    /// <summary>
    /// Gets a specific action factory by its type
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    internal static MappingTypeFactory<AbstractAction> GetAction(Type type) => actions[type.FullName];

    /// <summary>
    /// Gets all action types and their attribute annotations depending on the specified visibility
    /// </summary>
    /// <param name="forTopLevel"></param>
    /// <returns></returns>
    public static SortedDictionary<ActionAttribute, MappingTypeFactory<AbstractAction>> GetAllActions(bool forTopLevel) => new SortedDictionary<ActionAttribute, MappingTypeFactory<AbstractAction>>(
            actions
                .Where(kvp =>
                {
                    if (kvp.Value.Attribute is not ActionAttribute actionAttribute
                    || actionAttribute.Visibility == MappingMenuVisibility.Never)
                    {
                        return false;
                    }

                    if (forTopLevel)
                    {
                        return actionAttribute.Visibility is MappingMenuVisibility.Always
                            or MappingMenuVisibility.OnlyTopLevel;
                    }

                    return actionAttribute.Visibility is MappingMenuVisibility.Always or MappingMenuVisibility.UnlessTopLevel;
                })
                .ToDictionary(t => t.Value.Attribute as ActionAttribute, t => t.Value)
            );
}
