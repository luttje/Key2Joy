using KeyToJoy.Input;
using Linearstar.Windows.RawInput;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Linearstar.Windows.RawInput.Native;
using KeyToJoy.Mapping;

namespace KeyToJoy
{
    public partial class MouseMoveTriggerOptionsControl : UserControl, ISelectAndSetupTrigger
    {
        public MouseMoveTriggerOptionsControl()
        {
            InitializeComponent();

            cmbMouseDirection.DataSource = Enum.GetNames(typeof(AxisDirection));
        }

        public void Select(BaseTrigger trigger)
        {
            var thisTrigger = (MouseMoveTrigger)trigger;

            cmbMouseDirection.SelectedItem = thisTrigger.AxisBinding;
        }

        public void Setup(BaseTrigger trigger)
        {
            var thisTrigger = (MouseMoveTrigger)trigger;

            thisTrigger.AxisBinding = (AxisDirection)cmbMouseDirection.SelectedItem;
        }
    }
}
