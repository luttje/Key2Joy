using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Contracts.Mapping.Triggers;

namespace Key2Joy.Mapping.Actions.Logic;

[Action(
    Description = "Multiple Actions in Sequence",
    Visibility = MappingMenuVisibility.OnlyTopLevel,
    NameFormat = "Run Sequence: {0}",
    GroupName = "Logic",
    GroupImage = "application_xp_terminal"
)]
public class SequenceAction : CoreAction
{
    public IList<AbstractAction> ChildActions { get; set; }

    public SequenceAction(string name)
        : base(name) => this.ChildActions = new List<AbstractAction>();

    public override async Task Execute(AbstractInputBag inputBag = null)
    {
        foreach (var childAction in this.ChildActions)
        {
            await childAction.Execute(inputBag);
        }
    }

    public override void OnStartListening(AbstractTriggerListener listener, ref IList<AbstractAction> otherActions)
    {
        base.OnStartListening(listener, ref otherActions);

        foreach (var childAction in this.ChildActions)
        {
            childAction.OnStartListening(listener, ref otherActions);
        }
    }

    public override void OnStopListening(AbstractTriggerListener listener)
    {
        base.OnStopListening(listener);

        foreach (var childAction in this.ChildActions)
        {
            childAction.OnStopListening(listener);
        }
    }

    public override string GetNameDisplay()
    {
        StringBuilder actions = new();

        for (var i = 0; i < this.ChildActions.Count; i++)
        {
            if (i > 0)
            {
                actions.Append(", ");
            }

            actions.Append(this.ChildActions[i].GetNameDisplay());
        }

        return this.Name.Replace("{0}", actions.ToString());
    }

    public override bool Equals(object obj)
    {
        if (obj is not SequenceAction action)
        {
            return false;
        }

        return action.Name == this.Name;
    }
}
