using Key2Joy.Contracts.Mapping;
using Key2Joy.LowLevelInput;
using System;
using System.Text.Json.Serialization;

namespace Key2Joy.Mapping
{
    [Trigger(
        Description = "Mouse Button Event"
    )]
    public class MouseButtonTrigger : CoreTrigger, IPressState, IReturnInputHash, IEquatable<MouseButtonTrigger>
    {
        public const string PREFIX_UNIQUE = nameof(MouseButtonTrigger);

        public Mouse.Buttons MouseButtons { get; set; }
        public PressState PressState { get; set; }

        [JsonConstructor]
        public MouseButtonTrigger(string name)
            : base(name)
        { }

        public override AbstractTriggerListener GetTriggerListener()
        {
            return MouseButtonTriggerListener.Instance;
        }

        public override string GetUniqueKey()
        {
            return $"{PREFIX_UNIQUE}_{MouseButtons}";
        }

        public static int GetInputHashFor(Mouse.Buttons mouseButtons)
        {
            return (int)mouseButtons;
        }

        public int GetInputHash()
        {
            return GetInputHashFor(MouseButtons);
        }

        // Keep Press and Release together while sorting
        public override int CompareTo(AbstractMappingAspect other)
        {
            if (other == null || !(other is MouseButtonTrigger otherMouseTrigger))
                return base.CompareTo(other);

            return $"{MouseButtons}#{(int)PressState}"
                .CompareTo($"{otherMouseTrigger.MouseButtons}#{(int)otherMouseTrigger.PressState}");
        }

        public override bool Equals(object obj)
        {
            if (!(obj is MouseButtonTrigger other))
                return false;

            return Equals(other);
        }

        public bool Equals(MouseButtonTrigger other)
        {
            return MouseButtons == other.MouseButtons
                && PressState == other.PressState;
        }

        public override string ToString()
        {
            string format = "(mouse) {1} {0}";
            return format.Replace("{0}", MouseButtons.ToString())
                .Replace("{1}", Enum.GetName(typeof(PressState), PressState));
        }
    }
}
