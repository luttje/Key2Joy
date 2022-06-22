using KeyToJoy.Input;
using KeyToJoy.Input.LowLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyToJoy.Mapping
{
    internal class KeyboardTriggerListener : TriggerListener
    {
        internal static KeyboardTriggerListener instance;
        internal static KeyboardTriggerListener Instance
        {
            get
            {
                if (instance == null)
                    instance = new KeyboardTriggerListener();
                
                return instance;
            }
        }

        private GlobalInputHook globalKeyboardHook;
        private Dictionary<Keys, BaseAction> lookupDown;
        private Dictionary<Keys, BaseAction> lookupRelease;

        private KeyboardTriggerListener()
        {
            lookupDown = new Dictionary<Keys, BaseAction>();
            lookupRelease = new Dictionary<Keys, BaseAction>();
        }

        protected override void Start()
        {
            // This captures global keyboard input and blocks default behaviour by setting e.Handled
            globalKeyboardHook = new GlobalInputHook();
            globalKeyboardHook.KeyboardInputEvent += OnKeyInputEvent;

            base.Start();
        }

        protected override void Stop()
        {
            instance = null;
            globalKeyboardHook.KeyboardInputEvent -= OnKeyInputEvent;
            globalKeyboardHook.Dispose();

            base.Stop();
        }

        internal override void AddMappedOption(MappedOption mappedOption)
        {
            var trigger = mappedOption.Trigger as KeyboardTrigger;
            
            if(trigger.PressedDown)
                lookupDown.Add(trigger.Keys, mappedOption.Action);
            else
                lookupRelease.Add(trigger.Keys, mappedOption.Action);
        }

        private void OnKeyInputEvent(object sender, GlobalKeyboardHookEventArgs e)
        {
            // Test if this is a bound key, if so halt default input behaviour
            var keys = VirtualKeyConverter.KeysFromVirtual(e.KeyboardData.VirtualCode);
            Dictionary<Keys, BaseAction> dictionary;

            if (e.KeyboardState == KeyboardState.KeyDown)
                dictionary = lookupDown;
            else
                dictionary = lookupRelease;

            if (!dictionary.TryGetValue(keys, out var action))
                return;

            if (!TryOverrideKeyboardInput(action, new KeyboardInputBag
            {
                State = e.KeyboardState,
                Keys = keys
            }))
                return;

            e.Handled = true;
        }

        private bool TryOverrideKeyboardInput(BaseAction action, KeyboardInputBag inputBag)
        {
            action.Execute(inputBag);

            return true;
        }
        private bool TryOverrideMouseButtonInput(BaseAction action, MouseButtonInputBag inputBag)
        {
            action.Execute(inputBag);

            return true;
        }
    }
}
