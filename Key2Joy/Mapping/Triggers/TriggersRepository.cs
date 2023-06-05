using Jint;
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
    public class TriggersRepository
    {
        private static Dictionary<string, MappingTypeFactory<AbstractTrigger>> triggers;

        /// <summary>
        /// Loads all triggers in the assembly, optionally merging it with additional trigger types.
        /// </summary>
        /// <param name="additionalTriggerTypeFactories"></param>
        public static void Buffer(IReadOnlyList<MappingTypeFactory<AbstractTrigger>> additionalTriggerTypeFactories = null)
        {
            triggers = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.GetCustomAttribute(typeof(TriggerAttribute), false) != null)
                .ToDictionary(
                    t => t.FullName,
                    t => new MappingTypeFactory<AbstractTrigger>(t.FullName, (MappingAttribute)t.GetCustomAttribute<TriggerAttribute>())
                );

            if (additionalTriggerTypeFactories == null)
            {
                return;
            }

            foreach (var triggerFactory in additionalTriggerTypeFactories)
            {
                if (triggers.ContainsKey(triggerFactory.FullTypeName))
                {
                    Console.WriteLine("Trigger {0} already exists in the trigger buffer. Overwriting.", triggerFactory.FullTypeName);
                }

                triggers.Add(triggerFactory.FullTypeName, triggerFactory);
            }
        }

        /// <summary>
        /// Gets all trigger types and their attribute annotations
        /// </summary>
        /// <param name="forTopLevel"></param>
        /// <returns></returns>
        public static Dictionary<string, MappingTypeFactory<AbstractTrigger>> GetAllTriggers()
        {
            return triggers;
        }

        /// <summary>
        /// Gets all trigger types and their attribute annotations depending on the specified visibility
        /// </summary>
        /// <param name="forTopLevel"></param>
        /// <returns></returns>
        public static SortedDictionary<TriggerAttribute, MappingTypeFactory<AbstractTrigger>> GetAllTriggers(bool forTopLevel)
        {
            return new SortedDictionary<TriggerAttribute, MappingTypeFactory<AbstractTrigger>>(
                triggers
                    .Where(kvp =>
                    {
                        var triggerAttribute = kvp.Value.Attribute as TriggerAttribute;

                        if (triggerAttribute == null
                        || triggerAttribute.Visibility == MappingMenuVisibility.Never)
                            return false;

                        if (forTopLevel)
                            return triggerAttribute.Visibility == MappingMenuVisibility.Always
                                || triggerAttribute.Visibility == MappingMenuVisibility.OnlyTopLevel;

                        return triggerAttribute.Visibility == MappingMenuVisibility.Always || triggerAttribute.Visibility == MappingMenuVisibility.UnlessTopLevel;
                    })
                    .ToDictionary(t => t.Value.Attribute as TriggerAttribute, t => t.Value)
                );
        }
    }
}
