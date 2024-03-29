using System;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Actions;

namespace Key2Joy.Mapping.Actions.Util;

[Action(
    Description = "Get System Time",
    Visibility = MappingMenuVisibility.Never,
    NameFormat = "Get the system time"
)]
public class GetTimeAction : CoreAction
{
    public GetTimeAction(string name)
        : base(name)
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
    public long ExecuteForScript() => DateTimeOffset.Now.ToUnixTimeSeconds();

    public override bool Equals(object obj)
    {
        if (obj is not GetTimeAction)
        {
            return false;
        }

        return true;
    }
}
