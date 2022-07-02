using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyToJoy.Mapping
{
    [Action(
        Description = "Wait for a specified duration (useful inbetween actions in a sequence)",
        Visibility = ActionVisibility.UnlessTopLevel,
        OptionsUserControl = typeof(WaitActionControl),
        NameFormat = "Wait for {0}ms"
    )]
    internal class WaitAction : BaseAction
    {
        [JsonProperty]
        public TimeSpan WaitTime;

        public WaitAction(string name, string description)
            : base(name, description)
        {
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
