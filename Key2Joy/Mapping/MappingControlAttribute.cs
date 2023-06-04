using Key2Joy.Mapping;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Mapping
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
        /// <param name="mappingType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static Type GetCorrespondingControlType(Type mappingType, out object[] parameters)
        {
            parameters = new object[] { };

            // If it's LuaScriptAction or JavascriptAction change the typename to ScriptAction
            if (mappingType == typeof(LuaScriptAction)
                || mappingType == typeof(JavascriptAction))
            {
                parameters = new object[]{
                    mappingType == typeof(LuaScriptAction) ? "Lua" : "Javascript"
                };
            }

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

                    if (attr.ForType != null && attr.ForType == mappingType)
                    {
                        return type;
                    }

                    if (attr.ForTypes != null && attr.ForTypes.Contains(mappingType))
                    {
                        return type;
                    }
                }
            }

            throw new NotImplementedException($"No MappingControl found for {mappingType.Name}");
        }
    }
}
