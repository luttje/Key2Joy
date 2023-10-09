using System;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Contracts.Mapping.Triggers;

namespace Key2Joy.Mapping.Actions.Logic;

[Action(
    Description = "Shows a MessageBox containing text",
    NameFormat = "Show MessageBox with '{0}' for text",
    GroupName = "Logic",
    GroupImage = "application_xp_terminal"
)]
public class MessageBoxAction : CoreAction
{
    [JsonInclude]
    public string Content { get; set; } = "Hello World!";


    public MessageBoxAction(string name)
        : base(name)
    { }

    /// <markdown-doc>
    /// <parent-name>Logic</parent-name>
    /// <path>Api/Logic</path>
    /// </markdown-doc>
    /// <summary>
    /// Displays a MessageBox with the given text
    /// </summary>
    /// <param name="content">The text to display</param>
    /// <name>MessageBox.Show</name>
    [ExposesScriptingMethod("MessageBox.Show")]
    public void ExecuteForScript(string content) => System.Windows.MessageBox.Show(content);

    public override async Task Execute(AbstractInputBag inputBag = null) => System.Windows.MessageBox.Show(this.Content);

    public override string GetNameDisplay() => this.Name.Replace("{0}", this.Content);

    public override bool Equals(object obj)
    {
        if (obj is not MessageBoxAction)
        {
            return false;
        }

        return this.Content == ((MessageBoxAction)obj).Content;
    }
}
