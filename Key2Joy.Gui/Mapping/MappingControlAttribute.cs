using Key2Joy.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Gui.Mapping
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class MappingControlAttribute : Attribute
    {
        public string ImageResourceName { get; set; } = "error";

        /// <summary>
        /// Returns the control to go with the given action/trigger mapping type.
        /// </summary>
        /// <param name="mappingType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        internal static Type GetCorrespondingControlType(Type mappingType, out object[] parameters)
        {
            var typeName = mappingType.Name;
            parameters = new object[] { };

            // If it's LuaScriptAction or JavascriptAction change the typename to ScriptAction
            if (mappingType == typeof(LuaScriptAction)
                || mappingType == typeof(JavascriptAction))
            {
                typeName = "ScriptAction";
                parameters = new object[]{
                    mappingType == typeof(LuaScriptAction) ? "Lua" : "Javascript"
                };
            }

            var fullName = $"{typeof(Program).Namespace}.{nameof(Mapping)}.{typeName}Control";
            
            return Assembly.GetExecutingAssembly().GetType(fullName);
        }
    }
}
