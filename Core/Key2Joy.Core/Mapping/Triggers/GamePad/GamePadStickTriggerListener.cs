using System.Collections.Generic;
using CommonServiceLocator;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Triggers;
using Key2Joy.LowLevelInput.XInput;

namespace Key2Joy.Mapping.Triggers.GamePad;

public class GamePadStickLookup : Dictionary<int, List<AbstractMappedOption>>
{ }

public enum GamePadStickSide
{
    Left = 0,
    Right = 1,
}

public class GamePadStickTriggerListener : CoreTriggerListener
{
    public static GamePadStickTriggerListener instance;

    public static GamePadStickTriggerListener Instance
    {
        get
        {
            instance ??= new GamePadStickTriggerListener();

            return instance;
        }
    }

    private readonly Dictionary<GamePadStickSide, GamePadStickLookup> stickAxisLookups;
    private readonly IXInputService xInputService;

    private GamePadStickTriggerListener()
    {
        this.xInputService = ServiceLocator.Current.GetInstance<IXInputService>();

        this.stickAxisLookups = new Dictionary<GamePadStickSide, GamePadStickLookup>();
        this.stickAxisLookups[GamePadStickSide.Left] = new GamePadStickLookup();
        this.stickAxisLookups[GamePadStickSide.Right] = new GamePadStickLookup();
    }

    protected override void Start()
    {
        //var allMappedOptions = this.stickAxisLookups.Values
        //    .SelectMany(x => x.Values.SelectMany(x => x));

        //foreach (var mappedOptions in allMappedOptions)
        //{
        //  // Register all devices that are mapped to
        //}

        // Let's try what happens if we just listen to all available devices
        for (var i = 0; i < XInputService.MaxDevices; i++)
        {
            this.xInputService.RegisterDevice(i);
        }

        this.xInputService.StateChanged += this.XInputService_StateChanged;
        this.xInputService.StartPolling();

        base.Start();
    }

    protected override void Stop()
    {
        this.xInputService.StopPolling();
        this.xInputService.StateChanged -= this.XInputService_StateChanged;
        instance = null;

        base.Stop();
    }

    public override void AddMappedOption(AbstractMappedOption mappedOption)
    {
        var trigger = mappedOption.Trigger as GamePadStickTrigger;
        var lookup = this.stickAxisLookups[trigger.StickSide];

        if (!lookup.TryGetValue(trigger.GetInputHash(), out var mappedOptions))
        {
            lookup.Add(trigger.GetInputHash(), mappedOptions = new List<AbstractMappedOption>());
        }

        mappedOptions.Add(mappedOption);
    }

    public override bool GetIsTriggered(AbstractTrigger trigger)
    {
        if (trigger is not GamePadStickTrigger gamePadStickTrigger)
        {
            return false;
        }

        var state = this.xInputService.GetState(gamePadStickTrigger.GamePadIndex);

        if (gamePadStickTrigger.StickSide == GamePadStickSide.Left)
        {
            return state.Gamepad.IsLeftThumbMoved(gamePadStickTrigger.DeltaMargin);
        }
        else
        {
            return state.Gamepad.IsRightThumbMoved(gamePadStickTrigger.DeltaMargin);
        }
    }

    /// <summary>
    /// This method is called when the state of a gamepad has changed.
    /// We'll use it to find in the lookup which mapped options are triggered based on
    /// the triggered device index and margins (using <see cref="GetIsTriggered"/>).
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void XInputService_StateChanged(object sender, DeviceStateChangedEventArgs e)
    {
        var state = e.NewState;

        List<AbstractMappedOption> mappedOptions = new();

        foreach (var stickAxisLookupKvp in this.stickAxisLookups)
        {
            var sideToLookup = stickAxisLookupKvp.Key;
            var lookup = stickAxisLookupKvp.Value;

            foreach (var lookupKvp in lookup)
            {
                var triggerHash = lookupKvp.Key;
                var lookupMappedOptions = lookupKvp.Value;

                foreach (var mappedOption in lookupMappedOptions)
                {
                    if (this.GetIsTriggered(mappedOption.Trigger))
                    {
                        mappedOptions.AddRange(lookupMappedOptions);
                    }
                }
            }
        }

        GamePadStickInputBag inputBag = new()
        {
            LeftStickDelta = state.Gamepad.GetStickDelta(GamePadStickSide.Left),
            RightStickDelta = state.Gamepad.GetStickDelta(GamePadStickSide.Right),
        };

        this.DoExecuteTrigger(
            mappedOptions,
            inputBag,
            this.GetIsTriggered
        );
    }
}
