using Key2Joy.Contracts.Plugins;
using Key2Joy.Plugin.HelloWorld.Mapping;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Plugin.HelloWorld
{
    public class Plugin : PluginBase
    {
        public override string Name => "HelloWorld";
        public override string Description => "HelloWorld plugin for Key2Joy";
        public override string Author => "Luttje";
        public override string Website => "https://github.com/luttje/Key2Joy";

        public override void Initialize() 
        { }
    }
}
