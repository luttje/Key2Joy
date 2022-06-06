using Newtonsoft.Json;
using System;

namespace KeyToJoy.Input
{
    [JsonObject(MemberSerialization.OptIn)]
    internal abstract class Binding: ICloneable
    {

        [JsonProperty]
        public string Name => GetUniqueBindingKey();

        // Must return an input value unique in the preset. Like a Keys combination or an AxisDirection.
        internal abstract string GetUniqueBindingKey();

        public static bool operator ==(Binding a, Binding b)
        {
            if (System.Object.ReferenceEquals(a, b))
                return true;

            return a.Equals(b);
        }
        public static bool operator !=(Binding a, Binding b) => !(a == b);

        public abstract object Clone();
    }
}
