using Key2Joy.Contracts.Mapping;
using Key2Joy.LowLevelInput;
using System.Collections.Generic;

namespace Key2Joy.Mapping
{
    public class MouseButtonTriggerListener : PressReleaseTriggerListener<MouseButtonTrigger>
    {
        public static MouseButtonTriggerListener instance;
        public static MouseButtonTriggerListener Instance
        {
            get
            {
                instance ??= new MouseButtonTriggerListener();

                return instance;
            }
        }

        private GlobalInputHook globalMouseButtonHook;
        private readonly Dictionary<Mouse.Buttons, bool> currentButtonsDown = new();

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

        public override bool GetIsTriggered(AbstractTrigger trigger)
        {
            if (trigger is not MouseButtonTrigger mouseButtonTrigger)
            {
                return false;
            }

            return currentButtonsDown.ContainsKey(mouseButtonTrigger.MouseButtons);
        }

        private void OnMouseButtonInputEvent(object sender, GlobalMouseHookEventArgs e)
        {
            if (!IsActive)
            {
                return;
            }

            // Mouse movement is handled through WndProc and TryOverrideMouseMoveInput in MouseMoveTriggerListener
            if (e.MouseState == MouseState.Move)
            {
                return;
            }

            var buttons = Mouse.ButtonsFromEvent(e, out var isDown);
            var dictionary = lookupRelease;

            if (isDown)
            {
                dictionary = lookupDown;

                if (currentButtonsDown.ContainsKey(buttons))
                {
                    return; // Prevent firing multiple times for a single key press
                }
                else
                {
                    currentButtonsDown.Add(buttons, true);
                }
            }
            else
            {
                if (currentButtonsDown.ContainsKey(buttons))
                {
                    currentButtonsDown.Remove(buttons);
                }
            }

            MouseButtonInputBag inputBag = new()
            {
                State = e.MouseState,
                IsDown = isDown,
                LastX = e.RawData.Position.X,
                LastY = e.RawData.Position.Y,
            };

            var hash = MouseButtonTrigger.GetInputHashFor(buttons);
            dictionary.TryGetValue(hash, out var mappedOptions);

            if (DoExecuteTrigger(
                mappedOptions,
                inputBag,
                trigger =>
                {
                    var mouseTrigger = trigger as MouseButtonTrigger;
                    return mouseTrigger.GetInputHash() == hash
                        && mouseTrigger.MouseButtons == buttons;
                }))
            {
                e.Handled = true;
            }
        }
    }
}
