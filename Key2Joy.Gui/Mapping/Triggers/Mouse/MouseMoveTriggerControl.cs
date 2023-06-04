using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Key2Joy.Mapping;

namespace Key2Joy.Gui.Mapping
{
    [MappingControl(
        ForType = typeof(Key2Joy.Mapping.MouseMoveTrigger),
        ImageResourceName = "mouse"
    )]
    public partial class MouseMoveTriggerControl : UserControl, ITriggerOptionsControl
    {
        public event EventHandler OptionsChanged;
        
        public MouseMoveTriggerControl()
        {
            InitializeComponent();

            var directions = new List<AxisDirection>();
                
            foreach(AxisDirection direction in Enum.GetValues(typeof(AxisDirection)))
            {
                if (direction != AxisDirection.None)
                    directions.Add((AxisDirection)direction);
            }

            cmbMouseDirection.DataSource = directions;
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

        private void cmbMouseDirection_SelectedIndexChanged(object sender, EventArgs e)
        {
            OptionsChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
