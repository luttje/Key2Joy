using System;
using System.Windows.Forms;
using Key2Joy.LowLevelInput;
using Linearstar.Windows.RawInput.Native;
using Newtonsoft.Json;

namespace Key2Joy.Mapping
{
    [Trigger(
        Description = "Mouse Button Event",
        OptionsUserControl = typeof(MouseButtonTriggerOptionsControl)
    )]
    public class MouseButtonTrigger : BaseTrigger, IPressState, IReturnInputHash, IEquatable<MouseButtonTrigger>
    {
        public const string PREFIX_UNIQUE = nameof(MouseButtonTrigger);

        [JsonProperty]
        public Mouse.Buttons MouseButtons { get; set; }

        [JsonProperty]
        public PressState PressState { get; set; }

        [JsonConstructor]
        public MouseButtonTrigger(string name, string description)
            : base(name, description)
        { }

        internal override TriggerListener GetTriggerListener()
        {
            return MouseButtonTriggerListener.Instance;
        }

        internal override string GetUniqueKey()
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
        public override int CompareTo(BaseTrigger other)
        {
            if (other == null || !(other is MouseButtonTrigger otherMouseTrigger))
                return base.CompareTo(other);

            return $"{MouseButtons}#{(int)PressState}"
                .CompareTo($"{otherMouseTrigger.MouseButtons}#{(int)otherMouseTrigger.PressState}");
        }

        public override bool Equals(object obj)
        {
            if(!(obj is MouseButtonTrigger other)) 
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

        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
}
