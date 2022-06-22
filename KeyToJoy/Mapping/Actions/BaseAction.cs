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
        : ICloneable
    {
        [JsonProperty]
        internal string Name { get; set; }

        public string ImageResource { get; set; }
        
        protected string description;

        public BaseAction(string name, string description)
        {
            Name = name;
            this.description = description;
        }

        internal abstract Task Execute(InputBag inputBag);
        
        public abstract object Clone();

        internal virtual void OnStartListening()
        { }
        internal virtual void OnStopListening()
        { }

        public virtual string GetNameDisplay()
        {
            return Name;
        }

        public virtual Image GetImage()
        {
            return ImageResource != null
                ? (Bitmap)Resources.ResourceManager.GetObject(ImageResource)
                : null;            
        }

        public override string ToString()
        {
            return GetNameDisplay();
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
