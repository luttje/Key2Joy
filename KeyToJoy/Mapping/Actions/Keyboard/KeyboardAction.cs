using KeyToJoy.Input;
using KeyToJoy.Input.LowLevel;
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
    [ExposesScriptingEnumeration(typeof(Keys))]
    internal class KeyboardAction : BaseAction
    {
        internal const string SCRIPT_COMMAND = "keyboard";
        
        [JsonProperty]
        public byte Key { get; set; }

        [JsonProperty]
        public PressState PressState { get; set; }

        public KeyboardAction(string name, string description)
            : base(name, description)
        {
        }

        [ExposesScriptingMethod(SCRIPT_COMMAND)]
        public void ExecuteActionForScript(Keys keys, PressState pressState)
        {
            Key = (byte)keys;
            PressState = pressState;

            if (PressState == PressState.Press || PressState == PressState.PressAndRelease)
            {
                SimKeyboard.KeyDown(Key);

                if (PressState == PressState.PressAndRelease)
                {
                    Task.Run(async () =>
                    {
                        // TODO: Make this a configurable value
                        await Task.Delay(50);
                        SimKeyboard.KeyUp(Key);
                    });
                }
            }
            
            if (PressState == PressState.Release)
                SimKeyboard.KeyUp(Key);
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
                SimKeyboard.KeyDown(Key);
            else if(PressState == PressState.Release
                || (PressState == PressState.PressAndRelease && !isInputDown))
                SimKeyboard.KeyUp(Key);
        }

        public override string GetNameDisplay()
        {
            return Name.Replace("{0}", Enum.GetName(typeof(Keys), Key))
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
