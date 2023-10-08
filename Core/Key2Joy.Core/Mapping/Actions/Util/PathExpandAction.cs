using System;
using System.Threading.Tasks;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Contracts.Mapping.Triggers;

namespace Key2Joy.Mapping.Actions.Util;

[Action(
    Description = "Expand Path System Variables",
    Visibility = MappingMenuVisibility.Never,
    NameFormat = "Expand Path System Variables"
)]
public class PathExpandAction : CoreAction
{
    public PathExpandAction(string name)
        : base(name)
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
    public string ExecuteForScript(string path) => Environment.ExpandEnvironmentVariables(path);

    public override async Task Execute(AbstractInputBag inputBag = null)
    {
        // TODO: Currently this is only a script action...
    }

    public override bool Equals(object obj)
    {
        if (obj is not PathExpandAction)
        {
            return false;
        }

        return true;
    }
}
