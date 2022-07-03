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
    public class KeyboardTrigger : BaseTrigger, IEquatable<KeyboardTrigger>
    {
        public const string PREFIX_UNIQUE = nameof(KeyboardTrigger);

        private Keys keys;


        [JsonProperty]
        public Keys Keys { get; set; }
        
        [JsonProperty]
        public PressState PressedState { get; set; }

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
                && PressedState == other.PressedState;
        }

        public override string ToString()
        {
            string format = "(keyboard) {1} {0}";
            return format.Replace("{0}", Keys.ToString())
                .Replace("{1}", Enum.GetName(typeof(PressState), PressedState));
        }

        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
}
