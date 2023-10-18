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

    /// <summary>
    /// Copies a given array to a new array of the target type, e.g: from object[] to string[].
    /// </summary>
    /// <param name="originalArray"></param>
    /// <param name="elementType"></param>
    /// <returns></returns>
    public static Array CopyArrayToNewType(this object[] originalArray, Type elementType)
    {
        var newArray = Array.CreateInstance(elementType, originalArray.Length);

        for (var i = 0; i < originalArray.Length; i++)
        {
            newArray.SetValue(Convert.ChangeType(originalArray[i], elementType), i);
        }

        return newArray;

        // Explored alternatives:
        // B: This doesn't actually cast it tot he correct type (but leaves it an object[]
        //var convertedArray = (Array)Array.ConvertAll(originalArray, item => Convert.ChangeType(item, elementType));

        //return convertedArray;

        // C: Dynamically invoke Array.ConvertAll so that it returns an array of type elementType[]
        //var convertAllMethod = typeof(Array).GetMethod("ConvertAll", BindingFlags.Public | BindingFlags.Static);
        //var genericConvertAllMethod = convertAllMethod.MakeGenericMethod(typeof(object), elementType);
        //var converterDelegateType = typeof(Func<,>).MakeGenericType(typeof(object), elementType);

        //return (Array)genericConvertAllMethod.Invoke(
        //    null,
        //    new object[] {
        //        originalArray,
        //        Delegate.CreateDelegate(converterDelegateType, typeof(Convert).GetMethod("ChangeType", new[] { typeof(object), typeof(Type) }))
        //    });
    }
}
