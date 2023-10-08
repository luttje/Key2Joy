using Key2Joy.Contracts.Mapping;
using System;
using System.Text.Json.Serialization;
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
        [JsonInclude]
        public TimeSpan WaitTime;

        public WaitAction(string name)
            : base(name)
        {
        }

        public override Task Execute(AbstractInputBag inputBag = null)
        {
            return Task.Delay(WaitTime);
        }

        public override string GetNameDisplay()
        {
            return Name.Replace("{0}", WaitTime.TotalMilliseconds.ToString());
        }
    }
}
