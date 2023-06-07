using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Util;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Key2Joy.Mapping
{
    [Action(
        Description = "Cancels an interval previously established by calling SetInterval()",
        Visibility = MappingMenuVisibility.Never
    )]
    [ObjectListViewGroup(
        Name = "Logic",
        Image = "script_code"
    )]
    public class ClearIntervalAction : CoreAction
    {
        public ClearIntervalAction(string name)
            : base(name)
        {
        }

        /// <markdown-doc>
        /// <parent-name>Logic</parent-name>
        /// <path>Api/Logic</path>
        /// </markdown-doc>
        /// <summary>
        /// Cancels an interval previously established by calling SetInterval()
        /// </summary>
        /// <markdown-example>
        /// Shows how to count up to 3 every second and then stop by using ClearInterval();
        /// <code language="js">
        /// <![CDATA[
        /// var count = 0;
        /// var intervalId;
        ///
        /// intervalId = setInterval(() => {
        ///    Print(count++);
        ///
        ///    if(count == 3)
        ///       clearInterval(intervalId);
        /// }, 1000);
        ///
        /// Print(intervalId);
        /// ]]>
        /// </code>
        /// </markdown-example>
        /// <name>ClearInterval</name>
        /// <param name="intervalId">Id returned by SetInterval to cancel</param>
        [ExposesScriptingMethod("ClearInterval")]
        [ExposesScriptingMethod("clearInterval")] // Alias to conform to JS standard
        public void ExecuteForScript(IdPool.IntervalId intervalId)
        {
            intervalId.Cancel();
        }

        public override Task Execute(AbstractInputBag inputBag = null)
        {
            // Irrelevant because only scripts should use this function
            return null;
        }

        public override string GetNameDisplay()
        {
            // Irrelevant because only scripts should use this function
            return null;
        }
    }
}
