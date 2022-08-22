using Key2Joy.Config;
using Linearstar.Windows.RawInput;
using SimWinInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Key2Joy.Mapping
{
    internal class MouseMoveTriggerListener : TriggerListener
    {
        internal override bool HasWndProcHandle => true;

        internal static MouseMoveTriggerListener instance;
        internal static MouseMoveTriggerListener Instance
        {
            get
            {
                if (instance == null)
                    instance = new MouseMoveTriggerListener();
                
                return instance;
            }
        }

        private const double SENSITIVITY = 0.05;
        private const int WM_INPUT = 0x00FF;

        private Dictionary<int, List<MappedOption>> lookupAxis;
        private System.ComponentModel.Container components;

        private MouseMoveTriggerListener()
        {
            lookupAxis = new Dictionary<int, List<MappedOption>>();

            components = new System.ComponentModel.Container();
        }

        protected override void Start()
        {
            RawInputDevice.RegisterDevice(HidUsageAndPage.Mouse, RawInputDeviceFlags.InputSink, Handle);

            base.Start();
        }

        protected override void Stop()
        {
            instance = null;

            base.Stop();
        }

        internal override void AddMappedOption(MappedOption mappedOption)
        {
            var trigger = mappedOption.Trigger as MouseMoveTrigger;

            if (!lookupAxis.TryGetValue(trigger.GetInputHash(), out var mappedOptions))
                lookupAxis.Add(trigger.GetInputHash(), mappedOptions = new List<MappedOption>());

            mappedOptions.Add(mappedOption);
        }

        internal override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (!IsActive)
                return;
            
            if (m.Msg != WM_INPUT)
                return;

            var data = RawInputData.FromHandle(m.LParam);

            if (!(data is RawInputMouseData mouse))
                return;

            if (TryOverrideMouseMoveInput(mouse.Mouse.LastX, mouse.Mouse.LastY))
                return;
        }
        
        private void tmrAxisTimeout_Tick(object sender, EventArgs e)
        {
            var controllerId = 0;
            var state = SimGamePad.Instance.State[controllerId];
            state.RightStickX = 0;
            state.RightStickY = 0;
            SimGamePad.Instance.Update(controllerId);
        }

        private bool TryOverrideMouseMoveInput(int lastX, int lastY)
        {
            var deltaX = (short)Math.Min(Math.Max(lastX * short.MaxValue * SENSITIVITY, short.MinValue), short.MaxValue);
            var deltaY = (short)-Math.Min(Math.Max(lastY * short.MaxValue * SENSITIVITY, short.MinValue), short.MaxValue);

            var mappedOptions = new List<MappedOption>();
            List<MappedOption> matchedOptions;

            if (
                (
                    deltaX > 0
                    && lookupAxis.TryGetValue(
                        MouseMoveTrigger.GetInputHashFor(AxisDirection.Right), 
                        out matchedOptions)
                )
                ||
                (
                    deltaX < 0
                    && lookupAxis.TryGetValue(
                        MouseMoveTrigger.GetInputHashFor(AxisDirection.Left), 
                        out matchedOptions)
                )
            )
            {
                mappedOptions.AddRange(matchedOptions);
            }
            
            if (
                (
                    deltaY > 0
                    && lookupAxis.TryGetValue(
                        MouseMoveTrigger.GetInputHashFor(AxisDirection.Forward), 
                        out matchedOptions)
                )
                ||
                (
                    deltaY < 0
                    && lookupAxis.TryGetValue(
                        MouseMoveTrigger.GetInputHashFor(AxisDirection.Backward), 
                        out matchedOptions)
                )
            )
            {
                mappedOptions.AddRange(matchedOptions);
            }

            foreach (var mappedOption in mappedOptions)
            {
                mappedOption.Action.Execute(new MouseMoveInputBag
                {
                    Trigger = mappedOption.Trigger,
                    DeltaX = deltaX,
                    DeltaY = deltaY,
                });
            }

            return true;
        }
    }
}
