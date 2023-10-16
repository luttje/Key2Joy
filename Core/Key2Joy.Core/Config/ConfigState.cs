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

    [BooleanConfigControl(
        Text = "Mute informative message about this app minimizing by default"
    )]
    public bool MuteCloseExitMessage
    {
        get => this.muteCloseExitMessage;
        set => this.SaveIfInitialized(this.muteCloseExitMessage = value);
    }

    private bool muteCloseExitMessage;

    [BooleanConfigControl(
        Text = "Override default behaviour when trigger action is executed"
    )]
    public bool OverrideDefaultTriggerBehaviour
    {
        get => this.overrideDefaultTriggerBehaviour;
        set => this.SaveIfInitialized(this.overrideDefaultTriggerBehaviour = value);
    }

    private bool overrideDefaultTriggerBehaviour = true;

    [TextConfigControl(
        Text = "Last loaded mapping profile file location"
    )]
    public string LastLoadedProfile
    {
        get => this.lastLoadedProfile;
        set => this.SaveIfInitialized(this.lastLoadedProfile = value);
    }

    private string lastLoadedProfile;

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
