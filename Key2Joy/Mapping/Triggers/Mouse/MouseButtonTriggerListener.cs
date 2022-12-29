using Key2Joy.LowLevelInput;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Key2Joy.Mapping
{
    public class MouseButtonTriggerListener : PressReleaseTriggerListener<MouseButtonTrigger>
    {
        public static MouseButtonTriggerListener instance;
        public static MouseButtonTriggerListener Instance
        {
            get
            {
                if (instance == null)
                    instance = new MouseButtonTriggerListener();
                
                return instance;
            }
        }

        private GlobalInputHook globalMouseButtonHook;
        private readonly Dictionary<Mouse.Buttons, bool> currentButtonsDown = new Dictionary<Mouse.Buttons, bool>();

        public bool GetButtonsDown(Mouse.Buttons buttons)
        {
            return currentButtonsDown.ContainsKey(buttons);
        }

        protected override void Start()
        {
            // This captures global mouse input and blocks default behaviour by setting e.Handled
            globalMouseButtonHook = new GlobalInputHook();
            globalMouseButtonHook.MouseInputEvent += OnMouseButtonInputEvent;

            base.Start();
        }

        protected override void Stop()
        {
            instance = null;
            globalMouseButtonHook.MouseInputEvent -= OnMouseButtonInputEvent;
            globalMouseButtonHook.Dispose();
            globalMouseButtonHook = null;

            base.Stop();
        }
        
        public override bool GetIsTriggered(BaseTrigger trigger)
        {
            if (!(trigger is MouseButtonTrigger mouseButtonTrigger))
                return false;

            return currentButtonsDown.ContainsKey(mouseButtonTrigger.MouseButtons);
        }

        private void OnMouseButtonInputEvent(object sender, GlobalMouseHookEventArgs e)
        {
            if (!IsActive)
                return;

            // Mouse movement is handled through WndProc and TryOverrideMouseMoveInput in MouseMoveTriggerListener
            if (e.MouseState == MouseState.Move)
                return;

            var buttons = Mouse.Buttons.None;
            var isDown = false;
                
            try
            {
                buttons = Mouse.ButtonsFromState(e.MouseState, out isDown);
            }
            catch (NotImplementedException) { }

            var dictionary = lookupRelease;

            if (isDown)
            {
                dictionary = lookupDown;

                if (currentButtonsDown.ContainsKey(buttons))
                    return; // Prevent firing multiple times for a single key press
                else
                    currentButtonsDown.Add(buttons, true);
            }
            else
            {
                if (currentButtonsDown.ContainsKey(buttons))
                    currentButtonsDown.Remove(buttons);
            }

            var inputBag = new MouseButtonInputBag
            {
                State = e.MouseState,
                IsDown = isDown,
                LastX = e.MouseData.Position.X,
                LastY = e.MouseData.Position.Y,
            };

            var hash = MouseButtonTrigger.GetInputHashFor(buttons);
            dictionary.TryGetValue(hash, out var mappedOptions);
            
            if(DoExecuteTrigger(
                mappedOptions,
                inputBag,
                trigger =>
                {
                    var mouseTrigger = trigger as MouseButtonTrigger;
                    return mouseTrigger.GetInputHash() == hash
                        && mouseTrigger.MouseButtons == buttons;
                }))
                e.Handled = true;
        }
    }
}
