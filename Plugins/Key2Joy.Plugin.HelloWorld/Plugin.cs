using Key2Joy.Contracts.Plugins;

namespace Key2Joy.Plugin.HelloWorld;

public class Plugin : PluginBase
{
    public override string Name => "HelloWorld";
    public override string Description => "Plugin demonstration for Key2Joy";
    public override string Author => "Luttje";
    public override string Website => "https://github.com/luttje/Key2Joy";

    public override void Initialize()
    { }
}
