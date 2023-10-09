using System;
using System.Runtime.Serialization;

namespace Key2Joy.Contracts.Plugins;

[Serializable]
public class PluginLoadException : Exception, ISafeSerializationData
{
    public PluginLoadException(string message) : base(message)
    {
    }

    public void CompleteDeserialization(object deserialized) => throw new NotImplementedException();
}
