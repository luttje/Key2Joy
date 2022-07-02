using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KeyToJoy.Mapping
{
    internal class TriggerAttribute : MappingAttribute
    {

        /// <summary>
        /// Gets all trigger types and their attribute annotations
        /// </summary>
        /// <returns></returns>
        public static Dictionary<Type, TriggerAttribute> GetAllTriggers()
        {
            return Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.GetCustomAttribute(typeof(TriggerAttribute), false) != null)
                .ToDictionary(t => t, t => t.GetCustomAttribute(typeof(TriggerAttribute), false) as TriggerAttribute);
        }
    }
}
