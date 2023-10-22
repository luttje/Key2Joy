using System;
using System.Threading.Tasks;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Contracts.Mapping.Triggers;

namespace Key2Joy.Mapping.Actions;

[Action(
    Description = "Disabled Action",
    NameFormat = DisabledNameFormat,
    Visibility = MappingMenuVisibility.Never,
    GroupName = "Requires Attention",
    GroupImage = "cross"
)]
public class DisabledAction : CoreAction
{
    private const string DisabledNameFormat = "The action '{0}' was unavailable upon loading Key2Joy. We have replaced it with this placeholder.";
    public string ActionName { get; set; }

    public DisabledAction(string name)
        : base(name)
    { }

    public override async Task Execute(AbstractInputBag inputBag = null)
    { }

    public override string GetNameDisplay() => DisabledNameFormat.Replace("{0}", this.ActionName);

    public override bool Equals(object obj)
    {
        if (obj is not DisabledAction action)
        {
            return false;
        }

        return action.ActionName == this.ActionName;
    }
}
