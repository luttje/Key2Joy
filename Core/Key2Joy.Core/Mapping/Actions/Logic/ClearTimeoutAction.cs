using System;
using System.Threading.Tasks;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Contracts.Mapping.Triggers;

namespace Key2Joy.Mapping.Actions.Logic
{
    [Action(
        Description = "Cancels a timeout previously established by calling SetTimeout()",
        Visibility = MappingMenuVisibility.Never,
        GroupName = "Logic",
        GroupImage = "application_xp_terminal"
    )]
    public class ClearTimeoutAction : CoreAction
    {
        public ClearTimeoutAction(string name)
            : base(name)
        {
        }

        /// <markdown-doc>
        /// <parent-name>Logic</parent-name>
        /// <path>Api/Logic</path>
        /// </markdown-doc>
        /// <summary>
        /// Cancels a timeout previously established by calling SetTimeout()
        /// </summary>
        /// <markdown-example>
        /// Shows how to set and immediately cancel a timeout.
        /// <code language="js">
        /// <![CDATA[
        /// var timeoutID = setTimeout(() => {
        ///    Print("You shouldn't see this because the timeout will have been cancelled!");
        /// }, 1000);
        ///
        /// Print(timeoutID);
        ///
        /// clearTimeout(timeoutID);
        /// ]]>
        /// </code>
        /// </markdown-example>
        /// <name>ClearTimeout</name>
        /// <param name="timeoutId">Id returned by SetTimeout to cancel</param>
        [ExposesScriptingMethod("ClearTimeout")]
        [ExposesScriptingMethod("clearTimeout")] // Alias to conform to JS standard
        public void ExecuteForScript(IdPool.TimeoutId timeoutId)
        {
            timeoutId.Cancel();
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
