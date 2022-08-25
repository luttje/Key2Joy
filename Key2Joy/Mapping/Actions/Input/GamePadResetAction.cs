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
        Visibility = MappingMenuVisibility.Never,
        NameFormat = "Reset GamePad #{0}"
    )]
    [ExposesScriptingEnumeration(typeof(Simulator.GamePadStick))]
    [Util.ObjectListViewGroup(
        Name = "GamePad Reset Simulation",
        Image = "joystick"
    )]
    internal class GamePadResetAction : BaseAction
    {
        [JsonProperty]
        public int GamePadIndex { get; set; }

        public GamePadResetAction(string name, string description)
            : base(name, description)
        {
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
            GamePadIndex = gamepadIndex;

            GamePadManager.Instance.EnsurePluggedIn(GamePadIndex);

            var simPad = SimGamePad.Instance;
            var state = simPad.State[GamePadIndex];
            state.Reset();
            simPad.Update(GamePadIndex);
        }

        internal override async Task Execute(IInputBag inputBag = null)
        {
            var simPad = SimGamePad.Instance;
            var state = simPad.State[GamePadIndex];
            state.Reset();
            simPad.Update(GamePadIndex);
        }

        public override string GetNameDisplay()
        {
            return Name.Replace("{0}", GamePadIndex.ToString());
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
                GamePadIndex = GamePadIndex,
                ImageResource = ImageResource,
                Name = Name,
            };
        }
    }
}
