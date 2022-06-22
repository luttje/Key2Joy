using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyToJoy.Mapping
{
    [Action(
        Name = "App Commands",
        OptionsUserControl = typeof(AppCommandActionControl)
    )]
    internal class AppCommandAction : BaseAction
    {
        [JsonProperty]
        public string Command { get; set; }

        public AppCommandAction(string name, string imagePath)
            : base(name, imagePath)
        { }

        internal override async Task Execute(InputBag inputBag)
        {
            if (!Program.RunAppCommand(Command))
            {
                MessageBox.Show("This app command is invalid or could not be run at this time!", "Invalid command!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        public override string GetContextDisplay()
        {
            return "App";
        }

        public override bool Equals(object obj)
        {
            if (!(obj is AppCommandAction action))
                return false;

            return action.Command == Command;
        }
    }
}
