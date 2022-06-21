using KeyToJoy.Input;
using Linearstar.Windows.RawInput;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Linearstar.Windows.RawInput.Native;
using KeyToJoy.Mapping;

namespace KeyToJoy
{
    public partial class MouseMoveTriggerOptionsControl : UserControl
    {
        public MouseMoveTriggerOptionsControl()
        {
            InitializeComponent();

            cmbMouseDirection.DataSource = Enum.GetNames(typeof(AxisDirection));
        }
    }
}
