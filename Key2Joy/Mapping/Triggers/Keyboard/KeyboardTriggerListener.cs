using Key2Joy.LowLevelInput;
using System;
using System.Collections.Generic;

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
            globalKeyboardHook = null;

            base.Stop();
        }

        private void OnKeyInputEvent(object sender, GlobalKeyboardHookEventArgs e)
        {
            if (!IsActive)
                return;

            // Test if this is a bound key, if so halt default input behaviour
            var keys = VirtualKeyConverter.KeysFromVirtual(e.KeyboardData.VirtualCode);
            Dictionary<int, List<MappedOption>> dictionary;
            
            if (e.KeyboardState == KeyboardState.KeyDown)
                dictionary = lookupDown;
            else
                dictionary = lookupRelease;

            if (!dictionary.TryGetValue(KeyboardTrigger.GetInputHashFor(keys), out var mappedOptions))
                return;

            foreach (var mappedOption in mappedOptions)
            {
                TryOverrideKeyboardInput(mappedOption.Action, new KeyboardInputBag
                {
                    Trigger = mappedOption.Trigger,
                    State = e.KeyboardState,
                    Keys = keys
                });
            }

            e.Handled = true;
        }

        private bool TryOverrideKeyboardInput(BaseAction action, KeyboardInputBag inputBag)
        {
            action.Execute(inputBag);

            return true; // Unused return parameter! We always override default behaviour with e.Handled = true;
        }
    }
}
