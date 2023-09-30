using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Config
{
    public class ConfigState
    {        
        public string LastInstallPath
        {
            get => lastInstallPath;
            set => SaveIfInitialized(lastInstallPath = value);
        }
        private string lastInstallPath;

        [BooleanConfigControl(
            Text = "Mute informative message about this app minimizing by default"
        )]
        public bool MuteCloseExitMessage
        {
            get => muteCloseExitMessage;
            set => SaveIfInitialized(muteCloseExitMessage = value);
        }
        private bool muteCloseExitMessage;

        [BooleanConfigControl(
            Text = "Override default behaviour when trigger action is executed"
        )]
        public bool OverrideDefaultTriggerBehaviour
        {
            get => overrideDefaultTriggerBehaviour;
            set => SaveIfInitialized(overrideDefaultTriggerBehaviour = value);
        }
        private bool overrideDefaultTriggerBehaviour = true;

        [TextConfigControl(
            Text = "Last loaded mapping profile file location"
        )]
        public string LastLoadedProfile
        {
            get => lastLoadedProfile;
            set => SaveIfInitialized(lastLoadedProfile = value);
        }
        private string lastLoadedProfile;

        [TextConfigControl(
            Text = "Path to directory where logs are saved"
        )]
        public string LogOutputPath
        {
            get => logOutputPath;
            set => SaveIfInitialized(logOutputPath = value);
        }
        private string logOutputPath = Path.Combine(ConfigManager.GetAppDirectory(), "Logs");

        public Dictionary<string, string> EnabledPlugins { get; set; } = new Dictionary<string, string>();

        private void SaveIfInitialized(object changedValue = null)
        {
            if (ConfigManager.Instance.IsInitialized)
                ConfigManager.Instance.Save();
        }
    }
}
