using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Key2Joy.Config;

public class ConfigState
{
    private IConfigManager configManager;

    [JsonConstructor]
    public ConfigState()
    { }

    public ConfigState(IConfigManager configManager)
        => this.configManager = configManager;

    public string LastInstallPath
    {
        get => this.lastInstallPath;
        set => this.SaveIfInitialized(this.lastInstallPath = value);
    }

    private string lastInstallPath;

    [EnumConfigControl(
        Text = "Mapped options grouping style",
        EnumType = typeof(ViewMappingGroupType)
    )]
    public ViewMappingGroupType SelectedViewMappingGroupType
    {
        get => this.selectedViewMappingGroupType;
        set => this.SaveIfInitialized(this.selectedViewMappingGroupType = value);
    }

    private ViewMappingGroupType selectedViewMappingGroupType = ViewMappingGroupType.ByAction;

    [BooleanConfigControl(
        Text = "Minimize app when pressing the close button"
    )]
    public bool ShouldCloseButtonMinimize
    {
        get => this.shouldCloseButtonMinimize;
        set => this.SaveIfInitialized(this.shouldCloseButtonMinimize = value);
    }

    private bool shouldCloseButtonMinimize;

    [TextConfigControl(
        Text = "Last loaded mapping profile file location"
    )]
    public string LastLoadedProfile
    {
        get => this.lastLoadedProfile;
        set => this.SaveIfInitialized(this.lastLoadedProfile = value);
    }

    private string lastLoadedProfile;

    #region Listener Overrides

    [BooleanConfigControl(
        Text = "While armed override the default Keyboard behaviour for mapped keys"
    )]
    public bool ListenerOverrideDefaultKeyboard
    {
        get => this.listenerOverrideDefaultKeyboard;
        set => this.SaveIfInitialized(this.listenerOverrideDefaultKeyboard = value);
    }

    private bool listenerOverrideDefaultKeyboard = true;

    [BooleanConfigControl(
        Text = "While armed override the default Keyboard behaviour for all keys",
        Hint = "Make sure you map an 'Abort' action to a key, so you can disarm the mappings."
    )]
    public bool ListenerOverrideDefaultKeyboardAll
    {
        get => this.listenerOverrideDefaultKeyboardAll;
        set => this.SaveIfInitialized(this.listenerOverrideDefaultKeyboardAll = value);
    }

    private bool listenerOverrideDefaultKeyboardAll = false;

    [BooleanConfigControl(
        Text = "While armed override the default Mouse behaviour for mapped buttons"
    )]
    public bool ListenerOverrideDefaultMouse
    {
        get => this.listenerOverrideDefaultMouse;
        set => this.SaveIfInitialized(this.listenerOverrideDefaultMouse = value);
    }

    private bool listenerOverrideDefaultMouse = true;

    [BooleanConfigControl(
        Text = "While armed override the default Mouse behaviour for all buttons",
        Hint = "Make sure you map an 'Abort' action to a key, otherwise you can't click the disarm checkbox!"
    )]
    public bool ListenerOverrideDefaultMouseAll
    {
        get => this.listenerOverrideDefaultMouseAll;
        set => this.SaveIfInitialized(this.listenerOverrideDefaultMouseAll = value);
    }

    private bool listenerOverrideDefaultMouseAll = false;

    [BooleanConfigControl(
        Text = "While armed override the default Mouse Move behaviour for mapped moves",
        Hint = "This cannot properly override moves in a specific direction. Use the 'Any' mouse move direction instead."
    )]
    public bool ListenerOverrideDefaultMouseMove
    {
        get => this.listenerOverrideDefaultMouseMove;
        set => this.SaveIfInitialized(this.listenerOverrideDefaultMouseMove = value);
    }

    private bool listenerOverrideDefaultMouseMove = false;

    [BooleanConfigControl(
        Text = "While armed override the default Mouse Move behaviour for all moves",
        Hint = "Make sure you map an 'Abort' action to a key, otherwise you can't move to click the disarm checkbox!"
    )]
    public bool ListenerOverrideDefaultMouseMoveAll
    {
        get => this.listenerOverrideDefaultMouseMoveAll;
        set => this.SaveIfInitialized(this.listenerOverrideDefaultMouseMoveAll = value);
    }

    private bool listenerOverrideDefaultMouseMoveAll = false;

    #endregion Listener Overrides

    public Dictionary<string, string> EnabledPlugins { get; set; } = new Dictionary<string, string>();

    private void SaveIfInitialized(object changedValue = null)
    {
        if (this.configManager == null || !this.configManager.IsInitialized)
        {
            return;
        }

        this.configManager.Save();
    }
}
