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
    internal class MouseButtonTriggerListener : TriggerListener
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
        private Dictionary<Mouse.Buttons, BaseAction> lookup;
        
        private MouseButtonTriggerListener()
        {
            lookup = new Dictionary<Mouse.Buttons, BaseAction>();
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

            base.Stop();
        }

        internal override void AddMappedOption(MappedOption mappedOption)
        {
            var trigger = mappedOption.Trigger as MouseButtonTrigger;
            
            lookup.Add(trigger.MouseButtons, mappedOption.Action);
        }

        private void OnMouseButtonInputEvent(object sender, GlobalMouseHookEventArgs e)
        {
            // Mouse movement is handled through WndProc and TryOverrideMouseMoveInput
            if (e.MouseState == MouseState.Move)
                return;

            try
            {
                var buttons = Mouse.Buttons.None;
                var isDown = false;
                
                try
                {
                    buttons = Mouse.ButtonsFromState(e.MouseState, out isDown);
                }
                catch (NotImplementedException) { }
                
                // Test if this is a bound mouse button, if so halt default input behaviour
                if (!lookup.TryGetValue(buttons, out var action))
                    return;
                
                if (!TryOverrideMouseButtonInput(action, new MouseButtonInputBag
                {
                    State = e.MouseState,
                    IsDown = isDown,
                    LastX = e.MouseData.Position.X,
                    LastY = e.MouseData.Position.Y,
                }))
                    return;

                e.Handled = true;
            }
            catch (ArgumentOutOfRangeException) { }
        }
        
        private bool TryOverrideMouseButtonInput(BaseAction action, MouseButtonInputBag inputBag)
        {
            action.Execute(inputBag);

            return true;
        }
    }
}
