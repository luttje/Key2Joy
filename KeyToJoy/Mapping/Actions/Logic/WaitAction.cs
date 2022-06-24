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
        FunctionMethodName = nameof(WaitAction.ExecuteActionForScript)
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

        public object[] ExecuteActionForScript(params object[] parameters)
        {
            if (parameters.Length < 2)
                throw new ArgumentException($"{SCRIPT_COMMAND} expected a callback and wait time!");
            
            if(!(parameters[0] is NLua.LuaFunction callback))
                throw new ArgumentException($"{SCRIPT_COMMAND} expected a callback as the first argument!");

            var waitTime = parameters[1] as long?;

            if (waitTime == null)
                throw new ArgumentException($"{SCRIPT_COMMAND} expected a wait time (long) as the second argument!");

            WaitTime = TimeSpan.FromMilliseconds((long)waitTime);
            var task = Task.Run(async () =>
            {
                await this.Execute();
                callback.Call();
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
