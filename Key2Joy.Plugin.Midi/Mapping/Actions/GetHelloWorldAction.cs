using Key2Joy.Mapping;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Key2Joy.Plugin.Midi.Mapping
{
    [Action(
        Description = "Get Hello World",
        NameFormat = "Demonstrates plugin creation by greeting {0}"
    )]
    [Util.ObjectListViewGroup(
        Name = "Logic",
        Image = "script_code"
    )]
    public class GetHelloWorldAction : BaseAction
    {
        [JsonProperty]
        public string Target = "World";

        public GetHelloWorldAction(string name, string description)
            : base(name, description)
        { }

        public override async Task Execute(IInputBag inputBag = null)
        {
            Output.WriteLine(Output.OutputModes.Verbose, $"Hello {Target}!");
        }

        public override string GetNameDisplay()
        {
            return Name.Replace("{0}", Target.ToString());
        }

        public override bool Equals(object obj)
        {
            if (!(obj is GetHelloWorldAction action))
                return false;

            return true;
        }

        public override object Clone()
        {
            return new GetHelloWorldAction(Name, description)
            {
                ImageResource = ImageResource,
                Target = Target,
                Name = Name,
            };
        }
    }
}
