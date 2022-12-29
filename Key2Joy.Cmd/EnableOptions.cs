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
    [Verb("enable", HelpText = "Enable the provided preset profile.")]
    internal class EnableOptions : Options
    {
        [Option(
            shortName: 'p', 
            longName: "preset",
            Required = false,
            HelpText = "Path of the preset to load or modify. Path can be relative to preset directory. By default the last used preset is selected (if available)."
        )]
        public string PresetPath { get; set; }

        public override void Handle()
        {
            if (PresetPath == null)
                PresetPath = ConfigManager.Instance.LastLoadedPreset;

            try 
            { 
                InteropClient.Instance.SendCommand(new EnableCommand
                {
                    ProfilePath = PresetPath
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
