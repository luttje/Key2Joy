using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Contracts.Mapping
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class AbstractMappedOption : ICloneable
    {
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        public AbstractAction Action;
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        public AbstractTrigger Trigger;

        public abstract object Clone();
    }
}
