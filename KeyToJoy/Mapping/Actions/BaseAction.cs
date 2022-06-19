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
    internal abstract class BaseAction
    {
        internal static List<BaseAction> All = new List<BaseAction>();

        [JsonProperty]
        public string Name { get; set; }
        
        private string imageResource;

        public BaseAction(string name, string imageResource)
        {
            this.Name = name;
            this.imageResource = imageResource;
        }

        public static BaseAction Register(BaseAction bindableAction)
        {
            All.Add(bindableAction);

            return bindableAction;
        }

        internal abstract void Execute(InputBag inputBag);

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
            return imageResource != null
                ? (Bitmap)Resources.ResourceManager.GetObject(imageResource)
                : null;            
        }

        public static bool operator ==(BaseAction a, BaseAction b)
        {
            if (System.Object.ReferenceEquals(a, b))
                return true;

            if (System.Object.ReferenceEquals(a, null)
                || System.Object.ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }
        public static bool operator !=(BaseAction a, BaseAction b) => !(a == b);
    }
}
