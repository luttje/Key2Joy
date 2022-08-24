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
