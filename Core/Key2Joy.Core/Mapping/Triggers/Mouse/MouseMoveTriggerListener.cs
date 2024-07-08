using System;
using System.Collections.Generic;
using System.Diagnostics;
using CommonServiceLocator;
using Key2Joy.Config;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Triggers;
using Key2Joy.LowLevelInput;

namespace Key2Joy.Mapping.Triggers.Mouse;

public class MouseMoveTriggerListener : CoreTriggerListener, IOverrideDefaultBehavior
{
    public static MouseMoveTriggerListener instance;

    public static MouseMoveTriggerListener Instance
    {
        get
        {
            instance ??= new MouseMoveTriggerListener();

            return instance;
        }
    }

    /// <summary>
    /// Force a new instance to be created, used to reset the listener when mappings
    /// are re-armed.
    /// </summary>
    /// <returns></returns>
    public static MouseMoveTriggerListener NewInstance() => instance = new MouseMoveTriggerListener();

    private static readonly TimeSpan IS_MOVING_TOLERANCE = TimeSpan.FromMilliseconds(10);

    private readonly Dictionary<int, List<AbstractMappedOption>> lookupAxis;

    private GlobalInputHook globalMouseButtonHook;
    private List<int> lastDirectionHashes;
    private DateTime lastMoveTime;

    private int? lastAllowedX = null;
    private int? lastAllowedY = null;

    private MouseMoveTriggerListener()
        => this.lookupAxis = new Dictionary<int, List<AbstractMappedOption>>();

    /// <inheritdoc/>
    public bool ShouldListenerOverrideDefault(bool executedAny)
    {
        var configManager = ServiceLocator.Current.GetInstance<IConfigManager>();
        var config = configManager.GetConfigState();
        var listenerOverrideDefaultAll = config.ListenerOverrideDefaultMouseMoveAll;

        if (listenerOverrideDefaultAll)
        {
            return true;
        }

        var listenerOverrideDefault = config.ListenerOverrideDefaultMouseMove;
        return listenerOverrideDefault && executedAny;
    }

    /// <inheritdoc/>
    protected override void Start()
    {
        // This captures global mouse input and blocks default behaviour by setting e.Handled
        this.globalMouseButtonHook = new GlobalInputHook();
        this.globalMouseButtonHook.MouseInputEvent += this.OnMouseInputEvent;
        this.lastAllowedX = null;
        this.lastAllowedY = null;

        base.Start();
    }

    /// <inheritdoc/>
    protected override void Stop()
    {
        instance = null;
        this.globalMouseButtonHook.MouseInputEvent -= this.OnMouseInputEvent;
        this.globalMouseButtonHook.Dispose();
        this.globalMouseButtonHook = null;

        base.Stop();
    }

    /// <summary>
    /// Can be called to reset the cursor to the center. Useful for games that take control of the cursor
    /// and place it somewhere else.
    /// Used by <see cref="Actions.Logic.AppCommand.ResetMouseMoveTriggerCenter"/>
    /// </summary>
    public void ResetCenterCursor()
    {
        this.lastAllowedX = null;
        this.lastAllowedY = null;
        System.Diagnostics.Debug.WriteLine($"Mouse move reset center");
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

    private void OnMouseInputEvent(object sender, GlobalMouseHookEventArgs e)
    {
        if (!this.IsActive)
        {
            return;
        }

        /// Mouse buttons are handled through <see cref="MouseButtonTriggerListener"/>
        if (e.MouseState != MouseState.Move)
        {
            return;
        }

        var currentX = e.RawData.Position.X;
        var currentY = e.RawData.Position.Y;

        var deltaX = currentX - (this.lastAllowedX ?? 0);
        var deltaY = currentY - (this.lastAllowedY ?? 0);

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

        var shouldOverride = this.DoExecuteTrigger(
            mappedOptions,
            inputBag,
            trigger => directionHashes.Contains((trigger as IReturnInputHash).GetInputHash())
        );

        this.lastDirectionHashes = directionHashes;
        this.lastMoveTime = DateTime.Now;

        if (!shouldOverride || (!this.lastAllowedX.HasValue && !this.lastAllowedY.HasValue))
        {
            this.lastAllowedX = e.RawData.Position.X;
            this.lastAllowedY = e.RawData.Position.Y;
        }

        if (shouldOverride)
        {
            e.Handled = true;
        }
    }
}
