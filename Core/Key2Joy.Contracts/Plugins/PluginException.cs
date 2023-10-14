using System;
using System.Runtime.Serialization;

namespace Key2Joy.Contracts.Plugins;

[Serializable]
public class PluginException : Exception, ISafeSerializationData
{
    public PluginException()
        : base()
    { }

    public PluginException(string message)
        : base(message)
    { }

    protected PluginException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    public void CompleteDeserialization(object deserialized)
    {
    }

    /// <summary>
    /// Takes interesting parts from the exception for the message of a new exception.
    /// </summary>
    /// <param name="ex"></param>
    /// <returns></returns>
    public static PluginException FromException(PluginBase plugin, Exception ex)
    {
        var message = $"[Plugin: {plugin.Name}] Error:\r\n{ex.Message}\r\n{ex.StackTrace}\r\n";

        return new PluginException(message);
    }
}
