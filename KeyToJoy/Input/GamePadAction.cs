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

        public static GamePadAction LeftShoulder = new GamePadAction("XboxSeriesX_LB", GamePadControl.LeftShoulder);
        public static GamePadAction LeftTrigger = new GamePadAction("XboxSeriesX_LT", GamePadControl.LeftTrigger);
        public static GamePadAction RightShoulder = new GamePadAction("XboxSeriesX_RB", GamePadControl.RightShoulder);
        public static GamePadAction RightTrigger = new GamePadAction("XboxSeriesX_RT", GamePadControl.RightTrigger);

        public static GamePadAction LeftStickUp = new GamePadAction("XboxSeriesX_Left_Stick_Up", GamePadControl.LeftStickUp);
        public static GamePadAction LeftStickRight = new GamePadAction("XboxSeriesX_Left_Stick_Right", GamePadControl.LeftStickRight);
        public static GamePadAction LeftStickDown = new GamePadAction("XboxSeriesX_Left_Stick_Down", GamePadControl.LeftStickDown);
        public static GamePadAction LeftStickLeft = new GamePadAction("XboxSeriesX_Left_Stick_Left", GamePadControl.LeftStickLeft);
        public static GamePadAction LeftStickClick = new GamePadAction("XboxSeriesX_Left_Stick_Click", GamePadControl.LeftStickClick);
        public static GamePadAction DPadUp = new GamePadAction("XboxSeriesX_Dpad_Up", GamePadControl.DPadUp);
        public static GamePadAction DPadRight = new GamePadAction("XboxSeriesX_Dpad_Right", GamePadControl.DPadRight);
        public static GamePadAction DPadDown = new GamePadAction("XboxSeriesX_Dpad_Down", GamePadControl.DPadDown);
        public static GamePadAction DPadLeft = new GamePadAction("XboxSeriesX_Dpad_Left", GamePadControl.DPadLeft);

        public static GamePadAction RightStickUp = new GamePadAction("XboxSeriesX_Right_Stick_Up", GamePadControl.RightStickUp);
        public static GamePadAction RightStickRight = new GamePadAction("XboxSeriesX_Right_Stick_Right", GamePadControl.RightStickRight);
        public static GamePadAction RightStickDown = new GamePadAction("XboxSeriesX_Right_Stick_Down", GamePadControl.RightStickDown);
        public static GamePadAction RightStickLeft = new GamePadAction("XboxSeriesX_Right_Stick_Left", GamePadControl.RightStickLeft);
        public static GamePadAction RightStickClick = new GamePadAction("XboxSeriesX_Right_Stick_Click", GamePadControl.RightStickClick);

        public static GamePadAction X = new GamePadAction("XboxSeriesX_X", GamePadControl.X);
        public static GamePadAction Y = new GamePadAction("XboxSeriesX_Y", GamePadControl.Y);
        public static GamePadAction A = new GamePadAction("XboxSeriesX_A", GamePadControl.A);
        public static GamePadAction B = new GamePadAction("XboxSeriesX_B", GamePadControl.B);

    }
}
