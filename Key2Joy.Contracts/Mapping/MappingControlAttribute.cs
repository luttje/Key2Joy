using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Key2Joy.Contracts.Mapping;

namespace Key2Joy.Contracts.Mapping
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class MappingControlAttribute : Attribute
    {
        public Type ForType { get; set; }
        public Type[] ForTypes { get; set; }
        public string ImageResourceName { get; set; } = "error";

        /// <summary>
        /// Returns the control to go with the given action/trigger mapping type.
        /// </summary>
        /// <param name="mappingTypeName"></param>
        /// <returns></returns>
        public static Type GetCorrespondingControlType(string mappingTypeName)
        {
            // Check all loaded assemblies for a class with the MappingControl attribute that has a ForTypeName that matches the mappingType's name.
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    var attr = type.GetCustomAttribute<MappingControlAttribute>();
                    if (attr == null)
                    {
                        continue;

                    }

                    if (attr.ForType != null && attr.ForType.FullName == mappingTypeName)
                    {
                        return type;
                    }

                    if (attr.ForTypes != null && attr.ForTypes.Any(t => t.FullName == mappingTypeName))
                    {
                        return type;
                    }
                }
            }

            throw new NotImplementedException($"No MappingControl found for {mappingTypeName}");
        }
    }
}
