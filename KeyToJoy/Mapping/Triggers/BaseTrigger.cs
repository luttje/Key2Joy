using KeyToJoy.Properties;
using Newtonsoft.Json;
using System;
using System.Drawing;

namespace KeyToJoy.Mapping
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class BaseTrigger: ICloneable
    {
        [JsonProperty]
        internal string Name { get; set; }

        // Must return an input value unique in the preset. Like a Keys combination or an AxisDirection.
        // Will be used to quickly lookup input triggers and their corresponding action
        internal abstract string GetUniqueKey();

        public string ImageResource { get; set; }

        internal BaseTrigger(string name)
        {
            Name = name;
        }

        public virtual Image GetImage()
        {
            return ImageResource != null
                ? (Bitmap)Resources.ResourceManager.GetObject(ImageResource)
                : null;
        }

        public static bool operator ==(BaseTrigger a, BaseTrigger b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (ReferenceEquals(a, null)
                || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }
        public static bool operator !=(BaseTrigger a, BaseTrigger b) => !(a == b);

        public abstract object Clone();
    }
}
