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
        internal Image Image
        {
            get
            {
                return ResourceName != null 
                    ? (Bitmap)Resources.ResourceManager.GetObject(ResourceName) 
                    : null;
            }
        }

        internal string DisplayName => ToString();

        [JsonProperty]
        internal string ResourceName;

        public BindableAction(string resourceName)
        {
            this.ResourceName = resourceName;
        }

        internal abstract void PerformPressBind(bool inputKeyDown);
        internal abstract short PerformMoveBind(short inputMouseDelta, short currentAxisDelta);
    }
}
