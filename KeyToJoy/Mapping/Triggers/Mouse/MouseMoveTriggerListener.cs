using Linearstar.Windows.RawInput;
using SimWinInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyToJoy.Mapping
{
    internal class MouseMoveTriggerListener : TriggerListener
    {
        internal override bool HasWndProcHandle => true;
        
        private const double SENSITIVITY = 0.05;
        private const int WM_INPUT = 0x00FF;

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
        
        private Dictionary<AxisDirection, BaseAction> lookup;
        private System.ComponentModel.Container components;
        private Timer tmrAxisTimeout;

        private MouseMoveTriggerListener()
        {
            lookup = new Dictionary<AxisDirection, BaseAction>();

            components = new System.ComponentModel.Container();
            tmrAxisTimeout = new Timer(components);
            tmrAxisTimeout.Interval = 250;
            tmrAxisTimeout.Tick += this.tmrAxisTimeout_Tick;
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
            
            lookup.Add(trigger.AxisBinding, mappedOption.Action);
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
            tmrAxisTimeout.Stop();
            tmrAxisTimeout.Start();

            var deltaX = (short)Math.Min(Math.Max(lastX * short.MaxValue * SENSITIVITY, short.MinValue), short.MaxValue);
            var deltaY = (short)-Math.Min(Math.Max(lastY * short.MaxValue * SENSITIVITY, short.MinValue), short.MaxValue);
            BaseAction action;

            if (
                (
                    deltaX > 0
                    && lookup.TryGetValue(AxisDirection.Right, out action)
                )
                ||
                (
                    deltaX < 0
                    && lookup.TryGetValue(AxisDirection.Left, out action)
                )
            )
            {
                if (action != null)
                    action.Execute(new MouseMoveInputBag
                    {
                        DeltaX = deltaX,
                    });
            }
            if (
                (
                    deltaY > 0
                    && lookup.TryGetValue(AxisDirection.Forward, out action)
                )
                ||
                (
                    deltaY < 0
                    && lookup.TryGetValue(AxisDirection.Backward, out action)
                )
            )
            {
                if (action != null)
                    action.Execute(new MouseMoveInputBag
                    {
                        DeltaY = deltaY,
                    });
            }

            return true;
        }
    }
}
