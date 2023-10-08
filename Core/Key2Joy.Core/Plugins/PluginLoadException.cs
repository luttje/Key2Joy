using System;

namespace Key2Joy.Plugins;

internal class PluginLoadException : Exception
{
    public PluginLoadException(string message) : base(message)
    {
    }
}
