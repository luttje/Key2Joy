using Newtonsoft.Json;
using System.Collections.Generic;
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

        public override string GetContextDisplay()
        {
            return "Logic";
        }
    }
}
