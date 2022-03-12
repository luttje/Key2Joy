using SimWinInput;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyToJoy.Input
{
    internal class BindingSetting
    {
        public Bitmap HighlightImage;
        public GamePadControl Control;
        public Keys? DefaultKeyBind = null;
        //public MouseButtons? DefaultMouseBind = null;
        public AxisDirection? DefaultAxisBind = null;

        public string GetControlDisplay()
        {
            return Control.ToString();
        }

        internal object GetBindDisplay()
        {
            if (DefaultKeyBind != null)
                return DefaultKeyBind.ToString();
            else if (DefaultAxisBind != null)
            {
                var axis = Enum.GetName(typeof(AxisDirection), DefaultAxisBind);

                return $"Mouse {axis}";
            }

            return "Unbound";
        }
    }
}
