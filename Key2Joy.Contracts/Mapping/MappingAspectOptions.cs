using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Contracts.Mapping
{
    [Serializable]
    public class MappingAspectOptions : Dictionary<string, object>, ISerializable
    {
        public MappingAspectOptions() { }

        protected MappingAspectOptions(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
