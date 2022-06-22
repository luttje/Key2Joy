using System;
using System.Windows.Forms;
using KeyToJoy.Input.LowLevel;
using Linearstar.Windows.RawInput.Native;
using Newtonsoft.Json;
using MouseButtons = KeyToJoy.Mapping.MouseButtons;

namespace KeyToJoy.Mapping
{
    [Trigger(
        Name = "Mouse Button Event",
        OptionsUserControl = typeof(MouseButtonTriggerOptionsControl)
    )]
    public class MouseButtonTrigger : BaseTrigger, IEquatable<MouseButtonTrigger>
    {
        public const string PREFIX_UNIQUE = nameof(MouseButtonTrigger);

        [JsonProperty]
        public MouseButtons MouseButtons { get; set; }


        [JsonConstructor]
        public MouseButtonTrigger(string name, string imageResource)
            :base(name, imageResource)
        { }
        
        internal override string GetUniqueKey()
        {
            return $"{PREFIX_UNIQUE}_{MouseButtons}";
        }

        public override bool Equals(object obj)
        {
            if(!(obj is MouseButtonTrigger other)) 
                return false;

            return Equals(other);
        }

        public bool Equals(MouseButtonTrigger other)
        {
            return MouseButtons == other.MouseButtons;
        }

        public override string ToString()
        {
            return "(mouse) " + MouseButtons.ToString();
        }

        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
}
