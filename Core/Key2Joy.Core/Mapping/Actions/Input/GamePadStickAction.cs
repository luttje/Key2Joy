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
        Description = "GamePad Stick Simulation",
        Visibility = MappingMenuVisibility.Never,
        NameFormat = "Move {0} Stick on GamePad #{3} by {1},{2}",
        GroupName = "GamePad Stick Simulation",
        GroupImage = "joystick"
    )]
    public class GamePadStickAction : CoreAction
    {

        public Simulator.GamePadStick Stick { get; set; }
        public double DeltaX { get; set; }
        public double DeltaY { get; set; }
        public int GamePadIndex { get; set; }

        public GamePadStickAction(string name)
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
        /// Simulate moving a gamepad joystick
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
        /// <param name="deltaX">The fraction by which to move the stick forward (negative) or backward (positive)</param>
        /// <param name="deltaY">The fraction by which to move the stick right (positive) or left (negative)</param>
        /// <param name="stick">Which gamepad stick to move, either GamePadStick.Left (default) or .Right</param>
        /// <param name="gamepadIndex">Which of 4 possible gamepads to simulate: 0 (default), 1, 2 or 3</param>
        /// <name>GamePad.SimulateMove</name>
        [ExposesScriptingMethod("GamePad.SimulateMove")]
        public async void ExecuteForScript(
            double deltaX,
            double deltaY,
            Simulator.GamePadStick stick = Simulator.GamePadStick.Left,
            int gamepadIndex = 0)
        {
            this.DeltaX = deltaX;
            this.DeltaY = deltaY;
            this.Stick = stick;
            this.GamePadIndex = gamepadIndex;

            GamePadManager.Instance.EnsurePluggedIn(this.GamePadIndex);

            await this.Execute();
        }

        public override async Task Execute(AbstractInputBag inputBag = null)
        {
            var simPad = SimGamePad.Instance;
            var state = simPad.State[this.GamePadIndex];

            var x = (short)(short.MinValue * this.DeltaX);
            var y = (short)(short.MinValue * this.DeltaY);

            if (this.Stick == Simulator.GamePadStick.Right)
            {
                state.RightStickX = x;
                state.RightStickY = y;
            }
            else
            {
                state.LeftStickX = x;
                state.LeftStickY = y;
            }

            simPad.Update(this.GamePadIndex);
        }

        public override string GetNameDisplay()
        {
            return this.Name.Replace("{0}", this.Stick == Simulator.GamePadStick.Left ? "Left" : "Right")
                .Replace("{1}", this.DeltaX.ToString())
                .Replace("{2}", this.DeltaY.ToString())
                .Replace("{3}", this.GamePadIndex.ToString());
        }

        public override bool Equals(object obj)
        {
            if (obj is not GamePadStickAction action)
            {
                return false;
            }

            return action.Stick == this.Stick
                && action.DeltaX == this.DeltaX
                && action.DeltaY == this.DeltaY;
        }
    }
}
