using System;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Contracts.Mapping.Triggers;
using Key2Joy.Contracts.Plugins;

namespace Key2Joy.Mapping.Actions.Logic;

[Action(
    Description = "Repeatedly calls a function or executes a code snippet, with a fixed time delay between each call",
    Visibility = MappingMenuVisibility.Never,
    GroupName = "Logic",
    GroupImage = "application_xp_terminal"
)]
public class SetIntervalAction : CoreAction
{
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
    /// Shows how to count every second
    /// <code language="js">
    /// <![CDATA[
    /// let counter = 0;
    /// setInterval(function () {
    ///     counter++;
    ///     Print(counter);
    /// }, 1000)
    /// ]]>
    /// </code>
    /// </markdown-example>
    /// <param name="callback">Function to execute after each wait</param>
    /// <param name="waitTime">Time to wait (in milliseconds)</param>
    /// <param name="arguments">Zero or more extra parameters to pass to the function</param>
    /// <returns>An interval id that can be removed with clearInterval</returns>
    /// <name>SetInterval</name>
    [ExposesScriptingMethod("SetInterval")]
    [ExposesScriptingMethod("setInterval")] // Alias to conform to JS standard
    public IdPool.IntervalId ExecuteForScript(CallbackAction callback, long waitTime, params object[] arguments)
    {
        this.WaitTime = TimeSpan.FromMilliseconds(waitTime);

        CancellationTokenSource cancellation = new();
        var token = cancellation.Token;
        Task.Run(async () =>
        {
            while (true)
            {
                token.ThrowIfCancellationRequested();

                await Task.Delay(this.WaitTime);

                token.ThrowIfCancellationRequested();

                callback.Invoke(arguments);
            }
        }, token);

        return IdPool.CreateNewId<IdPool.IntervalId>(cancellation);
    }

    public override Task Execute(AbstractInputBag inputBag = null) =>
        // Irrelevant because only scripts should use this function
        Task.Delay(this.WaitTime);

    public override string GetNameDisplay() =>
        // Irrelevant because only scripts should use this function
        this.Name.Replace("{0}", this.WaitTime.TotalMilliseconds.ToString());
}
