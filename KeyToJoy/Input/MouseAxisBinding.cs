using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyToJoy.Input
{
    internal class MouseAxisBinding : Binding
    {
        const string PREFIX = "Mouse ";
        private AxisDirection axisBinding;

        [JsonConstructor]
        internal MouseAxisBinding(string name)
        {
            this.axisBinding = (AxisDirection)Enum.Parse(typeof(AxisDirection), name.Replace(PREFIX, ""));
        }

        internal MouseAxisBinding(AxisDirection axisBinding)
        {
            this.axisBinding = axisBinding;
        }

        internal override string GetUniqueBindingKey()
        {
            return ToString();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is MouseAxisBinding other)) return false;

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
