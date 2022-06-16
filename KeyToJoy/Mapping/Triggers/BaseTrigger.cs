using Newtonsoft.Json;
using System;

namespace KeyToJoy.Mapping
{
    [JsonObject(MemberSerialization.OptIn)]
    internal abstract class BaseTrigger: ICloneable
    {

        [JsonProperty]
        public string Name => GetUniqueBindingKey();

        // Must return an input value unique in the preset. Like a Keys combination or an AxisDirection.
        internal abstract string GetUniqueBindingKey();

        public static bool operator ==(BaseTrigger a, BaseTrigger b)
        {
            if (System.Object.ReferenceEquals(a, b))
                return true;

            if (System.Object.ReferenceEquals(a, null)
                || System.Object.ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }
        public static bool operator !=(BaseTrigger a, BaseTrigger b) => !(a == b);

        public abstract object Clone();
    }
}
