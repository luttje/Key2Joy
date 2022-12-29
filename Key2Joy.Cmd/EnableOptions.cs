using CommandLine.Text;
using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Key2Joy.Config;
using Key2Joy.Interop;

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
            if (ProfilePath == null)
                ProfilePath = ConfigManager.Instance.LastLoadedProfile;

            Console.WriteLine($"Commanding Key2Joy to enable the profile: {ProfilePath}");
            
            try 
            { 
                InteropClient.Instance.SendCommand(new EnableCommand
                {
                    ProfilePath = ProfilePath
                });
            }
            catch(TimeoutException)
            {
                // TODO: Start Key2Joy and try again
                throw new NotImplementedException("TODO: Start Key2Joy");
            }
        }
    }
}
