using Key2Joy.Contracts.Plugins;

namespace Key2Joy.Plugin.Midi;

public class Plugin : PluginBase
{
    public override string Name => "Midi";
    public override string Description => "Midi tools to listen for and simulate midi input/output.";
    public override string Author => "Luttje";
    public override string Website => "https://github.com/luttje/Key2Joy";
}
