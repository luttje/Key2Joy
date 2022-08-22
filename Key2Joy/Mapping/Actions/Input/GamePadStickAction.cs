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
using System.Windows.Forms;

namespace Key2Joy.Mapping
{
    [Action(
        Description = "GamePad Stick Simulation",
        Visibility = ActionVisibility.Never,
        NameFormat = "Move GamePad stick {0} by {1},{2}"
    )]
    [ExposesScriptingEnumeration(typeof(Simulator.GamePadStick))]
    [Util.ObjectListViewGroup(
        Name = "GamePad Stick Simulation",
        Image = "joystick"
    )]
    internal class GamePadStickAction : BaseAction
    {

        [JsonProperty]
        public Simulator.GamePadStick Stick { get; set; }

        [JsonProperty]
        public double DeltaX { get; set; }
        
        [JsonProperty]
        public double DeltaY { get; set; }

        public GamePadStickAction(string name, string description)
            : base(name, description)
        {
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
        /// <name>GamePad.SimulateMove</name>
        [ExposesScriptingMethod("GamePad.SimulateMove")]
        public async void ExecuteForScript(double deltaX, double deltaY, Simulator.GamePadStick stick = Simulator.GamePadStick.Left)
        {
            DeltaX = deltaX;
            DeltaY = deltaY;
            Stick = stick;
            
            await Execute();
        }

        internal override async Task Execute(InputBag inputBag = null)
        {
            var simPad = SimGamePad.Instance;
            var targetGamePad = 0; // TODO: Support multiple gamepads
            var state = simPad.State[targetGamePad];

            var x = (short)(short.MinValue * DeltaX);
            var y = (short)(short.MinValue * DeltaY);

            if(Stick == Simulator.GamePadStick.Right) 
            { 
                state.RightStickX = x;
                state.RightStickY = y;
            }
            else
            {
                state.LeftStickX = x;
                state.LeftStickY = y;
            }

            simPad.Update(targetGamePad);
        }

        public override string GetNameDisplay()
        {
            return Name.Replace("{0}", Stick == Simulator.GamePadStick.Left ? "Left" : "Right")
                .Replace("{1}", DeltaX.ToString())
                .Replace("{2}", DeltaY.ToString());
        }

        public override bool Equals(object obj)
        {
            if (!(obj is GamePadStickAction action))
                return false;

            return action.Stick == Stick
                && action.DeltaX == DeltaX
                && action.DeltaY == DeltaY;
        }

        public override object Clone()
        {
            return new GamePadStickAction(Name, description)
            {
                Stick = Stick,
                DeltaX = DeltaX,
                DeltaY = DeltaY,
                ImageResource = ImageResource,
                Name = Name,
            };
        }
    }
}
