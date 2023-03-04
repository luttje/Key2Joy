using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Mapping
{
    public class TriggerAttribute : MappingAttribute
    {

        /// <summary>
        /// Gets all trigger types and their attribute annotations
        /// </summary>
        /// <returns></returns>
        public static SortedDictionary<TriggerAttribute, Type> GetAllTriggers(bool forTopLevel)
        {
            return new SortedDictionary<TriggerAttribute, Type>(
                Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t =>
                {
                    var triggerAttribute = t.GetCustomAttributes(typeof(TriggerAttribute), false).FirstOrDefault() as TriggerAttribute;

                    if (triggerAttribute == null
                    || triggerAttribute.Visibility == MappingMenuVisibility.Never)
                        return false;

                    if (forTopLevel)
                        return triggerAttribute.Visibility == MappingMenuVisibility.Always
                            || triggerAttribute.Visibility == MappingMenuVisibility.OnlyTopLevel;

                    return triggerAttribute.Visibility == MappingMenuVisibility.Always || triggerAttribute.Visibility == MappingMenuVisibility.UnlessTopLevel;
                })
                .ToDictionary(t => t.GetCustomAttribute(typeof(TriggerAttribute), false) as TriggerAttribute, t => t)
            );
        }
    }
}
