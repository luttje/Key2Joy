using System;

namespace Key2Joy.Contracts.Mapping.Actions
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
            this.FunctionName = functionName;
        }
    }
}
