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
        Visibility = ActionVisibility.Never,
        NameFormat = "Timeout for {0}ms"
    )]
    [Util.ObjectListViewGroup(
        Name = "Logic",
        Image = "script_code"
    )]
    internal class SetTimeoutAction : BaseAction
    {
        [JsonProperty]
        public TimeSpan WaitTime;

        public SetTimeoutAction(string name, string description)
            : base(name, description)
        {
        }

        /// <markdown-doc>
        /// <parent-name>Logic</parent-name>
        /// <path>Api/Logic</path>
        /// </markdown-doc>
        /// <summary>
        /// Timeout for the specified duration in milliseconds, then execute the callback
        /// </summary>
        /// <markdown-example>
        /// Shows how to count down from 3 and execute a command using Javascript.
        /// <code language="js">
        /// <![CDATA[
        /// setTimeout(function () {
        ///   Print("Aborting in 3 second...")
        ///    
        ///   setTimeout(function () {
        ///     Print("Three")
        /// 
        ///     setTimeout(function () {
        ///       Print("Two")
        /// 
        ///       setTimeout(function () {
        ///         Print("One")
        /// 
        ///         setTimeout(function () {
        ///           App.Command("abort")
        ///         }, 1000)
        ///       }, 1000)
        ///     }, 1000)
        ///   }, 1000)
        /// }, 1000)
        /// ]]>
        /// </code>
        /// </markdown-example>
        /// <markdown-example>
        /// Shows how to count down from 3 each second and execute a command using Lua.
        /// <code language="lua">
        /// <![CDATA[
        /// SetTimeout(function ()
        ///    Print("Aborting in 3 second...")
        ///
        ///    SetTimeout(function ()
        ///       Print("Three")
        ///
        ///       SetTimeout(function ()
        ///          Print("Two")
        ///
        ///          SetTimeout(function ()
        ///             Print("One")
        ///
        ///             SetTimeout(function ()
        ///                App.Command("abort")
        ///             end, 1000)
        ///          end, 1000)
        ///       end, 1000)
        ///    end, 1000)
        /// end, 1000)
        /// ]]>
        /// </code>
        /// </markdown-example>
        /// <param name="callback">Function to execute after the wait</param>
        /// <param name="waitTime">Time to wait (in milliseconds)</param>
        /// <name>SetTimeout</name>
        [ExposesScriptingMethod("SetTimeout")]
        [ExposesScriptingMethod("setTimeout")] // Alias to conform to JS standard
        public async void ExecuteForScript(Action callback, long waitTime)
        {
            WaitTime = TimeSpan.FromMilliseconds(waitTime);
            await Task.Delay(WaitTime);
            callback.Invoke();
        }

        internal override Task Execute(InputBag inputBag = null)
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
            return new SetTimeoutAction(Name, description)
            {
                WaitTime = WaitTime,
                ImageResource = ImageResource,
                Name = Name,
            };
        }
    }
}
