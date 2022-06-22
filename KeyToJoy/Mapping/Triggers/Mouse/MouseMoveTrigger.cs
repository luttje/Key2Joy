using Newtonsoft.Json;
using System;

namespace KeyToJoy.Mapping
{
    [Trigger(
        Name = "Mouse Move Event",
        OptionsUserControl = typeof(MouseMoveTriggerOptionsControl)
    )]
    public class MouseMoveTrigger : BaseTrigger
    {
        public const string PREFIX_UNIQUE = nameof(MouseButtonTrigger);

        [JsonProperty]
        public AxisDirection AxisBinding { get; set; }

        [JsonConstructor]
        public MouseMoveTrigger(string name, string imageResource)
            : base(name, imageResource)
        { }

        internal override string GetUniqueKey()
        {
            return $"{PREFIX_UNIQUE}_{AxisBinding}";
        }

        public override bool Equals(object obj)
        {
            if (!(obj is MouseMoveTrigger other)) return false;

            return AxisBinding == other.AxisBinding;
        }

        public override string ToString()
        {
            var axis = Enum.GetName(typeof(AxisDirection), AxisBinding);

            return axis;
        }

        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
}
