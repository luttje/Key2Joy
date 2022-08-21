using Key2Joy.LowLevelInput;
using System;

namespace Key2Joy.Mapping
{
    internal class MouseButtonTriggerListener : PressReleaseTriggerListener<MouseButtonTrigger>
    {
        internal static MouseButtonTriggerListener instance;
        internal static MouseButtonTriggerListener Instance
        {
            get
            {
                if (instance == null)
                    instance = new MouseButtonTriggerListener();
                
                return instance;
            }
        }

        private GlobalInputHook globalMouseButtonHook;
        
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
                dictionary = lookupDown;
                
            // Test if this is a bound mouse button, if so halt default input behaviour
            if (!dictionary.TryGetValue(MouseButtonTrigger.GetInputHashFor(buttons), out var mappedOptions))
                return;

            foreach (var mappedOption in mappedOptions)
            {
                TryOverrideMouseButtonInput(mappedOption.Action, new MouseButtonInputBag
                {
                    Trigger = mappedOption.Trigger,
                    State = e.MouseState,
                    IsDown = isDown,
                    LastX = e.MouseData.Position.X,
                    LastY = e.MouseData.Position.Y,
                });
            }

            e.Handled = true;
        }
        
        private bool TryOverrideMouseButtonInput(BaseAction action, MouseButtonInputBag inputBag)
        {
            action.Execute(inputBag);

            return true; // Unused return parameter! We always override default behaviour with e.Handled = true;
        }
    }
}
