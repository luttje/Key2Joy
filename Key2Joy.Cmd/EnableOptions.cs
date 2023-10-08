using System;
using CommandLine;
using Key2Joy.Interop;
using Key2Joy.Mapping;

namespace Key2Joy.Cmd
{
    [Verb("enable", HelpText = "Enable the provided mapping profile.")]
    internal class EnableOptions : Options
    {
        [Option(
            shortName: 'p',
            longName: "profile",
            Required = false,
            HelpText = "Path of the profile to load or modify. Path can be relative to profile directory. By default the last used profile is selected (if available)."
        )]
        public string ProfilePath { get; set; }

        public override void Handle()
        {
            MappingProfile profile;

            if (this.ProfilePath != null)
            {
                profile = MappingProfile.Load(this.ProfilePath);
            }
            else
            {
                profile = MappingProfile.RestoreLastLoaded();
            }

            try
            {
                InteropClient.Instance.SendCommand(new EnableCommand
                {
                    ProfilePath = profile.FilePath
                });

                Console.WriteLine($"Commanded Key2Joy to enable the profile: {profile.FilePath}");
            }
            catch (TimeoutException)
            {
                this.SafelyRetry(() =>
                {
                    Console.WriteLine("Key2Joy is not running, starting it now...");
                    Key2JoyManager.StartKey2Joy();
                    this.Handle();
                });
            }
        }
    }
}
