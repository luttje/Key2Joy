using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KeyToJoy.Mapping
{
    public interface IScriptable
    {
        /// <summary>
        /// What the function name would be called
        /// </summary>
        /// <returns></returns>
        string FunctionName { get; }

        /// <summary>
        /// Returns the method name that actually executes the function
        /// </summary>
        string FunctionMethodName { get; }

        /// <summary>
        /// Returns enumerations to expose to the scripting environment
        /// </summary>
        Type[] ExposesEnumerations { get; }
    }
}
