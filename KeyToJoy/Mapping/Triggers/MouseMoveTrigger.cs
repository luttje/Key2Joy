using Newtonsoft.Json;
using System;

namespace KeyToJoy.Mapping
{
    internal class MouseMoveTrigger : BaseTrigger
    {
        const string PREFIX = "Mouse Move ";
        private AxisDirection axisBinding;

        [JsonConstructor]
        internal MouseMoveTrigger(string name)
        {
            try
            {
                this.axisBinding = (AxisDirection)Enum.Parse(typeof(AxisDirection), name.Replace(PREFIX, ""));
            }
            catch(ArgumentException ex)
            {
                // Handle profile file versions before 2
                var oldPrefix = "Mouse ";
                this.axisBinding = (AxisDirection)Enum.Parse(typeof(AxisDirection), name.Replace(oldPrefix, ""));
            }
        }

        internal MouseMoveTrigger(AxisDirection axisBinding)
        {
            this.axisBinding = axisBinding;
        }

        internal override string GetUniqueBindingKey()
        {
            return ToString();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is MouseMoveTrigger other)) return false;

            return axisBinding == other.axisBinding;
        }

        public override string ToString()
        {
            var axis = Enum.GetName(typeof(AxisDirection), axisBinding);

            return PREFIX + axis;
        }

        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
}
