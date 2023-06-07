using Key2Joy.Contracts.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Key2Joy.Mapping
{
    [Trigger(
        Description = "Multiple Triggers Combined",
        Visibility = MappingMenuVisibility.OnlyTopLevel
    )]
    public class CombinedTrigger : CoreTrigger, IEquatable<CombinedTrigger>
    {
        public const string PREFIX_UNIQUE = nameof(CombinedTrigger);

        // TODO: [JsonProperty(ItemTypeNameHandling = TypeNameHandling.All)]
        public List<AbstractTrigger> Triggers { get; set; }

        [JsonConstructor]
        public CombinedTrigger(string name)
            : base(name)
        { }

        public override AbstractTriggerListener GetTriggerListener()
        {
            return CombinedTriggerListener.Instance;
        }

        public override string GetUniqueKey()
        {
            return ToString();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is CombinedTrigger other))
                return false;

            return Equals(other);
        }

        public bool Equals(CombinedTrigger other)
        {
            return Triggers.SequenceEqual(other.Triggers);
        }

        public override string ToString()
        {
            return string.Join(" + ", Triggers);
        }

        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
}
