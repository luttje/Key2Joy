using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Actions;
using Key2Joy.Contracts.Mapping.Triggers;
using Key2Joy.LowLevelInput;
using SimWinInput;

namespace Key2Joy.Mapping.Actions.Input
{
    [Action(
        Description = "GamePad Reset Simulation",
        Visibility = MappingMenuVisibility.Never,
        NameFormat = "Reset GamePad #{0}",
        GroupName = "GamePad Reset Simulation",
        GroupImage = "joystick"
    )]
    public class GamePadResetAction : CoreAction
    {
        public int GamePadIndex { get; set; }

        public GamePadResetAction(string name)
            : base(name)
        {
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
        /// Reset the gamepad so the stick returns to the resting position (0,0)
        /// </summary>
        /// <markdown-example>
        /// Moves the left gamepad joystick halfway down and to the right, then resets after 500ms
        /// <code language="lua">
        /// <![CDATA[
        /// GamePad.SimulateMove(0.5,0.5)
        /// SetTimeout(function()
        ///    GamePad.Reset()
        /// end, 500)
        /// ]]>
        /// </code>
        /// </markdown-example>
        /// <param name="gamepadIndex">Which of 4 possible gamepads to simulate: 0 (default), 1, 2 or 3</param>
        /// <name>GamePad.Reset</name>
        [ExposesScriptingMethod("GamePad.Reset")]
        public async void ExecuteForScript(int gamepadIndex = 0)
        {
            this.GamePadIndex = gamepadIndex;

            GamePadManager.Instance.EnsurePluggedIn(this.GamePadIndex);

            var simPad = SimGamePad.Instance;
            var state = simPad.State[this.GamePadIndex];
            state.Reset();
            simPad.Update(this.GamePadIndex);
        }

        public override async Task Execute(AbstractInputBag inputBag = null)
        {
            var simPad = SimGamePad.Instance;
            var state = simPad.State[this.GamePadIndex];
            state.Reset();
            simPad.Update(this.GamePadIndex);
        }

        public override string GetNameDisplay()
        {
            return this.Name.Replace("{0}", this.GamePadIndex.ToString());
        }

        public override bool Equals(object obj)
        {
            if (obj is not GamePadResetAction)
            {
                return false;
            }

            return true;
        }
    }
}
