using System.Collections.Generic;
using CommonServiceLocator;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Triggers;
using Key2Joy.LowLevelInput.XInput;

namespace Key2Joy.Mapping.Triggers.GamePad;

public class GamePadButtonTriggerListener : PressReleaseTriggerListener<GamePadButtonTrigger>
{
    private static GamePadButtonTriggerListener instance;

    public static GamePadButtonTriggerListener Instance
    {
        get
        {
            instance ??= new GamePadButtonTriggerListener();

            return instance;
        }
    }

    private readonly IXInputService xInputService;
    private readonly Dictionary<GamePadButton, bool> currentKeysDown = new();

    private GamePadButtonTriggerListener()
    {
        this.xInputService = ServiceLocator.Current.GetInstance<IXInputService>();
        this.currentKeysDown = new();
    }

    public bool GetKeyDown(GamePadButton key)
        => this.currentKeysDown.ContainsKey(key);

    protected override void Start()
    {
        for (var i = 0; i < XInputService.MaxDevices; i++)
        {
            this.xInputService.RegisterDevice(i);
        }

        this.xInputService.StateChanged += this.XInputService_StateChanged;
        this.xInputService.StartPolling();
        this.currentKeysDown.Clear();

        base.Start();
    }

    protected override void Stop()
    {
        this.xInputService.StopPolling();
        this.xInputService.StateChanged -= this.XInputService_StateChanged;
        instance = null;
        this.currentKeysDown.Clear();

        base.Stop();
    }

    public override bool GetIsTriggered(AbstractTrigger trigger)
    {
        if (trigger is not GamePadButtonTrigger gamePadButtonTrigger)
        {
            return false;
        }

        return this.currentKeysDown.ContainsKey(gamePadButtonTrigger.Button);
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
        var mappedOptions = new List<AbstractMappedOption>();

        GamePadButtonInputBag inputBag = new()
        {
            PressedButtons = state.Gamepad.GetPressedButtonsList(),
            State = state.Gamepad,
        };

        // Finds all mapped options that are triggered by newly pressed buttons
        foreach (var button in inputBag.PressedButtons)
        {
            if (this.currentKeysDown.ContainsKey(button))
            {
                continue; // Prevent firing multiple times for a single button press
            }

            this.currentKeysDown.Add(button, true);

            var hash = GamePadButtonTrigger.GetInputHashFor(button);

            if (this.LookupPress.TryGetValue(hash, out var dictionaryMappedOptions))
            {
                mappedOptions.AddRange(dictionaryMappedOptions);
            }
        }

        // Go through all buttons that are down, but not pressed anymore
        // and find all mapped options that are triggered by them
        var releasedButtons = new List<GamePadButton>();

        foreach (var button in this.currentKeysDown.Keys)
        {
            if (inputBag.PressedButtons.Contains(button))
            {
                continue;
            }

            releasedButtons.Add(button);

            var hash = GamePadButtonTrigger.GetInputHashFor(button);

            if (this.LookupRelease.TryGetValue(hash, out var dictionaryMappedOptions))
            {
                mappedOptions.AddRange(dictionaryMappedOptions);
            }
        }

        inputBag.ReleasedButtons = releasedButtons;

        if (this.DoExecuteTrigger(
            mappedOptions,
            inputBag,
            this.GetIsTriggered)
        )
        {
            // TODO: Override default behaviour (if possible, if not then we should let the user know)
        }
    }
}
