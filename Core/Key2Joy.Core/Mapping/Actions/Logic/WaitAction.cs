using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Contracts.Mapping.Triggers;

namespace Key2Joy.Mapping.Actions.Logic;

[Action(
    Description = "Wait for a specified duration",
    Visibility = MappingMenuVisibility.UnlessTopLevel,
    NameFormat = "Wait for {0}ms"
)]
public class WaitAction : CoreAction
{
    [JsonInclude]
    public TimeSpan WaitTime { get; set; }

    public WaitAction(string name)
        : base(name)
    {
    }

    public override Task Execute(AbstractInputBag inputBag = null) => Task.Delay(this.WaitTime);

    public override string GetNameDisplay() => this.Name.Replace("{0}", this.WaitTime.TotalMilliseconds.ToString());
}
