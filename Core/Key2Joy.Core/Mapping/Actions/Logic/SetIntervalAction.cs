using Key2Joy.Contracts.Mapping;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Key2Joy.Mapping
{
    [Action(
        Description = "Repeatedly calls a function or executes a code snippet, with a fixed time delay between each call",
        Visibility = MappingMenuVisibility.Never,
        GroupName = "Logic",
        GroupImage = "application_xp_terminal"
    )]
    public class SetIntervalAction : CoreAction
    {
        public delegate void CallbackAction(params object[] arguments);
            
        [JsonInclude]
        public TimeSpan WaitTime;

        public SetIntervalAction(string name)
            : base(name)
        {
        }

        /// <markdown-doc>
        /// <parent-name>Logic</parent-name>
        /// <path>Api/Logic</path>
        /// </markdown-doc>
        /// <summary>
        /// Repeatedly calls a function or executes a code snippet, with a fixed time delay between each call
        /// </summary>
        /// <markdown-example>
        /// Shows how to count up to 10 every second and then stop by using ClearInterval();
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
        /// <param name="callback">Function to execute after each wait</param>
        /// <param name="waitTime">Time to wait (in milliseconds)</param>
        /// <param name="arguments">Zero or more extra parameters to pass to the function</param>
        /// <name>SetInterval</name>
        [ExposesScriptingMethod("SetInterval")]
        [ExposesScriptingMethod("setInterval")] // Alias to conform to JS standard
        public IdPool.IntervalId ExecuteForScript(CallbackAction callback, long waitTime, params object[] arguments)
        {
            WaitTime = TimeSpan.FromMilliseconds(waitTime);

            var cancellation = new CancellationTokenSource();
            var token = cancellation.Token;
            Task.Run(async () =>
            {
                while (true)
                {
                    token.ThrowIfCancellationRequested();

                    await Task.Delay(WaitTime);

                    token.ThrowIfCancellationRequested();

                    callback.Invoke(arguments);
                }
            }, token);

            return IdPool.CreateNewId<IdPool.IntervalId>(cancellation);
        }

        public override Task Execute(AbstractInputBag inputBag = null)
        {
            // Irrelevant because only scripts should use this function
            return Task.Delay(WaitTime);
        }

        public override string GetNameDisplay()
        {
            // Irrelevant because only scripts should use this function
            return Name.Replace("{0}", WaitTime.TotalMilliseconds.ToString());
        }
    }
}
