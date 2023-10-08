using Key2Joy.Contracts.Mapping;
using System.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using Key2Joy.Contracts.Plugins;
using System.IO;

namespace Key2Joy.Plugin.HelloWorld.Mapping
{
    [Action(
        Description = "Get Hello World",
        NameFormat = "Demonstrates plugin creation by greeting '{0}'",
        GroupName = "Logic",
        GroupImage = "application_xp_terminal"
    )]
    public class GetHelloWorldAction : PluginAction
    {
        public string Target { get; set; } = "World";

        public async Task Execute(AbstractInputBag inputBag = null)
        {
            MessageBox.Show($"Hello {Target}!");
            Debug.WriteLine($"Hello {Target}!");
            //Output.WriteLine(Output.OutputModes.Verbose, $"Hello {Target}!");
        }

        [ExposesScriptingMethod("Hello.World")]
        public void ExecuteForScript(string target)
        {
            var fileWriter = File.AppendText(Path.Combine(Plugin.PluginDataDirectory, "test.txt"));
            fileWriter.AutoFlush = true;
            fileWriter.WriteLine($"Hello {target}!");
            fileWriter.Close();
            //MessageBox.Show($"Hello {target} / {Target}!"); // Very laggy for some reason
        }
        
        public override MappingAspectOptions BuildSaveOptions(MappingAspectOptions options)
        {
            options.Add("Target", Target);

            return options;
        }

        public override void LoadOptions(MappingAspectOptions options)
        {
            if (options.TryGetValue("Target", out var target))
            {
                Target = (string)target;
            }
        }

        public override string GetNameDisplay(string nameFormat)
        {
            return string.Format(nameFormat, Target);
        }
    }
}
