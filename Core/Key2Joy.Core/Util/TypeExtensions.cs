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

    /// <summary>
    /// Gets the minimum value for a numeric type
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static object GetNumericMinValue(this Type type)
    {
        type = Nullable.GetUnderlyingType(type) ?? type;

        if (type == typeof(int))
        {
            return int.MinValue;
        }
        else if (type == typeof(uint))
        {
            return uint.MinValue;
        }
        else if (type == typeof(long))
        {
            return long.MinValue;
        }
        else if (type == typeof(ulong))
        {
            return ulong.MinValue;
        }
        else if (type == typeof(short))
        {
            return short.MinValue;
        }
        else if (type == typeof(ushort))
        {
            return ushort.MinValue;
        }
        else if (type == typeof(byte))
        {
            return byte.MinValue;
        }
        else if (type == typeof(sbyte))
        {
            return sbyte.MinValue;
        }
        else if (type == typeof(float))
        {
            return float.MinValue;
        }
        else if (type == typeof(double))
        {
            return double.MinValue;
        }
        else if (type == typeof(decimal))
        {
            return decimal.MinValue;
        }
        else
        {
            throw new ArgumentException($"Type {type} is not a numeric type");
        }
    }

    /// <summary>
    /// Gets the maximum value for a numeric type
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static object GetNumericMaxValue(this Type type)
    {
        type = Nullable.GetUnderlyingType(type) ?? type;

        if (type == typeof(int))
        {
            return int.MaxValue;
        }
        else if (type == typeof(uint))
        {
            return uint.MaxValue;
        }
        else if (type == typeof(long))
        {
            return long.MaxValue;
        }
        else if (type == typeof(ulong))
        {
            return ulong.MaxValue;
        }
        else if (type == typeof(short))
        {
            return short.MaxValue;
        }
        else if (type == typeof(ushort))
        {
            return ushort.MaxValue;
        }
        else if (type == typeof(byte))
        {
            return byte.MaxValue;
        }
        else if (type == typeof(sbyte))
        {
            return sbyte.MaxValue;
        }
        else if (type == typeof(float))
        {
            return float.MaxValue;
        }
        else if (type == typeof(double))
        {
            return double.MaxValue;
        }
        else if (type == typeof(decimal))
        {
            return decimal.MaxValue;
        }
        else
        {
            throw new ArgumentException($"Type {type} is not a numeric type");
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static decimal ToDecimalSafe(object input)
    {
        if (input is decimal dec)
        {
            return dec;
        }

        if (input is double d)
        {
            if (d > (double)decimal.MaxValue)
            {
                return decimal.MaxValue;
            }
            else if (d < (double)decimal.MinValue)
            {
                return decimal.MinValue;
            }
            else
            {
                return (decimal)d;
            }
        }

        if (input is float f)
        {
            if (f > (float)decimal.MaxValue)
            {
                return decimal.MaxValue;
            }
            else if (f < (float)decimal.MinValue)
            {
                return decimal.MinValue;
            }
            else
            {
                return (decimal)f;
            }
        }

        try
        {
            return Convert.ToDecimal(input);
        }
        catch (OverflowException)
        {
            return decimal.MaxValue;
        }
    }
}
