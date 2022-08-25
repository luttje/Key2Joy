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
        Description = "Get System Time",
        Visibility = MappingMenuVisibility.Never,
        NameFormat = "Get the system time"
    )]
    internal class GetTimeAction : BaseAction
    {
        public GetTimeAction(string name, string description)
            : base(name, description)
        { }

        /// <markdown-doc>
        /// <parent-name>Util</parent-name>
        /// <path>Api/Util</path>
        /// </markdown-doc>
        /// <summary>
        /// Gets the current system time UNIX in seconds
        /// </summary>
        /// <markdown-example>
        /// The code below prints 1661456521 to the logs if the system time is 19:42:01 (GMT) on the 25th of August, 2022.
        /// <code language="js">
        /// <![CDATA[
        /// Print(Util.GetUnixTimeSeconds())
        /// ]]>
        /// </code>
        /// </markdown-example>
        /// <returns>Time since UNIX in seconds</returns>
        /// <name>Util.GetUnixTimeSeconds</name>
        [ExposesScriptingMethod("Util.GetUnixTimeSeconds")]
        public long ExecuteForScript()
        {
            return DateTimeOffset.Now.ToUnixTimeSeconds();
        }

        internal override async Task Execute(IInputBag inputBag = null)
        {
            // TODO: Currently this is only a script action...
        }

        public override bool Equals(object obj)
        {
            if (!(obj is GetTimeAction action))
                return false;

            return true;
        }

        public override object Clone()
        {
            return new GetTimeAction(Name, description)
            {
                ImageResource = ImageResource,
                Name = Name,
            };
        }
    }
}
