using Key2Joy.Contracts.Mapping;
using System.Threading.Tasks;
using System;
using Key2Joy.Contracts.Util;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Windows.Forms;

namespace Key2Joy.Plugin.Midi.Mapping
{
    [Action(
        Description = "Get Hello World",
        NameFormat = "Demonstrates plugin creation by greeting {0}"
    )]
    [ObjectListViewGroup(
        Name = "Logic",
        Image = "script_code"
    )]
    public class GetHelloWorldAction : AbstractAction
    {
        public string Target { get; set; } = "World";

        public GetHelloWorldAction(string name)
            : base(name)
        {

        }

        [ExposesScriptingMethod("Hello.World")]
        public override async Task Execute(AbstractInputBag inputBag = null)
        {
            MessageBox.Show("test");
            //Output.WriteLine(Output.OutputModes.Verbose, $"Hello {Target}!");
        }

        public override string GetNameDisplay()
        {
            return Name.Replace("{0}", Target.ToString());
        }
    }
}
