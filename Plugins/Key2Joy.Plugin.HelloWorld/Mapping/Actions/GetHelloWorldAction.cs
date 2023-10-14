using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Contracts.Mapping.Triggers;
using Key2Joy.Contracts.Plugins;

namespace Key2Joy.Plugin.HelloWorld.Mapping.Actions;

public enum Test
{
    IsNoTest = 0,
    IsTest = 1,
}

[ExposesEnumeration(typeof(Test))]
[Action(
    Description = "Get Hello World",
    NameFormat = "Demonstrates plugin creation by greeting '{0}'",
    GroupName = "Logic",
    GroupImage = "application_xp_terminal"
)]
public class GetHelloWorldAction : PluginAction
{
    public string Target { get; set; } = "World";

    public override void Execute(AbstractInputBag inputBag = null)
    {
        MessageBox.Show($"Hello {this.Target}!");
        Debug.WriteLine($"Hello {this.Target}!");
        //Output.WriteLine(Output.OutputModes.Verbose, $"Hello {Target}!");
    }

    [ExposesScriptingMethod("Hello.World")]
    public void ExecuteForScript(string target)
    {
        var fileWriter = File.AppendText(Path.Combine(this.Plugin.PluginDataDirectory, "test.txt"));
        fileWriter.AutoFlush = true;
        fileWriter.WriteLine($"Hello {target}!");
        fileWriter.Close();
        //MessageBox.Show($"Hello {target} / {Target}!"); // Very laggy for some reason
    }

    public override MappingAspectOptions BuildSaveOptions(MappingAspectOptions options)
    {
        options.Add("Target", this.Target);

        return options;
    }

    public override void LoadOptions(MappingAspectOptions options)
    {
        if (options.TryGetValue("Target", out var target))
        {
            this.Target = (string)target;
        }
    }

    public override string GetNameDisplay(string nameFormat) => string.Format(nameFormat, this.Target);
}
