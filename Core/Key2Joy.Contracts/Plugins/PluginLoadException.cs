using System;
using System.Runtime.Serialization;

namespace Key2Joy.Contracts.Plugins;

[Serializable]
public class PluginLoadException : PluginException
{
    public PluginLoadException()
        : base()
    { }

    public PluginLoadException(string message)
        : base(message)
    { }

    protected PluginLoadException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
