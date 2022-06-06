using Newtonsoft.Json;
using SimWinInput;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyToJoy.Input
{
    internal class GamePadAction : BindableAction
    {
        [JsonProperty]
        private GamePadControl Control;

        public GamePadAction(string imagePath, GamePadControl control)
            : base(imagePath)
        {
            this.Control = control;
        }

        public override string ToString()
        {
            return Control.ToString();
        }

        internal override void PerformPressBind(bool inputKeyDown)
        {
            if (inputKeyDown)
                SimGamePad.Instance.SetControl(Control);
            else
                SimGamePad.Instance.ReleaseControl(Control);
        }

        internal override short PerformMoveBind(short inputMouseDelta, short currentAxisDelta)
        {
            return (short)((inputMouseDelta + currentAxisDelta) / 2);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is GamePadAction action))
                return false;

            return action.Control == Control;
        }
    }
}
