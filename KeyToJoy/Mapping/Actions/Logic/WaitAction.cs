using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyToJoy.Mapping
{
    [Action(
        Name = "Wait for a specified duration",
        Visibility = ActionVisibility.UnlessTopLevel,
        OptionsUserControl = typeof(WaitActionControl)
    )]
    internal class WaitAction : BaseAction
    {
        [JsonProperty]
        public TimeSpan WaitTime;

        public WaitAction(string name, string imageResource)
            : base(name, imageResource)
        {
        }

        internal override Task Execute(InputBag inputBag)
        {
            return Task.Delay(WaitTime);
        }

        public override string GetNameDisplay()
        {
            return $"{Name} for {WaitTime.TotalMilliseconds}ms";
        }

        public override string GetContextDisplay()
        {
            return "Logic";
        }
    }
}
