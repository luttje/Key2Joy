using Newtonsoft.Json;
using SimWinInput;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyToJoy.Mapping
{
    [Action(
        Name = "GamePad/Controller Simulation",
        OptionsUserControl = typeof(GamePadActionControl)
    )]
    internal class GamePadAction : BaseAction
    {
        [JsonProperty]
        private GamePadControl Control;

        public GamePadAction(string name, string imagePath, GamePadControl control)
            : base(name, imagePath)
        {
            Control = control;
        }

        internal override async Task Execute(InputBag inputBag)
        {
            if (inputBag is MouseMoveInputBag mouseMoveInputBag)
            {
                // TODO: Sensitivity should be tweakable by user
                // TODO: Support non axis buttons when delta is over a threshold?
                var controllerId = 0;
                var state = SimGamePad.Instance.State[controllerId];

                switch (Control)
                {
                    case GamePadControl.LeftStickLeft:
                    case GamePadControl.LeftStickRight:
                        state.LeftStickX = (short)((mouseMoveInputBag.DeltaX + state.LeftStickX) / 2);
                        break;
                    case GamePadControl.LeftStickUp:
                    case GamePadControl.LeftStickDown:
                        state.LeftStickY = (short)((mouseMoveInputBag.DeltaY + state.LeftStickY) / 2);
                        break;
                    case GamePadControl.RightStickLeft:
                    case GamePadControl.RightStickRight:
                        state.RightStickX = (short)((mouseMoveInputBag.DeltaX + state.RightStickX) / 2);
                        break;
                    case GamePadControl.RightStickUp:
                    case GamePadControl.RightStickDown:
                        state.RightStickY = (short)((mouseMoveInputBag.DeltaY + state.RightStickY) / 2);
                        break;
                    default:
                        throw new NotImplementedException("This control does not (yet) support mouse axis input");
                }

                SimGamePad.Instance.Update(controllerId);
                
                return;
            }

            if ((inputBag is KeyboardInputBag keyboardInputBag && keyboardInputBag.State == Input.LowLevel.KeyboardState.KeyDown)
                || inputBag is MouseButtonInputBag)
                SimGamePad.Instance.SetControl(Control);
            else
                SimGamePad.Instance.ReleaseControl(Control);
        }

        public override string GetContextDisplay()
        {
            return "GamePad";
        }

        public override bool Equals(object obj)
        {
            if (!(obj is GamePadAction action))
                return false;

            return action.Control == Control;
        }
    }
}
