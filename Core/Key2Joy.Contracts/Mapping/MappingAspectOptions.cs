using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Contracts.Mapping
{
    [Serializable]
    public class MappingAspectOptions : Dictionary<string, object>, ISafeSerializationData
    {
        public MappingAspectOptions() { }

        protected MappingAspectOptions(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public void CompleteDeserialization(object deserialized)
        {
            
        }
    }
}
