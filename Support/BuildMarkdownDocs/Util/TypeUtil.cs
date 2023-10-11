using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace BuildMarkdownDocs.Util;

internal static class TypeUtil
{
    public static Type GetType(string typeName)
    {
        typeName = Regex.Replace(typeName, @"System\.Nullable{(.+)}", "System.Nullable`1[$1]");

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
}
