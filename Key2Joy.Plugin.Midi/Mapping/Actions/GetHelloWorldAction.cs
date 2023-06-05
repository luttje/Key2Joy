﻿using Key2Joy.Contracts.Mapping;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System;
using Key2Joy.Contracts.Util;
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
        [JsonProperty]
        public string Target = "World";


        [ExposesScriptingMethod("Hello.World")]
        public override async Task Execute(IInputBag inputBag = null)
        {
            MessageBox.Show("test");
            //Output.WriteLine(Output.OutputModes.Verbose, $"Hello {Target}!");
        }

        public override string GetNameDisplay()
        {
            return Name.Replace("{0}", Target.ToString());
        }

        public override object Clone()
        {
            return new GetHelloWorldAction()
            {
                Name = Name,
                Target = Target,
            };
        }
    }
}
