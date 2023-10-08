using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Key2Joy.Contracts.Mapping;
using Key2Joy.Contracts.Mapping.Triggers;

namespace Key2Joy.Mapping.Triggers.Logic
{
    [Trigger(
        Description = "Multiple Triggers Combined",
        Visibility = MappingMenuVisibility.OnlyTopLevel
    )]
    public class CombinedTrigger : CoreTrigger, IEquatable<CombinedTrigger>
    {
        public const string PREFIX_UNIQUE = nameof(CombinedTrigger);

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
            return this.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj is not CombinedTrigger other)
            {
                return false;
            }

            return this.Equals(other);
        }

        public bool Equals(CombinedTrigger other)
        {
            return this.Triggers.SequenceEqual(other.Triggers);
        }

        public override string ToString()
        {
            return string.Join(" + ", this.Triggers);
        }
    }
}
