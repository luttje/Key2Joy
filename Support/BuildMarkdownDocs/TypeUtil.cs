using System;
using System.Text.RegularExpressions;

namespace BuildMarkdownDocs;

internal static class TypeUtil
{
    public static void NotifyAssemblyRelation(Type type)
    {
        // The content of this is irrelevant. We only want to be sure that the assembly is referenced.
        if (type is null)
        {
            throw new ArgumentNullException(nameof(type));
        }
    }

    public static Type GetType(string typeName)
    {
        typeName = Regex.Replace(typeName, @"System\.Nullable{(.+)}", "System.Nullable`1[$1]");

        var type = Type.GetType(typeName);

        if (type != null)
        {
            return type;
        }

        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (var parentType in assembly.GetTypes())
            {
                if (parentType.Name == typeName)
                {
                    return type;
                }

                foreach (var childType in parentType.GetNestedTypes())
                {
                    if (childType.FullName.Replace("+", ".") == typeName)
                    {
                        return childType;
                    }
                }
            }

            var referencedAssemblies = assembly.GetReferencedAssemblies();

            foreach (var referencedAssembly in referencedAssemblies)
            {
                type = Type.GetType($"{typeName}, {referencedAssembly.FullName}");

                if (type != null)
                {
                    return type;
                }
            }

        }

        return null;
    }
}
