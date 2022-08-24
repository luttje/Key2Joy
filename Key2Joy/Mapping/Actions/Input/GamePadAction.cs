using Key2Joy.LowLevelInput;
using Newtonsoft.Json;
using SimWinInput;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Mapping
{
    [Action(
        Description = "GamePad/Controller Simulation",
        OptionsUserControl = typeof(GamePadActionControl),
        NameFormat = "{1} {0} on GamePad"
    )]
    [ExposesScriptingEnumeration(typeof(GamePadControl))]
    [ExposesScriptingEnumeration(typeof(PressState))]
    [Util.ObjectListViewGroup(
        Name = "GamePad Simulation",
        Image = "joystick"
    )]
    internal class GamePadAction : BaseAction, IPressState
    {
        [JsonProperty]
        public GamePadControl Control { get; set; }

        [JsonProperty]
        public PressState PressState { get; set; }

        [JsonProperty]
        public int GamePadIndex { get; set; }

        public GamePadAction(string name, string description)
            : base(name, description)
        {
        }

        public static List<MappedOption> GetAllButtonActions(PressState pressState)
        {
            var actionType = typeof(GamePadAction);
            var typeAttribute = ((ActionAttribute[])actionType.GetCustomAttributes(typeof(ActionAttribute), false))[0];

            var actions = new List<MappedOption>();
            foreach (var control in GetAllButtons())
            {
                var action = (GamePadAction)MakeAction(actionType, typeAttribute);
                action.Control = control;
                action.PressState = pressState;
                
                actions.Add(new MappedOption
                {
                    Action = action
                });
            }
            return actions;
        }

        internal static GamePadControl[] GetAllButtons()
        {
            var allEnums = Enum.GetValues(typeof(GamePadControl));

            // Skip the first (= None) enumeration value
            var buttons = new GamePadControl[allEnums.Length - 1];
            Array.Copy(allEnums, 1, buttons, 0, buttons.Length);

            return buttons;
        }

        internal override void OnStartListening(TriggerListener listener, ref List<BaseAction> otherActions)
        {
            base.OnStartListening(listener, ref otherActions);

            GamePadManager.Instance.EnsurePluggedIn(GamePadIndex);
        }

        /// <markdown-doc>
        /// <parent-name>Input</parent-name>
        /// <path>Api/Input</path>
        /// </markdown-doc>
        /// <summary>
        /// Simulate pressing or releasing (or both) gamepad buttons.
        /// </summary>
        /// <markdown-example>
        /// Shows how to press "A" on the gamepad for 500ms, then release it.
        /// <code language="js">
        /// <![CDATA[
        /// GamePad.Simulate(GamePadControl.A, PressState.Press);
        /// setTimeout(function () {
        ///     GamePad.Simulate(GamePadControl.A, PressState.Release);
        /// }, 500);
        /// ]]>
        /// </code>
        /// </markdown-example>
        /// <param name="control">Button to simulate</param>
        /// <param name="pressState">Action to simulate</param>
        /// <param name="gamepadIndex">Which of 4 possible gamepads to simulate (0, 1, 2 or 3)</param>
        /// <name>GamePad.Simulate</name>
        [ExposesScriptingMethod("GamePad.Simulate")]
        public async void ExecuteForScript(GamePadControl control, PressState pressState, int gamepadIndex = 0)
        {
            Control = control;
            PressState = pressState;
            GamePadIndex = gamepadIndex;

            GamePadManager.Instance.EnsurePluggedIn(GamePadIndex);

            if (PressState == PressState.Press)
                SimGamePad.Instance.SetControl(Control, GamePadIndex);
            
            if (PressState == PressState.Release)
                SimGamePad.Instance.ReleaseControl(Control, GamePadIndex);
        }

        internal override async Task Execute(IInputBag inputBag = null)
        {
            if (inputBag is MouseMoveInputBag mouseMoveInputBag)
            {
                // TODO: Sensitivity should be tweakable by user
                // TODO: Support non axis buttons when delta is over a threshold?
                var state = SimGamePad.Instance.State[GamePadIndex];

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

                SimGamePad.Instance.Update(GamePadIndex);
                
                return;
            }

            if (PressState == PressState.Press)
                SimGamePad.Instance.SetControl(Control, GamePadIndex);
            else if(PressState == PressState.Release)
                SimGamePad.Instance.ReleaseControl(Control, GamePadIndex);
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

            return action.Control == Control
                && action.PressState == PressState;
        }

        public override object Clone()
        {
            return new GamePadAction(Name, description)
            {
                Control = Control,
                PressState = PressState,
                GamePadIndex = GamePadIndex,
                ImageResource = ImageResource,
                Name = Name,
            };
        }
    }
}
