using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Util
{
    internal static class TypeExtensions
    {
        /// <summary>
        /// Source: https://stackoverflow.com/a/457708
        /// </summary>
        /// <param name="generic"></param>
        /// <param name="toCheck"></param>
        /// <returns></returns>
        internal static bool IsSubclassOfRawGeneric(this Type generic, Type toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }

        internal static bool IsList(this Type generic)
        {
            return generic.IsGenericType && (generic.GetGenericTypeDefinition() == typeof(List<>));
        }
    }
}
