﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Key2Joy.Mapping
{
    [Action(
        Description = "Cancels a timeout previously established by calling SetTimeout()",
        Visibility = ActionVisibility.Never
    )]
    [Util.ObjectListViewGroup(
        Name = "Logic",
        Image = "script_code"
    )]
    internal class ClearTimeoutAction : BaseAction
    {
        public ClearTimeoutAction(string name, string description)
            : base(name, description)
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

        internal override Task Execute(InputBag inputBag = null)
        {
            // Irrelevant because only scripts should use this function
            return null;
        }

        public override string GetNameDisplay()
        {
            // Irrelevant because only scripts should use this function
            return null;
        }

        public override object Clone()
        {
            // Irrelevant because only scripts should use this function
            return new ClearTimeoutAction(Name, description)
            {
                ImageResource = ImageResource,
                Name = Name,
            };
        }
    }
}