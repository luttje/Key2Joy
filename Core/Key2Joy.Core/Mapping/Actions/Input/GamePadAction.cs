using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Contracts.Mapping.Triggers;
using Key2Joy.LowLevelInput;
using Key2Joy.Mapping.Triggers.Mouse;
using SimWinInput;

namespace Key2Joy.Mapping.Actions.Input
{
    [Action(
        Description = "GamePad/Controller Simulation",
        NameFormat = "{1} {0} on GamePad #{2}",
        GroupName = "GamePad Simulation",
        GroupImage = "joystick"
    )]
    public class GamePadAction : CoreAction, IPressState
    {
        public GamePadControl Control { get; set; }
        public PressState PressState { get; set; }
        public int GamePadIndex { get; set; }

        public GamePadAction(string name)
            : base(name)
        {
        }

        public static List<MappedOption> GetAllButtonActions(PressState pressState)
        {
            var actionFactory = ActionsRepository.GetAction(typeof(GamePadAction));

            List<MappedOption> actions = new();
            foreach (var control in GetAllButtons())
            {
                var action = (GamePadAction)MakeAction(actionFactory);
                action.Control = control;
                action.PressState = pressState;

                actions.Add(new MappedOption
                {
                    Action = action
                });
            }
            return actions;
        }

        public static GamePadControl[] GetAllButtons()
        {
            var allEnums = Enum.GetValues(typeof(GamePadControl));

            // Skip the first (= None) enumeration value
            var buttons = new GamePadControl[allEnums.Length - 1];
            Array.Copy(allEnums, 1, buttons, 0, buttons.Length);

            return buttons;
        }

        public override void OnStartListening(AbstractTriggerListener listener, ref IList<AbstractAction> otherActions)
        {
            base.OnStartListening(listener, ref otherActions);

            GamePadManager.Instance.EnsurePluggedIn(this.GamePadIndex);
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
        /// <param name="gamepadIndex">Which of 4 possible gamepads to simulate: 0 (default), 1, 2 or 3</param>
        /// <name>GamePad.Simulate</name>
        [ExposesScriptingMethod("GamePad.Simulate")]
        public async void ExecuteForScript(GamePadControl control, PressState pressState, int gamepadIndex = 0)
        {
            this.Control = control;
            this.PressState = pressState;
            this.GamePadIndex = gamepadIndex;

            GamePadManager.Instance.EnsurePluggedIn(this.GamePadIndex);

            if (this.PressState == PressState.Press)
            {
                SimGamePad.Instance.SetControl(this.Control, this.GamePadIndex);
            }

            if (this.PressState == PressState.Release)
            {
                SimGamePad.Instance.ReleaseControl(this.Control, this.GamePadIndex);
            }
        }

        public override async Task Execute(AbstractInputBag inputBag = null)
        {
            if (inputBag is MouseMoveInputBag mouseMoveInputBag)
            {
                // TODO: Sensitivity should be tweakable by user
                // TODO: Support non axis buttons when delta is over a threshold?
                var state = SimGamePad.Instance.State[this.GamePadIndex];

                switch (this.Control)
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

                SimGamePad.Instance.Update(this.GamePadIndex);

                return;
            }

            if (this.PressState == PressState.Press)
            {
                SimGamePad.Instance.SetControl(this.Control, this.GamePadIndex);
            }
            else if (this.PressState == PressState.Release)
            {
                SimGamePad.Instance.ReleaseControl(this.Control, this.GamePadIndex);
            }
        }

        public override string GetNameDisplay()
        {
            return this.Name.Replace("{0}", this.Control.ToString())
                .Replace("{1}", Enum.GetName(typeof(PressState), this.PressState))
                .Replace("{2}", this.GamePadIndex.ToString());
        }

        public override bool Equals(object obj)
        {
            if (obj is not GamePadAction action)
            {
                return false;
            }

            return action.Control == this.Control
                && action.PressState == this.PressState;
        }
    }
}
