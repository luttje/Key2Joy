using Newtonsoft.Json;
using System;

namespace KeyToJoy.Mapping
{
    [Trigger(
        Name = "Mouse Move Event",
        OptionsUserControl = typeof(MouseMoveTriggerOptionsControl)
    )]
    public class MouseMoveTrigger : BaseTrigger
    {
        const string PREFIX = "Mouse Move ";
        private AxisDirection axisBinding;

        [JsonConstructor]
        public MouseMoveTrigger(string name, string imageResource)
            : base(name, imageResource)
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

        //internal MouseMoveTrigger(AxisDirection axisBinding)
        //{
        //    this.axisBinding = axisBinding;
        //}

        internal override string GetUniqueKey()
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
