using Newtonsoft.Json;
using SimWinInput;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyToJoy.Input
{
    internal class AppCommandAction : BindableAction
    {
        [JsonProperty]
        private string Command;

        private string Description;

        public AppCommandAction(string imagePath, string command, string description)
            : base(imagePath)
        {
            this.Command = command;
            this.Description = description;
        }

        public override string ToString()
        {
            return Command.ToString();
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

        public override bool Equals(object obj)
        {
            if (!(obj is AppCommandAction action))
                return false;

            return action.Command == Command;
        }
    }
}
