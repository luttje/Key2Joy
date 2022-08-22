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
        Description = "Mouse Button Simulation",
        Visibility = ActionVisibility.Never,
        NameFormat = "{1} {0} on Mouse"
    )]
    [ExposesScriptingEnumeration(typeof(Mouse.Buttons))]
    [Util.ObjectListViewGroup(
        Name = "Mouse Button Simulation",
        Image = "mouse"
    )]
    internal class MouseButtonAction : BaseAction, IPressState
    {        
        [JsonProperty]
        public Mouse.Buttons Button { get; set; }

        [JsonProperty]
        public PressState PressState { get; set; }

        public MouseButtonAction(string name, string description)
            : base(name, description)
        {
        }

        internal static Mouse.Buttons[] GetAllButtons()
        {
            var allEnums = Enum.GetValues(typeof(Mouse.Buttons));
            var buttons = new List<Mouse.Buttons>();

            // Skip the enumerations that are zero
            foreach (var buttonEnum in allEnums)
            {
                if ((short)buttonEnum == 0)
                    continue;

                buttons.Add((Mouse.Buttons)buttonEnum);
            }

            return buttons.ToArray();
        }

        /// <markdown-doc>
        /// <parent-name>Input</parent-name>
        /// <path>Api/Input</path>
        /// </markdown-doc>
        /// <summary>
        /// Simulate pressing or releasing (or both) mouse buttons.
        /// </summary>
        /// <param name="button">Button to simulate</param>
        /// <param name="pressState">Action to simulate</param>
        /// <name>Mouse.Simulate</name>
        [ExposesScriptingMethod("Mouse.Simulate")]
        public async void ExecuteForScript(Mouse.Buttons button, PressState pressState)
        {
            Button = button;
            PressState = pressState;

            await Execute();
        }

        internal override async Task Execute(InputBag inputBag = null)
        {
            if (PressState == PressState.Press)
                SimulatedMouse.PressButton(Button);
            else if(PressState == PressState.Release)
                SimulatedMouse.ReleaseButton(Button);
        }

        public override string GetNameDisplay()
        {
            return Name.Replace("{0}", Enum.GetName(typeof(Mouse.Buttons), Button))
                .Replace("{1}", Enum.GetName(typeof(Mouse.Buttons), PressState));
        }

        public override bool Equals(object obj)
        {
            if (!(obj is MouseButtonAction action))
                return false;

            return action.Button == Button
                && action.PressState == PressState;
        }

        public override object Clone()
        {
            return new MouseButtonAction(Name, description)
            {
                Button = Button,
                PressState = PressState,
                ImageResource = ImageResource,
                Name = Name,
            };
        }
    }
}
