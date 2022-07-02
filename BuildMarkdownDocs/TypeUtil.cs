using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildMarkdownDocs
{
    internal static class TypeUtil
    {
        public static void NotifyAssemblyRelation(Type type) 
        {
            // The content of this is irrelevant. We only want to be sure that the assembly is referenced.
        }

        public static Type GetType(string typeName)
        {
            var type = Type.GetType(typeName);
            
            if (type != null)
                return type;

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = assembly.GetType(typeName);

                if (type != null)
                    return type;

                var referencedAssemblies = assembly.GetReferencedAssemblies();

                foreach (var referencedAssembly in referencedAssemblies)
                {
                    type = Type.GetType($"{typeName}, {referencedAssembly.FullName}");

                    if (type != null)
                        return type;
                }
            }
            
            return null;
        }
    }
}
