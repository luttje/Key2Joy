using Key2Joy.Contracts.Mapping;
using Key2Joy.LowLevelInput;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Key2Joy.Mapping
{
    [Action(
        Description = "Mouse Button Simulation",
        Visibility = MappingMenuVisibility.Never,
        NameFormat = "{1} {0} on Mouse",
        GroupName = "Mouse Button Simulation",
        GroupImage = "mouse"
    )]
    public class MouseButtonAction : CoreAction, IPressState
    {
        public Mouse.Buttons Button { get; set; }
        public PressState PressState { get; set; }

        public MouseButtonAction(string name)
            : base(name)
        {
        }

        public static Mouse.Buttons[] GetAllButtons()
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

        public override async Task Execute(AbstractInputBag inputBag = null)
        {
            if (PressState == PressState.Press)
                SimulatedMouse.PressButton(Button);
            else if (PressState == PressState.Release)
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
    }
}
