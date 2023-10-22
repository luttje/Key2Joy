using System;
using System.Collections.Generic;
using System.Diagnostics;
using Key2Joy.Contracts;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Triggers;
using Linearstar.Windows.RawInput;

namespace Key2Joy.Mapping.Triggers.Mouse;

public class MouseMoveTriggerListener : CoreTriggerListener, IWndProcHandler
{
    public IntPtr Handle { get; set; }

    public static MouseMoveTriggerListener instance;

    public static MouseMoveTriggerListener Instance
    {
        get
        {
            instance ??= new MouseMoveTriggerListener();

            return instance;
        }
    }

    private static readonly TimeSpan IS_MOVING_TOLERANCE = TimeSpan.FromMilliseconds(10);
    private const int WM_INPUT = 0x00FF;

    private readonly Dictionary<int, List<AbstractMappedOption>> lookupAxis;

    private List<int> lastDirectionHashes;
    private DateTime lastMoveTime;

    private MouseMoveTriggerListener()
        => this.lookupAxis = new Dictionary<int, List<AbstractMappedOption>>();

    /// <inheritdoc/>
    protected override void Start()
    {
        RawInputDevice.RegisterDevice(HidUsageAndPage.Mouse, RawInputDeviceFlags.InputSink, this.Handle);

        base.Start();
    }

    /// <inheritdoc/>
    protected override void Stop()
    {
        instance = null;

        base.Stop();
    }

    /// <inheritdoc/>
    public override void AddMappedOption(AbstractMappedOption mappedOption)
    {
        var trigger = mappedOption.Trigger as MouseMoveTrigger;

        if (!this.lookupAxis.TryGetValue(trigger.GetInputHash(), out var mappedOptions))
        {
            this.lookupAxis.Add(trigger.GetInputHash(), mappedOptions = new List<AbstractMappedOption>());
        }

        mappedOptions.Add(mappedOption);
    }

    /// <inheritdoc/>
    public override bool GetIsTriggered(AbstractTrigger trigger)
    {
        if (trigger is not MouseMoveTrigger mouseMoveTrigger)
        {
            return false;
        }

        return DateTime.Now - this.lastMoveTime < IS_MOVING_TOLERANCE
            && this.lastDirectionHashes.Contains(mouseMoveTrigger.GetInputHash());
    }

    /// <inheritdoc/>
    public void WndProc(Message m)
    {
        if (!this.IsActive)
        {
            return;
        }

        if (m.Msg != WM_INPUT)
        {
            return;
        }

        try
        {
            var data = RawInputData.FromHandle(m.LParam);

            if (data is not RawInputMouseData mouse)
            {
                return;
            }

            if (this.TryOverrideMouseMoveInput(mouse.Mouse.LastX, mouse.Mouse.LastY))
            {
                return;
            }
        }
        catch (Linearstar.Windows.RawInput.Native.Win32ErrorException ex)
        {
            Output.WriteLine(ex);
            // This exception seems to occur accross AppDomain boundary, when clicking on a MessageBox OK button
            Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
        }
    }

    private bool TryOverrideMouseMoveInput(int deltaX, int deltaY)
    {
        List<AbstractMappedOption> mappedOptions = new();
        List<int> directionHashes = new();
        Dictionary<int, Func<bool>> directionChecks = new()
        {
            {
                MouseMoveTrigger.GetInputHashFor(AxisDirection.Any),
                () => deltaX != 0 || deltaY != 0
            },
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
                if (this.lookupAxis.TryGetValue(directionCheck.Key, out var matchedOptions))
                {
                    mappedOptions.AddRange(matchedOptions);
                    directionHashes.Add(directionCheck.Key);
                }
            }
        }

        AxisDeltaInputBag inputBag = new()
        {
            DeltaX = deltaX,
            DeltaY = deltaY,
        };

        this.DoExecuteTrigger(
            mappedOptions,
            inputBag,
            trigger => directionHashes.Contains((trigger as IReturnInputHash).GetInputHash())
        );

        this.lastDirectionHashes = directionHashes;
        this.lastMoveTime = DateTime.Now;

        return true;
    }
}
