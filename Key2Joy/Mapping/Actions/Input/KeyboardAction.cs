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
        Description = "Keyboard Simulation",
        NameFormat = "{1} {0} on Keyboard"
    )]
    [ExposesScriptingEnumeration(typeof(KeyboardKey))]
    [Util.ObjectListViewGroup(
        Name = "Keyboard Simulation",
        Image = "keyboard"
    )]
    public class KeyboardAction : BaseAction, IPressState
    {        
        [JsonProperty]
        public KeyboardKey Key { get; set; }

        [JsonProperty]
        public PressState PressState { get; set; }

        public KeyboardAction(string name, string description)
            : base(name, description)
        {
        }

        public static KeyboardKey[] GetAllKeys()
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
        /// <name>Keyboard.Simulate</name>
        [ExposesScriptingMethod("Keyboard.Simulate")]
        public async void ExecuteForScript(KeyboardKey key, PressState pressState)
        {
            Key = key;
            PressState = pressState;

            if (PressState == PressState.Press)
                SimulatedKeyboard.PressKey(Key);
            
            if (PressState == PressState.Release)
                SimulatedKeyboard.ReleaseKey(Key);
        }

        public override async Task Execute(IInputBag inputBag = null)
        {
            if (PressState == PressState.Press)
                SimulatedKeyboard.PressKey(Key);
            else if(PressState == PressState.Release)
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
