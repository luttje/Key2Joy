using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KeyToJoy.Mapping
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ExposesScriptingMethodAttribute : Attribute
    {
        /// <summary>
        /// What the function name would be called
        /// </summary>
        /// <returns></returns>
        public string FunctionName { get; set; }

        public ExposesScriptingMethodAttribute(string functionName)
        {
            FunctionName = functionName;
        }
    }
}
