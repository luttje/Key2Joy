using System;
using Linearstar.Windows.RawInput.Native;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace KeyToJoy.Mapping
{
    [Trigger(
        Name = "Keyboard Event",
        OptionsUserControl = typeof(KeyboardTriggerOptionsControl)
    )]
    public class KeyboardTrigger : BaseTrigger, IEquatable<KeyboardTrigger>
    {
        public const string PREFIX_UNIQUE = nameof(KeyboardTrigger);

        [JsonProperty]
        public Keys Keys { get; set; }

        [JsonConstructor]
        public KeyboardTrigger(string name)
            : base(name)
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
            return Keys == other.Keys;
        }

        public override string ToString()
        {
            return $"(keyboard) Move {Keys}";
        }

        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
}
