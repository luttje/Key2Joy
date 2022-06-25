using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyToJoy.Mapping
{
    [Action(
        Description = "Wait for a specified duration",
        Visibility = ActionVisibility.UnlessTopLevel,
        OptionsUserControl = typeof(WaitActionControl),
        NameFormat = "Wait for {0}ms",
        FunctionName = SCRIPT_COMMAND,
        FunctionMethodName = nameof(ExecuteActionForScript)
    )]
    internal class WaitAction : BaseAction
    {
        internal const string SCRIPT_COMMAND = "wait";
        
        [JsonProperty]
        public TimeSpan WaitTime;

        public WaitAction(string name, string description)
            : base(name, description)
        {
        }

        public object ExecuteActionForScript(BaseScriptAction scriptAction, params object[] parameters)
        {
            if (parameters.Length < 2)
                throw new ArgumentException($"{SCRIPT_COMMAND} expected a callback and wait time!");

            if (!scriptAction.TryConvertParameterToCallback(parameters[0], out Action callback))
                throw new ArgumentException($"{SCRIPT_COMMAND} expected a callback as the first argument!");

            if (!scriptAction.TryConvertParameterToLong(parameters[1], out long waitTime))
                throw new ArgumentException($"{SCRIPT_COMMAND} expected a wait time (long) as the second argument!");

            WaitTime = TimeSpan.FromMilliseconds(waitTime);
            var task = Task.Run(async () =>
            {
                await this.Execute();
                callback();
            });

            return null;
        }

        internal override Task Execute(InputBag inputBag = null)
        {
            return Task.Delay(WaitTime);
        }

        public override string GetNameDisplay()
        {
            return Name.Replace("{0}", WaitTime.TotalMilliseconds.ToString());
        }

        public override object Clone()
        {
            return new WaitAction(Name, description)
            {
                WaitTime = WaitTime,
                ImageResource = ImageResource,
                Name = Name,
            };
        }
    }
}
