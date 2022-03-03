using SimWinInput;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyToJoy
{
    internal enum AxisDirection
    {
        Up, Right, Down, Left
    }

    internal class BindingSetting
    {
        public Bitmap HighlightImage;
        public GamePadControl Control;
        public Keys? DefaultKeyBind = null;
        //public MouseButtons? DefaultMouseBind = null;
        public AxisDirection? DefaultAxisBind = null;
    }
}
