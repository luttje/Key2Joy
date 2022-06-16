using System;
using Linearstar.Windows.RawInput.Native;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace KeyToJoy.Mapping
{
    internal class KeyboardTrigger : BaseTrigger, IEquatable<KeyboardTrigger>
    {
        private Keys keys;

        [JsonConstructor]
        internal KeyboardTrigger(string name)
        {
            this.keys = (Keys)Enum.Parse(typeof(Keys), name);
        }

        internal KeyboardTrigger(Keys keys, RawKeyboardFlags? flags = null)
        {
            this.keys = keys;

            if (flags == null)
                return;

            if ((flags & RawKeyboardFlags.KeyE0) == RawKeyboardFlags.KeyE0)
            {
                if (keys == Keys.ControlKey)
                    this.keys = Keys.RControlKey;
                if (keys == Keys.ShiftKey)
                    this.keys = Keys.RShiftKey;
            }
            else
            {
                if (keys == Keys.ControlKey)
                    this.keys = Keys.LControlKey;
                if (keys == Keys.ShiftKey)
                    this.keys = Keys.LShiftKey;
            }
        }

        internal override string GetUniqueBindingKey()
        {
            return ToString();
        }

        public override bool Equals(object obj)
        {
            if(!(obj is KeyboardTrigger other)) 
                return false;

            return Equals(other);
        }

        public bool Equals(KeyboardTrigger other)
        {
            return keys == other.keys;
        }

        public override string ToString()
        {
            return keys.ToString();
        }

        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
}
