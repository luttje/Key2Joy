using Newtonsoft.Json;
using System.Windows.Forms;

namespace KeyToJoy.Mapping
{
    internal class AppCommandAction : BaseAction
    {
        [JsonProperty]
        private string Command;

        public AppCommandAction(string name, string imagePath, string command)
            : base(name, imagePath)
        {
            this.Command = command;
        }

        internal override void PerformPressBind(bool inputKeyDown)
        {
            if (!inputKeyDown)
                return;

            if (!Program.RunAppCommand(Command))
            {
                MessageBox.Show("This app command is invalid or could not be run at this time!", "Invalid command!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        // TODO: Should we even support mouse input for app commands?
        internal override short PerformMoveBind(short inputMouseDelta, short currentAxisDelta)
        {
            this.PerformPressBind(true);

            return 0;
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
