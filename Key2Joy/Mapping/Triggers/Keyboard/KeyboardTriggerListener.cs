using Key2Joy.LowLevelInput;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static Key2Joy.LowLevelInput.Mouse;

namespace Key2Joy.Mapping
{
    internal class KeyboardTriggerListener : PressReleaseTriggerListener<KeyboardTrigger>
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
        private readonly Dictionary<Keys, bool> currentKeysDown = new Dictionary<Keys, bool>();
        
        internal bool GetKeyDown(Keys key)
        {
            return currentKeysDown.ContainsKey(key);
        }

        protected override void Start()
        {
            // This captures global keyboard input and blocks default behaviour by setting e.Handled
            globalKeyboardHook = new GlobalInputHook();
            globalKeyboardHook.KeyboardInputEvent += OnKeyInputEvent;
            currentKeysDown.Clear();

            base.Start();
        }

        protected override void Stop()
        {
            instance = null;
            globalKeyboardHook.KeyboardInputEvent -= OnKeyInputEvent;
            globalKeyboardHook.Dispose();
            globalKeyboardHook = null;
            currentKeysDown.Clear();

            base.Stop();
        }

        internal override bool GetIsTriggered(BaseTrigger trigger)
        {
            if (!(trigger is KeyboardTrigger keyboardTrigger))
                return false;

            return currentKeysDown.ContainsKey(keyboardTrigger.Keys);
        }

        private void OnKeyInputEvent(object sender, GlobalKeyboardHookEventArgs e)
        {
            if (!IsActive)
                return;

            // Test if this is a bound key, if so halt default input behaviour
            var keys = VirtualKeyConverter.KeysFromVirtual(e.KeyboardData.VirtualCode);
            Dictionary<int, List<MappedOption>> dictionary;
            
            if (e.KeyboardState == KeyboardState.KeyDown) 
            { 
                dictionary = lookupDown;

                if (currentKeysDown.ContainsKey(keys))
                    return; // Prevent firing multiple times for a single key press
                else
                    currentKeysDown.Add(keys, true);
            }
            else
            {
                dictionary = lookupRelease;
                
                if (currentKeysDown.ContainsKey(keys))
                    currentKeysDown.Remove(keys);
            }

            var inputBag = new KeyboardInputBag
            {
                State = e.KeyboardState,
                Keys = keys
            };

            var hash = KeyboardTrigger.GetInputHashFor(keys);
            dictionary.TryGetValue(hash, out var mappedOptions);

            if(DoExecuteTrigger(
                mappedOptions,
                inputBag,
                trigger =>
                {
                    var keyboardTrigger = trigger as KeyboardTrigger;
                    return keyboardTrigger.GetInputHash() == hash
                        && keyboardTrigger.GetKeyboardState() == e.KeyboardState;
                }))
                e.Handled = true;
        }
    }
}
