using KeyToJoy.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyToJoy.Mapping
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class BaseAction
    {
        [JsonProperty]
        internal string Name { get; set; }

        public string ImageResource { get; set; }

        public BaseAction(string name)
        {
            Name = name;
        }

        internal abstract Task Execute(InputBag inputBag);

        public virtual string GetNameDisplay()
        {
            return Name;
        }

        public virtual string GetContextDisplay()
        {
            return ToString();
        }

        public virtual Image GetImage()
        {
            return ImageResource != null
                ? (Bitmap)Resources.ResourceManager.GetObject(ImageResource)
                : null;            
        }

        public static bool operator ==(BaseAction a, BaseAction b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (ReferenceEquals(a, null)
                || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }
        public static bool operator !=(BaseAction a, BaseAction b) => !(a == b);
    }
}
