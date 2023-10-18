using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace BuildMarkdownDocs.Util;

public static class TypeUtil
{
    public static string CleanTypeName(string typeName)
        => Regex.Replace(typeName, @"System\.Nullable{(.+)}", "System.Nullable`1[$1]");

    public static Type GetType(string typeName)
    {
        typeName = CleanTypeName(typeName);

        var type = Type.GetType(typeName);

        if (type != null)
        {
            return type;
        }

        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assembly in assemblies)
        {
            foreach (var parentType in assembly.GetTypes())
            {
                if (parentType.FullName == typeName)
                {
                    return parentType;
                }

                foreach (var childType in parentType.GetNestedTypes())
                {
                    if (childType.FullName.Replace("+", ".") == typeName)
                    {
                        return childType;
                    }
                }
            }
        }

        return null;
    }

    public static MethodInfo GetMethodInfo(string signature)
    {
        var pattern = @"M:(?<typeName>[\w.]+)\.(?<methodName>[\w]+)(\((?<parameters>.*?)\))?";
        var match = Regex.Match(signature, pattern);

        if (match.Success)
        {
            var typeName = match.Groups["typeName"].Value;
            var methodName = match.Groups["methodName"].Value;
            var parameters = match.Groups["parameters"].Value;

            var type = GetType(typeName) ?? throw new ArgumentException($"Invalid Type provided to GetMethodInfo");

            var paramTypes = ParseParameterTypes(parameters);
            var methodInfo = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance, null, paramTypes, null) ?? throw new ArgumentException($"Invalid Type provided to GetMethodInfo");

            return methodInfo;
        }

        throw new ArgumentException($"Invalid signature provided to GetMethodInfo");
    }

    public static Type[] ParseParameterTypes(string parameters)
    {
        if (string.IsNullOrEmpty(parameters))
        {
            return Type.EmptyTypes;
        }

        var paramTypeStrings = parameters.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        var paramTypes = new Type[paramTypeStrings.Length];

        for (var i = 0; i < paramTypeStrings.Length; i++)
        {
            paramTypes[i] = GetType(paramTypeStrings[i].Trim());
        }

        return paramTypes;
    }
}
