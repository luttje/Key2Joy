using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Key2Joy.Contracts.Mapping
{
    public abstract class AbstractMappedOption : ICloneable
    {
        [JsonInclude]
        public AbstractAction Action;
        [JsonInclude]
        public AbstractTrigger Trigger;

        public abstract object Clone();
    }
}
