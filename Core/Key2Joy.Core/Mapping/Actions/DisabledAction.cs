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
    private const string DisabledNameFormat = "The action '{0}' was unavailable upon loading Key2Joy. The error that caused this was: {1}";
    public string ActionName { get; set; }

    public DisabledAction(string name)
        : base(name)
    { }

    /// <inheritdoc/>
    public override async Task Execute(AbstractInputBag inputBag = null)
    { }

    /// <inheritdoc/>
    public override string GetNameDisplay()
        => DisabledNameFormat
            .Replace("{0}", this.ActionName)
            .Replace("{1}", this.Name);

    /// <inheritdoc/>
    public override bool Equals(object obj)
    {
        if (obj is not DisabledAction action)
        {
            return false;
        }

        return action.ActionName == this.ActionName;
    }
}
