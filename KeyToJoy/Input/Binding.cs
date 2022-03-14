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

        public static bool operator ==(Binding obj1, Binding obj2)
        {
            return obj1.Equals(obj2);
        }
        public static bool operator !=(Binding obj1, Binding obj2) => !(obj1 == obj2);

        public abstract object Clone();
    }
}
