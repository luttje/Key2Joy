﻿using Key2Joy.Config;
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

        private static readonly TimeSpan IS_MOVING_TOLERANCE = TimeSpan.FromMilliseconds(10);
        private const double SENSITIVITY = 0.05;
        private const int WM_INPUT = 0x00FF;

        private Dictionary<int, List<MappedOption>> lookupAxis;
        
        private List<int> lastDirectionHashes;
        private DateTime lastMoveTime;

        private MouseMoveTriggerListener()
        {
            lookupAxis = new Dictionary<int, List<MappedOption>>();
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

        internal override bool GetIsTriggered(BaseTrigger trigger)
        {
            if (!(trigger is MouseMoveTrigger mouseMoveTrigger))
                return false;

            return (DateTime.Now - lastMoveTime < IS_MOVING_TOLERANCE) 
                && lastDirectionHashes.Contains(mouseMoveTrigger.GetInputHash());
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
        
        private bool TryOverrideMouseMoveInput(int lastX, int lastY)
        {
            var deltaX = (short)Math.Min(Math.Max(lastX * short.MaxValue * SENSITIVITY, short.MinValue), short.MaxValue);
            var deltaY = (short)-Math.Min(Math.Max(lastY * short.MaxValue * SENSITIVITY, short.MinValue), short.MaxValue);

            var mappedOptions = new List<MappedOption>();
            var directionHashes = new List<int>();
            var directionChecks = new Dictionary<int, Func<bool>>()
            {
                {
                    MouseMoveTrigger.GetInputHashFor(AxisDirection.Right),
                    () => deltaX > 0
                },
                {
                    MouseMoveTrigger.GetInputHashFor(AxisDirection.Left),
                    () => deltaX < 0
                },
                {
                    MouseMoveTrigger.GetInputHashFor(AxisDirection.Forward),
                    () => deltaY > 0
                },
                {
                    MouseMoveTrigger.GetInputHashFor(AxisDirection.Backward),
                    () => deltaY < 0
                },
            };

            foreach (var directionCheck in directionChecks)
            {
                if (directionCheck.Value.Invoke())
                {
                    if (lookupAxis.TryGetValue(directionCheck.Key, out var matchedOptions))
                    {
                        mappedOptions.AddRange(matchedOptions);
                        directionHashes.Add(directionCheck.Key);
                    }
                }
            }

            var inputBag = new MouseMoveInputBag
            {
                DeltaX = deltaX,
                DeltaY = deltaY,
            };

            DoExecuteTrigger(
                mappedOptions,
                inputBag,
                trigger => directionHashes.Contains((trigger as IReturnInputHash).GetInputHash())
            );

            lastDirectionHashes = directionHashes;
            lastMoveTime = DateTime.Now;

            return true;
        }
    }
}
