using System;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace KeyToJoy.Mapping
{
    [Trigger(
        Description = "Keyboard Event",
        OptionsUserControl = typeof(KeyboardTriggerOptionsControl)
    )]
    public class KeyboardTrigger : BaseTrigger, IEquatable<KeyboardTrigger>
    {
        public const string PREFIX_UNIQUE = nameof(KeyboardTrigger);

        private Keys keys;


        [JsonProperty]
        public Keys Keys { get; set; }
        [JsonProperty]
        public bool PressedDown { get; set; }

        [JsonConstructor]
        public KeyboardTrigger(string name, string description)
            : base(name, description)
        { }

        internal override TriggerListener GetTriggerListener()
        {
            return KeyboardTriggerListener.Instance;
        }

        internal override string GetUniqueKey()
        {
            return $"{PREFIX_UNIQUE}_{Keys}";
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
                && PressedDown == other.PressedDown;
        }

        public override string ToString()
        {
            var state = PressedDown ? "Down" : "Released";
            
            return $"(keyboard) {Keys} [{state}]";
        }

        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
}
