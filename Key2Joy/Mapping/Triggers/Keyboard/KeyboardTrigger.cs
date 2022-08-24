using System;
using System.Windows.Forms;
using Key2Joy.LowLevelInput;
using Newtonsoft.Json;

namespace Key2Joy.Mapping
{
    [Trigger(
        Description = "Keyboard Event",
        OptionsUserControl = typeof(KeyboardTriggerOptionsControl)
    )]
    public class KeyboardTrigger : BaseTrigger, IPressState, IReturnInputHash, IEquatable<KeyboardTrigger>
    {
        public const string PREFIX_UNIQUE = nameof(KeyboardTrigger);

        [JsonProperty]
        public Keys Keys { get; set; }
        
        [JsonProperty]
        public PressState PressState { get; set; }

        [JsonConstructor]
        public KeyboardTrigger(string name, string description)
            : base(name, description)
        { }

        internal override TriggerListener GetTriggerListener()
        {
            return KeyboardTriggerListener.Instance;
        }

        public static int GetInputHashFor(Keys keys)
        {
            return (int)keys;
        }

        public int GetInputHash()
        {
            return GetInputHashFor(Keys);
        }

        internal override string GetUniqueKey()
        {
            return $"{PREFIX_UNIQUE}_{Keys}";
        }

        // Keep Press and Release together while sorting
        public override int CompareTo(BaseTrigger other)
        {
            if (other == null || !(other is KeyboardTrigger otherKeyboardTrigger))
                return base.CompareTo(other);
            
            return $"{Keys}#{(int)PressState}"
                .CompareTo($"{otherKeyboardTrigger.Keys}#{(int)otherKeyboardTrigger.PressState}");
        }
        
        public override bool Equals(object obj)
        {
            if(!(obj is KeyboardTrigger other)) 
                return false;

            return Equals(other);
        }

        public bool Equals(KeyboardTrigger other)
        {
            return Keys == other.Keys 
                && PressState == other.PressState;
        }

        public override string ToString()
        {
            string format = "(keyboard) {1} {0}";
            return format.Replace("{0}", Keys.ToString())
                .Replace("{1}", Enum.GetName(typeof(PressState), PressState));
        }

        public override object Clone()
        {
            return MemberwiseClone();
        }

        internal KeyboardState GetKeyboardState()
        {
            if (PressState == PressState.Press)
                return KeyboardState.KeyDown;

            return KeyboardState.KeyUp;
        }
    }
}
