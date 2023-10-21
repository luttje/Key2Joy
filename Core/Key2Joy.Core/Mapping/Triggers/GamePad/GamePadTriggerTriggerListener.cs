using System.Collections.Generic;
using CommonServiceLocator;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Triggers;
using Key2Joy.LowLevelInput.XInput;

namespace Key2Joy.Mapping.Triggers.GamePad;

public class GamePadTriggerTriggerListener : CoreTriggerListener
{
    public static GamePadTriggerTriggerListener instance;

    public static GamePadTriggerTriggerListener Instance
    {
        get
        {
            instance ??= new GamePadTriggerTriggerListener();

            return instance;
        }
    }

    private readonly Dictionary<GamePadSide, List<AbstractMappedOption>> triggerLookup;
    private readonly IXInputService xInputService;

    private GamePadTriggerTriggerListener()
    {
        this.xInputService = ServiceLocator.Current.GetInstance<IXInputService>();

        this.triggerLookup = new()
        {
            [GamePadSide.Left] = new List<AbstractMappedOption>(),
            [GamePadSide.Right] = new List<AbstractMappedOption>()
        };
    }

    /// <inheritdoc/>
    protected override void Start()
    {
        this.xInputService.StateChanged += this.XInputService_StateChanged;
        this.xInputService.StartPolling();

        base.Start();
    }

    /// <inheritdoc/>
    protected override void Stop()
    {
        this.xInputService.StopPolling();
        this.xInputService.StateChanged -= this.XInputService_StateChanged;
        instance = null;

        base.Stop();
    }

    /// <inheritdoc/>
    public override void AddMappedOption(AbstractMappedOption mappedOption)
    {
        var trigger = mappedOption.Trigger as GamePadTriggerTrigger;
        var lookup = this.triggerLookup[trigger.TriggerSide];

        lookup.Add(mappedOption);
    }

    /// <inheritdoc/>
    public override bool GetIsTriggered(AbstractTrigger trigger)
    {
        if (trigger is not GamePadTriggerTrigger gamePadTriggerTrigger)
        {
            return false;
        }

        var state = this.xInputService.GetState(gamePadTriggerTrigger.GamePadIndex);

        return state.Gamepad.IsTriggerPulled(gamePadTriggerTrigger.TriggerSide, gamePadTriggerTrigger.DeltaMargin);
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

        foreach (var stickAxisLookupKvp in this.triggerLookup)
        {
            var sideToLookup = stickAxisLookupKvp.Key;
            var lookup = stickAxisLookupKvp.Value;

            foreach (var mappedOption in lookup)
            {
                if (this.GetIsTriggered(mappedOption.Trigger))
                {
                    mappedOptions.Add(mappedOption);
                }
            }
        }

        GamePadTriggerInputBag inputBag = new()
        {
            LeftTriggerDelta = state.Gamepad.GetTriggerDelta(GamePadSide.Left),
            RightTriggerDelta = state.Gamepad.GetTriggerDelta(GamePadSide.Right),
        };

        this.DoExecuteTrigger(
            mappedOptions,
            inputBag,
            this.GetIsTriggered
        );
    }
}
