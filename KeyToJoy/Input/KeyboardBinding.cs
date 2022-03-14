using System;
using Linearstar.Windows.RawInput.Native;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace KeyToJoy.Input
{
    internal class KeyboardBinding : Binding, IEquatable<KeyboardBinding>
    {
        private Keys keyBinding;

        [JsonConstructor]
        internal KeyboardBinding(string name)
        {
            this.keyBinding = (Keys)Enum.Parse(typeof(Keys), name);
        }

        internal KeyboardBinding(Keys keys, RawKeyboardFlags? flags = null)
        {
            keyBinding = keys;

            if (flags == null)
                return;

            if ((flags & RawKeyboardFlags.KeyE0) == RawKeyboardFlags.KeyE0)
            {
                if (keys == Keys.ControlKey)
                    keyBinding = Keys.RControlKey;
                if (keys == Keys.ShiftKey)
                    keyBinding = Keys.RShiftKey;
            }
            else
            {
                if (keys == Keys.ControlKey)
                    keyBinding = Keys.LControlKey;
                if (keys == Keys.ShiftKey)
                    keyBinding = Keys.LShiftKey;
            }
        }

        internal override string GetUniqueBindingKey()
        {
            return ToString();
        }

        public override bool Equals(object obj)
        {
            if(!(obj is KeyboardBinding other)) 
                return false;

            return Equals(other);
        }

        public bool Equals(KeyboardBinding other)
        {
            return keyBinding == other.keyBinding;
        }

        public override string ToString()
        {
            return keyBinding.ToString();
        }

        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
}
