using Key2Joy.Contracts.Plugins;
using Key2Joy.Plugin.Midi.Mapping;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Plugin.Midi
{
    public class MidiPlugin : AbstractPlugin
    {
        public override string Name => "Midi";
        public override string Description => "Midi plugin for Key2Joy";
        public override string Author => "Luttje";
        public override string Website => "https://github.com/luttje/Key2Joy";

        public override IReadOnlyList<string> ActionFullTypeNames => new List<string>()
        {
            typeof(GetHelloWorldAction).FullName,
        };

        public override IReadOnlyList<string> TriggerFullTypeNames => new List<string>()
        {
            //typeof(GetHelloWorldTrigger).FullName,
        };

        public override void OnLoaded() 
        {
            // Write to plugin data directory (creating it if it doesn't exist)
            Directory.CreateDirectory(pluginDataDirectory);

            File.WriteAllText($"{pluginDataDirectory}/test.txt", "Hello World!");
        }
    }
}
