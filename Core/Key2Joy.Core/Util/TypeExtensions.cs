using System;
using System.Collections.Generic;

namespace Key2Joy.Util;

public static class TypeExtensions
{
    /// <summary>
    /// Source: https://stackoverflow.com/a/457708
    /// </summary>
    /// <param name="generic"></param>
    /// <param name="toCheck"></param>
    /// <returns></returns>
    public static bool IsSubclassOfRawGeneric(this Type generic, Type toCheck)
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

    public static bool IsList(this Type generic) => generic.IsGenericType && (generic.GetGenericTypeDefinition() == typeof(List<>));
}
