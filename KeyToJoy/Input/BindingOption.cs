using KeyToJoy.Properties;
using Newtonsoft.Json;
using SimWinInput;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyToJoy.Input
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class BindingOption : ICloneable
    {
        [JsonProperty]
        public GamePadControl Control;
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        public Binding Binding;

        public static Dictionary<GamePadControl, string> ControllerImages = new Dictionary<GamePadControl, string>();

        public string GetControlDisplay()
        {
            return Control.ToString();
        }

        internal object GetBindDisplay()
        {
            return Binding.ToString();
        }

        public object Clone()
        {
            return new BindingOption()
            {
                Binding = (Binding)Binding.Clone(),
                Control = Control,
            };
        }

        internal static void AddControllerImage(GamePadControl control, string resourceName)
        {
            ControllerImages.Add(control, resourceName);
        }

        internal static Image GetControllerImage(GamePadControl control)
        {
            if (!ControllerImages.TryGetValue(control, out var resourceName))
                throw new ArgumentException("This control has no connected image!");
            return (Bitmap)Resources.ResourceManager.GetObject(resourceName);
        }
    }
}
