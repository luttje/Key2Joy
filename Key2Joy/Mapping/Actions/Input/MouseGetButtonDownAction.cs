using Key2Joy.LowLevelInput;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Mapping
{
    [Action(
        Description = "Get Mouse Button Down",
        Visibility = MappingMenuVisibility.Never,
        NameFormat = "Get Mouse Button Down"
    )]
    internal class MouseGetButtonDownAction : BaseAction
    {
        public MouseGetButtonDownAction(string name, string description)
            : base(name, description)
        { }

        /// <markdown-doc>
        /// <parent-name>Input</parent-name>
        /// <path>Api/Input</path>
        /// </markdown-doc>
        /// <summary>
        /// Tests if the provided mouse button is currently pressed.
        /// 
        /// You can find the button codes in <see href="../Enumerations/Buttons.md"/>.
        /// </summary>
        /// <markdown-example>
        /// Shows how to show all mouse buttons currently pressed.
        /// <code language="lua">
        /// <![CDATA[
        /// for buttonName, button in pairs(Buttons)do
        ///    if(Buttons.GetButtonDown(button))then
        ///       Print(buttonName, "is currently pressed")
        ///    end
        /// end
        /// ]]>
        /// </code>
        /// </markdown-example>
        /// <param name="button">The button to test for</param>
        /// <returns>True if the button is currently pressed down, false otherwise</returns>
        /// <name>Mouse.GetButtonDown</name>
        [ExposesScriptingMethod("Mouse.GetButtonDown")]
        public bool ExecuteForScript(Mouse.Buttons button)
        {
            return MouseButtonTriggerListener.Instance.GetButtonsDown(button);
        }

        internal override async Task Execute(IInputBag inputBag = null)
        {
            // TODO: Currently this is only a script action...
        }

        public override string GetNameDisplay()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is MouseGetButtonDownAction action))
                return false;

            // TODO: Currently this is only a script action so this is irrelevant
            return false;
        }

        public override object Clone()
        {
            return new MouseGetButtonDownAction(Name, description)
            {
                ImageResource = ImageResource,
                Name = Name,
            };
        }
    }
}
