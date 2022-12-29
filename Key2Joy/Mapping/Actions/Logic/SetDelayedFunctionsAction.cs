using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Key2Joy.Mapping
{
    [Action(
        Description = "Timeout for a specified duration before executing a function",
        Visibility = MappingMenuVisibility.Never,
        NameFormat = "Timeout for {0}ms"
    )]
    [Util.ObjectListViewGroup(
        Name = "Logic",
        Image = "script_code"
    )]
    public class SetDelayedFunctionsAction : BaseAction
    {
        [JsonProperty]
        public TimeSpan WaitTime;

        public SetDelayedFunctionsAction(string name, string description)
            : base(name, description)
        {
        }

        /// <markdown-doc>
        /// <parent-name>Logic</parent-name>
        /// <path>Api/Logic</path>
        /// </markdown-doc>
        /// <summary>
        /// Execute functions whilst waiting the specified time between them.
        /// 
        /// The first function is executed immediately.
        /// </summary>
        /// <markdown-example>
        /// Shows how to count down from 3 and execute a command using Lua.
        /// <code language="lua">
        /// <![CDATA[
        /// SetDelayedFunctions(
        ///    1000,
        ///    function ()
        ///       Print("Aborting in 3 second...")
        ///    end,
        ///    function ()
        ///       Print("Three")
        ///    end,
        ///    function ()
        ///       Print("Two")
        ///    end,
        ///    function ()
        ///       Print("One")
        ///    end,
        ///    function ()
        ///       App.Command("abort")
        ///    end
        /// )
        /// ]]>
        /// </code>
        /// </markdown-example>
        /// <param name="waitTime">Time to wait (in milliseconds) between function calls</param>
        /// <param name="callbacks">One or more functions to execute</param>
        /// <name>SetDelayedFunctions</name>
        [ExposesScriptingMethod("SetDelayedFunctions")]
        public async void ExecuteForScript(long waitTime, params Action[] callbacks)
        {
            WaitTime = TimeSpan.FromMilliseconds(waitTime);

            for (int i = 0; i < callbacks.Length; i++)
            {
                if (i > 0)
                    await Task.Delay(WaitTime);
                
                callbacks[i].Invoke();
            }
        }

        public override Task Execute(IInputBag inputBag = null)
        {
            // Irrelevant because only scripts should use this function
            return Task.Delay(WaitTime);
        }

        public override string GetNameDisplay()
        {
            // Irrelevant because only scripts should use this function
            return Name.Replace("{0}", WaitTime.TotalMilliseconds.ToString());
        }

        public override object Clone()
        {
            return new SetDelayedFunctionsAction(Name, description)
            {
                WaitTime = WaitTime,
                ImageResource = ImageResource,
                Name = Name,
            };
        }
    }
}
