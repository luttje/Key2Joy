using KeyToJoy.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyToJoy.Input
{
    [JsonObject(MemberSerialization.OptIn)]
    internal abstract class BindableAction
    {
        internal static List<BindableAction> All = new List<BindableAction>();

        internal Image Image
        {
            get
            {
                return resourceName != null 
                    ? (Bitmap)Resources.ResourceManager.GetObject(resourceName) 
                    : null;
            }
        }

        internal string DisplayName => ToString();

        private string resourceName;

        public BindableAction(string resourceName)
        {
            this.resourceName = resourceName;
        }

        public static BindableAction Register(BindableAction bindableAction)
        {
            All.Add(bindableAction);

            return bindableAction;
        }

        internal abstract void PerformPressBind(bool inputKeyDown);
        internal abstract short PerformMoveBind(short inputMouseDelta, short currentAxisDelta);


        public static bool operator ==(BindableAction a, BindableAction b)
        {
            if (System.Object.ReferenceEquals(a, b))
                return true;

            if (System.Object.ReferenceEquals(a, null)
                || System.Object.ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }
        public static bool operator !=(BindableAction a, BindableAction b) => !(a == b);
    }
}
