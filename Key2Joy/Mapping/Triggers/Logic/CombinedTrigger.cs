using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Key2Joy.Mapping
{
    [Trigger(
        Description = "Multiple Triggers Combined",
        Visibility = MappingMenuVisibility.OnlyTopLevel,
        OptionsUserControl = typeof(CombinedTriggerOptionsControl)
    )]
    internal class CombinedTrigger : BaseTrigger, IEquatable<CombinedTrigger>
    {
        public const string PREFIX_UNIQUE = nameof(CombinedTrigger);

        [JsonProperty(ItemTypeNameHandling = TypeNameHandling.All)]
        public List<BaseTrigger> Triggers { get; set; }

        [JsonProperty]
        public int Timeout { get; set; }

        [JsonConstructor]
        public CombinedTrigger(string name, string description)
            : base(name, description)
        { }

        internal override TriggerListener GetTriggerListener()
        {
            return CombinedTriggerListener.Instance;
        }

        internal override string GetUniqueKey()
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
