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
        Description = "GamePad Reset Simulation",
        Visibility = ActionVisibility.Never,
        NameFormat = "GamePad Reset"
    )]
    [ExposesScriptingEnumeration(typeof(Simulator.GamePadStick))]
    [Util.ObjectListViewGroup(
        Name = "GamePad Reset Simulation",
        Image = "joystick"
    )]
    internal class GamePadResetAction : BaseAction
    {
        public GamePadResetAction(string name, string description)
            : base(name, description)
        {
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
        /// <name>GamePad.Reset</name>
        [ExposesScriptingMethod("GamePad.Reset")]
        public async void ExecuteForScript()
        {
            var simPad = SimGamePad.Instance;
            var targetGamePad = 0; // TODO: Support multiple gamepads
            var state = simPad.State[targetGamePad];
            state.Reset();
            simPad.Update();
        }

        internal override async Task Execute(InputBag inputBag = null)
        {
            var simPad = SimGamePad.Instance;
            var targetGamePad = 0; // TODO: Support multiple gamepads
            var state = simPad.State[targetGamePad];
            state.Reset();
            simPad.Update();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is GamePadResetAction action))
                return false;

            return true;
        }

        public override object Clone()
        {
            return new GamePadResetAction(Name, description)
            {
                ImageResource = ImageResource,
                Name = Name,
            };
        }
    }
}
