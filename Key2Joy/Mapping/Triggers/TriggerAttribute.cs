using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Key2Joy.Mapping
{
    public class TriggerAttribute : MappingAttribute
    {
        private static Dictionary<Type, TriggerAttribute> triggers;

        /// <summary>
        /// Loads all triggers in the assembly, optionally merging it with additional trigger types.
        /// </summary>
        /// <param name="additionalTriggers"></param>
        public static void BufferTriggers(IReadOnlyList<Type> additionalTriggers = null)
        {
            triggers = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.GetCustomAttribute(typeof(TriggerAttribute), false) != null)
                .ToDictionary(t => t, t => t.GetCustomAttribute(typeof(TriggerAttribute), false) as TriggerAttribute);

            if (additionalTriggers == null)
            {
                return;
            }

            foreach (var trigger in additionalTriggers)
            {
                if (triggers.ContainsKey(trigger))
                {
                    Console.WriteLine("Trigger {0} already exists in the trigger buffer. Overwriting.", trigger.Name);
                }

                triggers.Add(trigger, trigger.GetCustomAttribute(typeof(TriggerAttribute), false) as TriggerAttribute);
            }
        }

        /// <summary>
        /// Gets all trigger types and their attribute annotations
        /// </summary>
        /// <param name="forTopLevel"></param>
        /// <returns></returns>
        public static Dictionary<Type, TriggerAttribute> GetAllTriggers()
        {
            return triggers;
        }

        /// <summary>
        /// Gets all trigger types and their attribute annotations
        /// </summary>
        /// <returns></returns>
        public static SortedDictionary<TriggerAttribute, Type> GetAllTriggers(bool forTopLevel)
        {
            return new SortedDictionary<TriggerAttribute, Type>(
                triggers
                    .Where(kvp =>
                    {
                        var triggerAttribute = kvp.Value;

                        if (triggerAttribute == null
                        || triggerAttribute.Visibility == MappingMenuVisibility.Never)
                            return false;

                        if (forTopLevel)
                            return triggerAttribute.Visibility == MappingMenuVisibility.Always
                                || triggerAttribute.Visibility == MappingMenuVisibility.OnlyTopLevel;

                        return triggerAttribute.Visibility == MappingMenuVisibility.Always || triggerAttribute.Visibility == MappingMenuVisibility.UnlessTopLevel;
                    })
                    .ToDictionary(t => t.Value, t => t.Key)
                );
        }
    }
}
