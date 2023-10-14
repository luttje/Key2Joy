using System;

namespace Key2Joy.Contracts.Plugins;

public abstract class PluginBase : MarshalByRefObject
{
    public abstract string Name { get; }
    public abstract string Description { get; }
    public abstract string Author { get; }
    public abstract string Website { get; }

    public string PluginDirectory { get; set; }
    public string PluginDataDirectory { get; set; }

    public virtual void Initialize()
    { }
}
