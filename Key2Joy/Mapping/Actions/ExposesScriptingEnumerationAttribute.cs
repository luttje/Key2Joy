using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Mapping
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ExposesScriptingEnumerationAttribute : Attribute
    {
        /// <summary>
        /// Returns enumeration to expose to the scripting environment
        /// </summary>
        public Type ExposedEnumeration { get; set; }

        public ExposesScriptingEnumerationAttribute(Type exposedEnumeration)
        {
            ExposedEnumeration = exposedEnumeration;
        }
    }
}
