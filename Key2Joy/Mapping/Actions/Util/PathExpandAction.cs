using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Mapping
{
    [Action(
        Description = "Expand Path System Variables",
        Visibility = MappingMenuVisibility.Never,
        NameFormat = "Expand Path System Variables"
    )]
    internal class PathExpandAction : BaseAction
    {
        public PathExpandAction(string name, string description)
            : base(name, description)
        { }

        /// <markdown-doc>
        /// <parent-name>Util</parent-name>
        /// <path>Api/Util</path>
        /// </markdown-doc>
        /// <summary>
        /// Expands system environment variables (See also: https://ss64.com/nt/syntax-variables.html). 
        /// </summary>
        /// <markdown-example>
        /// Demonstrates how to get the home drive
        /// <code language="js">
        /// <![CDATA[
        /// Print(Util.PathExpand("%HOMEDRIVE%/"))
        /// ]]>
        /// </code>
        /// </markdown-example>
        /// <returns>String containing expanded path</returns>
        /// <param name="path">The path to expand</param>
        /// <name>Util.PathExpand</name>
        [ExposesScriptingMethod("Util.PathExpand")]
        public string ExecuteForScript(string path)
        {
            return Environment.ExpandEnvironmentVariables(path);
        }

        internal override async Task Execute(IInputBag inputBag = null)
        {
            // TODO: Currently this is only a script action...
        }

        public override bool Equals(object obj)
        {
            if (!(obj is PathExpandAction action))
                return false;

            return true;
        }

        public override object Clone()
        {
            return new PathExpandAction(Name, description)
            {
                ImageResource = ImageResource,
                Name = Name,
            };
        }
    }
}
