using Key2Joy.Contracts.Plugins;

namespace Key2Joy.Tests.Stubs.TestPlugin;

public class Plugin : PluginBase
{
    public override string Name => "TestPlugin";
    public override string Description => "TestPlugin plugin for Key2Joy";
    public override string Author => "Luttje";
    public override string Website => "https://github.com/luttje/Key2Joy";

    public override void Initialize()
    { }
}
