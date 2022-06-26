using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyToJoy.Mapping
{
    [Action(
        Description = "App Command",
        OptionsUserControl = typeof(AppCommandActionControl),
        NameFormat = "Run App Command '{0}'"
    )]
    internal class AppCommandAction : BaseAction
    {
        [JsonProperty]
        public string Command { get; set; }

        public AppCommandAction(string name, string description)
            : base(name, description)
        { }

        [ExposesScriptingMethod("AppCommand")]
        public void ExecuteForScript(string command)
        {
            Command = command;

            Task.Run(async () =>
            {
                // Wait a frame so we don't get an Access Violation on the lua.DoString
                // TODO: Figure out if there's a nicer way
                await Task.Delay(0);

                this.Execute();
            });
        }

        internal override async Task Execute(InputBag inputBag = null)
        {
            if (!Program.RunAppCommand(Command))
            {
                MessageBox.Show("This app command is invalid or could not be run at this time!", "Invalid command!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        public override string GetNameDisplay()
        {
            return Name.Replace("{0}", Command);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is AppCommandAction action))
                return false;

            return action.Command == Command;
        }

        public override object Clone()
        {
            return new AppCommandAction(Name, description)
            {
                Command = Command,
                ImageResource = ImageResource,
                Name = Name,
            };
        }
    }
}
