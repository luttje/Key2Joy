using System;
using System.Text.Json.Serialization;

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
