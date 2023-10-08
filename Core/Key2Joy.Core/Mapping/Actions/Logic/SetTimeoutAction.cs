﻿using Key2Joy.Contracts.Mapping;
using System;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Key2Joy.Mapping
{
    [Action(
        Description = "Sets a timer which executes a function or specified piece of code once the timer expires",
        Visibility = MappingMenuVisibility.Never,
        GroupName = "Logic",
        GroupImage = "application_xp_terminal"
    )]
    public class SetTimeoutAction : CoreAction
    {
        public delegate void CallbackAction(params object[] arguments);

        [JsonInclude]
        public TimeSpan WaitTime;

        public SetTimeoutAction(string name)
            : base(name)
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
        /// <param name="arguments">Zero or more extra parameters to pass to the function</param>
        /// <name>SetTimeout</name>
        [ExposesScriptingMethod("SetTimeout")]
        [ExposesScriptingMethod("setTimeout")] // Alias to conform to JS standard
        public IdPool.TimeoutId ExecuteForScript(CallbackAction callback, long waitTime, params object[] arguments)
        {
            WaitTime = TimeSpan.FromMilliseconds(waitTime);

            var cancellation = new CancellationTokenSource();
            var token = cancellation.Token;

            Task.Run(async () =>
            {
                token.ThrowIfCancellationRequested();

                await Task.Delay(WaitTime);

                token.ThrowIfCancellationRequested();

                lock (BaseScriptAction.LockObject)
                    callback.Invoke(arguments);
            }, token);

            return IdPool.CreateNewId<IdPool.TimeoutId>(cancellation);
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
