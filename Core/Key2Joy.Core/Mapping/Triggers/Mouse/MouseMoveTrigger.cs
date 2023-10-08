using Key2Joy.Contracts.Mapping;
using System;
using System.Text.Json.Serialization;

namespace Key2Joy.Mapping
{
    [Trigger(
        Description = "Mouse Move Event"
    )]
    public class MouseMoveTrigger : CoreTrigger, IReturnInputHash
    {
        public const string PREFIX_UNIQUE = nameof(MouseButtonTrigger);

        public AxisDirection AxisBinding { get; set; }

        [JsonConstructor]
        public MouseMoveTrigger(string name)
            : base(name)
        { }

        public override AbstractTriggerListener GetTriggerListener()
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
    }
}
