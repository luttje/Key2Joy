using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Contracts.Mapping
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ExposesScriptingMethodAttribute : Attribute
    {
        /// <summary>
        /// What the function name would be called
        /// </summary>
        /// <returns></returns>
        public string FunctionName { get; private set; }

        public ExposesScriptingMethodAttribute(string functionName)
        {
            FunctionName = functionName;
        }
    }
}
