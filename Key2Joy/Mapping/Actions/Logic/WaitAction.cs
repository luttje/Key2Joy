using Key2Joy.Contracts.Mapping;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Key2Joy.Mapping
{
    [Action(
        Description = "Wait for a specified duration",
        Visibility = MappingMenuVisibility.UnlessTopLevel,
        NameFormat = "Wait for {0}ms"
    )]
    public class WaitAction : CoreAction
    {
        [JsonProperty]
        public TimeSpan WaitTime;

        public WaitAction(string name, string description)
            : base(name, description)
        {
        }

        public override Task Execute(IInputBag inputBag = null)
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
