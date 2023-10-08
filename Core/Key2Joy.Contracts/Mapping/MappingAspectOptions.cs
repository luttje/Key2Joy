using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Key2Joy.Contracts.Mapping;

[Serializable]
public class MappingAspectOptions : Dictionary<string, object>, ISafeSerializationData
{
    public MappingAspectOptions() { }

    protected MappingAspectOptions(SerializationInfo info, StreamingContext context) : base(info, context) { }

    public void CompleteDeserialization(object deserialized)
    {

    }
}
