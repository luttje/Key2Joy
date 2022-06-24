using KeyToJoy.Input;
using KeyToJoy.Input.LowLevel;
using Newtonsoft.Json;
using SimWinInput;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyToJoy.Mapping
{
    [Action(
        Description = "GamePad/Controller Simulation",
        OptionsUserControl = typeof(GamePadActionControl),
        NameFormat = "{1} {0} on GamePad",
        FunctionName = SCRIPT_COMMAND,
        FunctionMethodName = nameof(GamePadAction.ExecuteActionForScript),
        ExposesEnumerations = new[] { typeof(GamePadControl), typeof(PressState) }
    )]
    internal class GamePadAction : BaseAction
    {
        internal const string SCRIPT_COMMAND = "gamepad";
        
        private static bool isPluggedIn = false;
        private GamePadControl control;

        [JsonProperty]
        public GamePadControl Control
        {
            get { return control; }
            set
            {
                control = value;
                OnControlChanged();
            }
        }

        [JsonProperty]
        public PressState PressState { get; set; }

        public GamePadAction(string name, string description)
            : base(name, description)
        {
        }

        internal override void OnStartListening(TriggerListener listener, ref List<BaseAction> otherActions)
        {
            base.OnStartListening(listener, ref otherActions);

            if (isPluggedIn)
                return;
            
            SimGamePad.Instance.PlugIn();
            isPluggedIn = true;
        }

        internal override void OnStopListening(TriggerListener listener)
        {
            base.OnStopListening(listener);

            if (!isPluggedIn)
                return;
            
            SimGamePad.Instance.Unplug();
            isPluggedIn = false;
        }

        private void OnControlChanged()
        {
            switch (Control)
            {
                case GamePadControl.A:
                    ImageResource = "XboxSeriesX_A";
                    break;
                case GamePadControl.B:
                    ImageResource = "XboxSeriesX_B";
                    break;
                case GamePadControl.Back:
                    ImageResource = "XboxSeriesX_Back";
                    break;
                case GamePadControl.DPadDown:
                    ImageResource = "XboxSeriesX_DPadDown";
                    break;
                case GamePadControl.DPadLeft:
                    ImageResource = "XboxSeriesX_DPadLeft";
                    break;
                case GamePadControl.DPadRight:
                    ImageResource = "XboxSeriesX_DPadRight";
                    break;
                case GamePadControl.DPadUp:
                    ImageResource = "XboxSeriesX_DPadUp";
                    break;
                case GamePadControl.LeftShoulder:
                    ImageResource = "XboxSeriesX_LeftShoulder";
                    break;
                case GamePadControl.LeftStickClick:
                    ImageResource = "XboxSeriesX_LeftStickClick";
                    break;
                case GamePadControl.LeftStickDown:
                    ImageResource = "XboxSeriesX_LeftStickDown";
                    break;
                case GamePadControl.LeftStickLeft:
                    ImageResource = "XboxSeriesX_LeftStickLeft";
                    break;
                case GamePadControl.LeftStickRight:
                    ImageResource = "XboxSeriesX_LeftStickRight";
                    break;
                case GamePadControl.LeftStickUp:
                    ImageResource = "XboxSeriesX_LeftStickUp";
                    break;
                case GamePadControl.LeftTrigger:
                    ImageResource = "XboxSeriesX_LeftTrigger";
                    break;
                case GamePadControl.RightShoulder:
                    ImageResource = "XboxSeriesX_RightShoulder";
                    break;
                case GamePadControl.RightStickClick:
                    ImageResource = "XboxSeriesX_RightStickClick";
                    break;
                case GamePadControl.RightStickDown:
                    ImageResource = "XboxSeriesX_RightStickDown";
                    break;
                case GamePadControl.RightStickLeft:
                    ImageResource = "XboxSeriesX_RightStickLeft";
                    break;
                case GamePadControl.RightStickRight:
                    ImageResource = "XboxSeriesX_RightStickRight";
                    break;
                case GamePadControl.RightStickUp:
                    ImageResource = "XboxSeriesX_RightStickUp";
                    break;
                case GamePadControl.RightTrigger:
                    ImageResource = "XboxSeriesX_RightTrigger";
                    break;
                case GamePadControl.Start:
                    ImageResource = "XboxSeriesX_Start";
                    break;
                case GamePadControl.X:
                    ImageResource = "XboxSeriesX_X";
                    break;
                case GamePadControl.Y:
                    ImageResource = "XboxSeriesX_Y";
                    break;
            }
        }

        public object[] ExecuteActionForScript(params object[] parameters)
        {
            if (parameters.Length < 2)
                throw new ArgumentException($"{SCRIPT_COMMAND} expected a gamepad control and press state!");

            if (!BaseScriptAction.TryConvertParameterToLong(parameters[0], out long controlNumber))
                throw new ArgumentException($"{SCRIPT_COMMAND} expected a gamepad control as the first argument!");

            control = (GamePadControl)controlNumber;

            if (!BaseScriptAction.TryConvertParameterToLong(parameters[1], out long pressStateNumber))
                throw new ArgumentException($"{SCRIPT_COMMAND} expected a press state as the second argument!");

            PressState = (PressState)pressStateNumber;

            if (PressState == PressState.Press || PressState == PressState.PressAndRelease)
            {
                SimGamePad.Instance.SetControl(Control);

                if (PressState == PressState.PressAndRelease)
                {
                    Task.Run(async () =>
                    {
                        // TODO: Make this a configurable value
                        await Task.Delay(50);
                        SimGamePad.Instance.ReleaseControl(Control);
                    });
                }
            }
            
            if (PressState == PressState.Release)
                SimGamePad.Instance.ReleaseControl(Control);
            
            return null;
        }

        internal override async Task Execute(InputBag inputBag = null)
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

            bool isInputDown = false;
            
            if (inputBag is KeyboardInputBag keyboardInputBag)
                isInputDown = keyboardInputBag.State == KeyboardState.KeyDown;
            
            if (inputBag is MouseButtonInputBag mouseButtonInputBag)
                isInputDown = mouseButtonInputBag.IsDown;

            if (PressState == PressState.Press
                || (PressState == PressState.PressAndRelease && isInputDown))
                SimGamePad.Instance.SetControl(Control);
            else if(PressState == PressState.Release
                || (PressState == PressState.PressAndRelease && !isInputDown))
                SimGamePad.Instance.ReleaseControl(Control);
        }

        public override string GetNameDisplay()
        {
            return Name.Replace("{0}", Control.ToString())
                .Replace("{1}", Enum.GetName(typeof(PressState), PressState));
        }

        public override bool Equals(object obj)
        {
            if (!(obj is GamePadAction action))
                return false;

            return action.Control == Control;
        }

        public override object Clone()
        {
            return new GamePadAction(Name, description)
            {
                Control = Control,
                ImageResource = ImageResource,
                Name = Name,
            };
        }
    }
}
