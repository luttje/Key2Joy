using System;

namespace Key2Joy.Plugins;

public enum PluginLoadStates
{
    NotLoaded,
    Loaded,
    FailedToLoad
}

public class PluginLoadState
{
    public string Name { get; set; }
    public string Author { get; set; }
    public string Website { get; set; }
    public string AssemblyPath { get; set; }

    public PluginLoadStates LoadState { get; set; } = PluginLoadStates.NotLoaded;
    public string LoadErrorMessage { get; set; } = null;

    public Type PluginType { get; set; } = null;
    internal PluginHostProxy PluginHost { get; private set; } = null;

    public PluginLoadState(string assemblyPath, Type pluginType = null)
    {
        this.AssemblyPath = assemblyPath;
        this.PluginType = pluginType;
    }

    internal void SetPluginHost(PluginHostProxy pluginHost)
    {
        this.PluginHost = pluginHost;

        if (pluginHost == null)
        {
            return;
        }

        this.Name = pluginHost.GetPluginName();
        this.Author = pluginHost.GetPluginAuthor();
        this.Website = pluginHost.GetPluginWebsite();
    }
}
