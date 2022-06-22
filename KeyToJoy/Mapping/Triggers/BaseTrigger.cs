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

        private string imageResource;

        internal BaseTrigger(string name, string imageResource)
        {
            this.Name = name;
            this.imageResource = imageResource;
        }

        public virtual Image GetImage()
        {
            return imageResource != null
                ? (Bitmap)Resources.ResourceManager.GetObject(imageResource)
                : null;
        }

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
