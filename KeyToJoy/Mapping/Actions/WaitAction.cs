using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyToJoy.Mapping
{
    internal class WaitAction : BaseAction
    {
        [JsonProperty]
        public TimeSpan WaitTime;

        public WaitAction(string name, string imagePath, TimeSpan waitTime)
            : base(name, imagePath)
        {
            this.WaitTime = waitTime;
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
