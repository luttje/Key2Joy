using Key2Joy.LowLevelInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Key2Joy.Mapping
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
        private Dictionary<Keys, MappedOption> lookupDown;
        private Dictionary<Keys, MappedOption> lookupRelease;

        private KeyboardTriggerListener()
        {
            lookupDown = new Dictionary<Keys, MappedOption>();
            lookupRelease = new Dictionary<Keys, MappedOption>();
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
            globalKeyboardHook = null;

            base.Stop();
        }

        internal override void AddMappedOption(MappedOption mappedOption)
        {
            var trigger = mappedOption.Trigger as KeyboardTrigger;
            
            if (trigger.PressState == PressState.Press)
                lookupDown.Add(trigger.Keys, mappedOption);
            if (trigger.PressState == PressState.Release)
                lookupRelease.Add(trigger.Keys, mappedOption);
        }

        private void OnKeyInputEvent(object sender, GlobalKeyboardHookEventArgs e)
        {
            if (!IsActive)
                return;

            // Test if this is a bound key, if so halt default input behaviour
            var keys = VirtualKeyConverter.KeysFromVirtual(e.KeyboardData.VirtualCode);
            Dictionary<Keys, MappedOption> dictionary;

            if (e.KeyboardState == KeyboardState.KeyDown)
                dictionary = lookupDown;
            else
                dictionary = lookupRelease;

            if (!dictionary.TryGetValue(keys, out var mappedOption))
                return;

            if (!TryOverrideKeyboardInput(mappedOption.Action, new KeyboardInputBag
            {
                Trigger = mappedOption.Trigger,
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
    }
}
