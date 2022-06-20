using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyToJoy.Mapping
{
    internal class SequenceAction : BaseAction
    {
        [JsonProperty(ItemTypeNameHandling = TypeNameHandling.All)]
        public List<BaseAction> ChildActions;

        public SequenceAction(string name, string imagePath, [JsonProperty(ItemTypeNameHandling = TypeNameHandling.All)] List<BaseAction> childActions)
            : base(name, imagePath)
        {
            this.ChildActions = childActions;
        }

        internal override async Task Execute(InputBag inputBag)
        {
            foreach (var childAction in ChildActions)
                await childAction.Execute(inputBag);
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
            
            return $"Sequence: {actions}";
        }

        public override string GetContextDisplay()
        {
            return "Logic";
        }

        public override bool Equals(object obj)
        {
            if (!(obj is SequenceAction action))
                return false;

            return action.Name == Name;
        }
    }
}
