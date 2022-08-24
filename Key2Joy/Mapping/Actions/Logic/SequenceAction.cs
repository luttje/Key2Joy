using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Key2Joy.Mapping
{
    [Action(
        Description = "Multiple Actions in Sequence",
        Visibility = MappingMenuVisibility.OnlyTopLevel,
        OptionsUserControl = typeof(SequenceActionControl),
        NameFormat = "Run Sequence: {0}"
    )]
    [Util.ObjectListViewGroup(
        Name = "Logic",
        Image = "script_code"
    )]
    internal class SequenceAction : BaseAction
    {
        [JsonProperty(ItemTypeNameHandling = TypeNameHandling.All)]
        public List<BaseAction> ChildActions;

        public SequenceAction(string name, string description)
            : base(name, description)
        {
            ChildActions = new List<BaseAction>();
        }

        internal override async Task Execute(IInputBag inputBag = null)
        {
            foreach (var childAction in ChildActions)
                await childAction.Execute(inputBag);
        }

        internal override void OnStartListening(TriggerListener listener, ref List<BaseAction> otherActions)
        {
            base.OnStartListening(listener, ref otherActions);

            foreach (var childAction in ChildActions)
            {
                childAction.OnStartListening(listener, ref otherActions);
            }
        }

        internal override void OnStopListening(TriggerListener listener)
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

        public override object Clone()
        {
            return new SequenceAction(Name, description)
            {
                ChildActions = ChildActions,
                ImageResource = ImageResource,
                Name = Name,
            };
        }
    }
}
