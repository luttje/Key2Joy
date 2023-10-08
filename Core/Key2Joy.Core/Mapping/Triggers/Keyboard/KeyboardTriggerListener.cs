using System.Collections.Generic;
using System.Windows.Forms;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Triggers;
using Key2Joy.LowLevelInput;

namespace Key2Joy.Mapping.Triggers.Keyboard;

public class KeyboardTriggerListener : PressReleaseTriggerListener<KeyboardTrigger>
{
    private static KeyboardTriggerListener instance;
    public static KeyboardTriggerListener Instance
    {
        get
        {
            instance ??= new KeyboardTriggerListener();

            return instance;
        }
    }

    private GlobalInputHook globalKeyboardHook;
    private readonly Dictionary<Keys, bool> currentKeysDown = new();

    public bool GetKeyDown(Keys key) => this.currentKeysDown.ContainsKey(key);

    protected override void Start()
    {
        // This captures global keyboard input and blocks default behaviour by setting e.Handled
        this.globalKeyboardHook = new GlobalInputHook();
        this.globalKeyboardHook.KeyboardInputEvent += this.OnKeyInputEvent;
        this.currentKeysDown.Clear();

        base.Start();
    }

    protected override void Stop()
    {
        instance = null;
        this.globalKeyboardHook.KeyboardInputEvent -= this.OnKeyInputEvent;
        this.globalKeyboardHook.Dispose();
        this.globalKeyboardHook = null;
        this.currentKeysDown.Clear();

        base.Stop();
    }

    public override bool GetIsTriggered(AbstractTrigger trigger)
    {
        if (trigger is not KeyboardTrigger keyboardTrigger)
        {
            return false;
        }

        return this.currentKeysDown.ContainsKey(keyboardTrigger.Keys);
    }

    private void OnKeyInputEvent(object sender, GlobalKeyboardHookEventArgs e)
    {
        if (!this.IsActive)
        {
            return;
        }

        // Test if this is a bound key, if so halt default input behaviour
        var keys = VirtualKeyConverter.KeysFromVirtual(e.KeyboardData.VirtualCode);
        Dictionary<int, List<AbstractMappedOption>> dictionary;

        if (e.KeyboardState == KeyboardState.KeyDown)
        {
            dictionary = this.lookupDown;

            if (this.currentKeysDown.ContainsKey(keys))
            {
                return; // Prevent firing multiple times for a single key press
            }
            else
            {
                this.currentKeysDown.Add(keys, true);
            }
        }
        else
        {
            dictionary = this.lookupRelease;

            if (this.currentKeysDown.ContainsKey(keys))
            {
                this.currentKeysDown.Remove(keys);
            }
        }

        KeyboardInputBag inputBag = new()
        {
            State = e.KeyboardState,
            Keys = keys
        };

        var hash = KeyboardTrigger.GetInputHashFor(keys);
        dictionary.TryGetValue(hash, out var mappedOptions);

        if (this.DoExecuteTrigger(
            mappedOptions,
            inputBag,
            trigger =>
            {
                var keyboardTrigger = trigger as KeyboardTrigger;
                return keyboardTrigger.GetInputHash() == hash
                    && keyboardTrigger.GetKeyboardState() == e.KeyboardState;
            }))
        {
            e.Handled = true;
        }
    }
}
