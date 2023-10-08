using System;

namespace Key2Joy.Contracts.Plugins;

public class PluginLoadException : Exception
{
    public PluginLoadException(string message) : base(message)
    {
    }
}
