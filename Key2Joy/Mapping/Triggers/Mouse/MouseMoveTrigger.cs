using Newtonsoft.Json;
using System;

namespace Key2Joy.Mapping
{
    [Trigger(
        Description = "Mouse Move Event"
    )]
    public class MouseMoveTrigger : BaseTrigger, IReturnInputHash
    {
        public const string PREFIX_UNIQUE = nameof(MouseButtonTrigger);

        [JsonProperty]
        public AxisDirection AxisBinding { get; set; }

        [JsonConstructor]
        public MouseMoveTrigger(string name, string description)
            : base(name, description)
        { }

        public override TriggerListener GetTriggerListener()
        {
            return MouseMoveTriggerListener.Instance;
        }

        public override string GetUniqueKey()
        {
            return $"{PREFIX_UNIQUE}_{AxisBinding}";
        }

        public static int GetInputHashFor(AxisDirection axisBinding)
        {
            return (int)axisBinding;
        }

        public int GetInputHash()
        {
            return GetInputHashFor(AxisBinding);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is MouseMoveTrigger other)) return false;

            return AxisBinding == other.AxisBinding;
        }

        public override string ToString()
        {
            var axis = Enum.GetName(typeof(AxisDirection), AxisBinding);
            return $"(mouse) Move {axis}";
        }

        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
}
