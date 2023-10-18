using System;
using CommandLine;
using Key2Joy.Interop;
using Key2Joy.Interop.Commands;
using Key2Joy.Mapping;

namespace Key2Joy.Cmd;

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

    public override void Handle(IInteropClient client)
    {
        var profilePath = this.ProfilePath;

        if (this.ProfilePath != null)
        {
            profilePath = MappingProfile.ResolveProfilePath(profilePath);
        }
        else
        {
            profilePath = MappingProfile.ResolveLastLoadedProfilePath();
        }

        try
        {
            client.SendCommand(new EnableCommand
            {
                ProfilePath = profilePath
            });

            Console.WriteLine($"Commanded Key2Joy to enable the profile: {profilePath}");
        }
        catch (TimeoutException)
        {
            this.SafelyRetry(() =>
            {
                Console.WriteLine("Key2Joy is not running, starting it now...");
                Key2JoyManager.StartKey2Joy();
                this.Handle(client);
            });
        }
    }
}
