using Key2Joy.Contracts.Mapping;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Mapping
{
    [Action(
        Description = "Multiple Actions in Sequence",
        Visibility = MappingMenuVisibility.OnlyTopLevel,
        NameFormat = "Run Sequence: {0}",
        GroupName = "Logic",
        GroupImage = "application_xp_terminal"
    )]
    public class SequenceAction : CoreAction
    {
        // TODO: [JsonProperty(ItemTypeNameHandling = TypeNameHandling.All)]
        public IList<AbstractAction> ChildActions;

        public SequenceAction(string name)
            : base(name)
        {
            ChildActions = new List<AbstractAction>();
        }

        public override async Task Execute(AbstractInputBag inputBag = null)
        {
            foreach (var childAction in ChildActions)
                await childAction.Execute(inputBag);
        }

        public override void OnStartListening(AbstractTriggerListener listener, ref IList<AbstractAction> otherActions)
        {
            base.OnStartListening(listener, ref otherActions);

            foreach (var childAction in ChildActions)
            {
                childAction.OnStartListening(listener, ref otherActions);
            }
        }

        public override void OnStopListening(AbstractTriggerListener listener)
        {
            base.OnStopListening(listener);

            foreach (var childAction in ChildActions)
            {
                childAction.OnStopListening(listener);
            }
        }

        public override string GetNameDisplay()
        {
            var actions = new StringBuilder();

            for (int i = 0; i < ChildActions.Count; i++)
            {
                if (i > 0)
                    actions.Append(", ");

                actions.Append(ChildActions[i].GetNameDisplay());
            }

            return Name.Replace("{0}", actions.ToString());
        }

        public override bool Equals(object obj)
        {
            if (!(obj is SequenceAction action))
                return false;

            return action.Name == Name;
        }
    }
}
