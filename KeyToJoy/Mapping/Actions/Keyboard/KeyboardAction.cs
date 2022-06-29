using KeyToJoy.LowLevelInput;
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

namespace KeyToJoy.Mapping
{
    [Action(
        Description = "Keyboard Simulation",
        OptionsUserControl = typeof(KeyboardActionControl),
        NameFormat = "{1} {0} on Keyboard"
    )]
    [ExposesScriptingEnumeration(typeof(KeyboardKey))]
    [Util.ObjectListViewGroup(
        Name = "Keyboard Simulation",
        Image = "keyboard"
    )]
    internal class KeyboardAction : BaseAction
    {        
        [JsonProperty]
        public KeyboardKey Key { get; set; }

        [JsonProperty]
        public PressState PressState { get; set; }

        public KeyboardAction(string name, string description)
            : base(name, description)
        {
        }

        internal static KeyboardKey[] GetAllKeys()
        {
            var allEnums = Enum.GetValues(typeof(KeyboardKey));
            var keys = new List<KeyboardKey>();

            // Skip the enumerations that are zero
            foreach (var keyEnum in allEnums)
            {
                if ((short)keyEnum == 0)
                    continue;

                keys.Add((KeyboardKey)keyEnum);
            }

            return keys.ToArray();
        }

        public static List<MappedOption> GetAllButtonActions(PressState pressState)
        {
            var actionType = typeof(KeyboardAction);
            var typeAttribute = ((ActionAttribute[])actionType.GetCustomAttributes(typeof(ActionAttribute), false))[0];

            var actions = new List<MappedOption>();
            foreach (var key in GetAllKeys())
            {
                var action = (KeyboardAction)MakeAction(actionType, typeAttribute);
                action.Key = key;
                action.PressState = pressState;

                actions.Add(new MappedOption
                {
                    Action = action
                });
            }
            return actions;
        }

        /// <markdown-doc>
        /// <parent-name>Input</parent-name>
        /// <path>Api/Input</path>
        /// </markdown-doc>
        /// <summary>
        /// Simulate pressing or releasing (or both) keyboard keys.
        /// </summary>
        /// <param name="key">Key to simulate</param>
        /// <param name="pressState">Action to simulate</param>
        /// <markdown-example>
        /// TODO: Explain how to use this function in js
        /// <code language="js">
        /// <![CDATA[//TODO: JS example]]>
        /// </code>
        /// </markdown-example>
        /// <markdown-example>
        /// TODO: Explain how to use this function in Lua
        /// <code language="lua">
        /// <![CDATA[--TODO: Lua example]]>
        /// </code>
        /// </markdown-example>
        /// <name>Keyboard.Simulate</name>
        [ExposesScriptingMethod("Keyboard.Simulate")]
        public void ExecuteForScript(KeyboardKey key, PressState pressState)
        {
            Key = key;
            PressState = pressState;

            if (PressState == PressState.Press || PressState == PressState.PressAndRelease)
            {
                SimulatedKeyboard.PressKey(Key);

                if (PressState == PressState.PressAndRelease)
                {
                    Task.Run(async () =>
                    {
                        // TODO: Make this a configurable value
                        await Task.Delay(50);
                        SimulatedKeyboard.ReleaseKey(Key);
                    });
                }
            }
            
            if (PressState == PressState.Release)
                SimulatedKeyboard.ReleaseKey(Key);
        }

        internal override async Task Execute(InputBag inputBag = null)
        {
            bool isInputDown = false;
            
            if (inputBag is KeyboardInputBag keyboardInputBag)
                isInputDown = keyboardInputBag.State == KeyboardState.KeyDown;
            
            if (inputBag is MouseButtonInputBag mouseButtonInputBag)
                isInputDown = mouseButtonInputBag.IsDown;

            if (PressState == PressState.Press
                || (PressState == PressState.PressAndRelease && isInputDown))
                SimulatedKeyboard.PressKey(Key);
            else if(PressState == PressState.Release
                || (PressState == PressState.PressAndRelease && !isInputDown))
                SimulatedKeyboard.ReleaseKey(Key);
        }

        public override string GetNameDisplay()
        {
            return Name.Replace("{0}", Enum.GetName(typeof(KeyboardKey), Key))
                .Replace("{1}", Enum.GetName(typeof(PressState), PressState));
        }

        public override bool Equals(object obj)
        {
            if (!(obj is KeyboardAction action))
                return false;

            return action.Key == Key
                && action.PressState == PressState;
        }

        public override object Clone()
        {
            return new KeyboardAction(Name, description)
            {
                Key = Key,
                PressState = PressState,
                ImageResource = ImageResource,
                Name = Name,
            };
        }
    }
}
