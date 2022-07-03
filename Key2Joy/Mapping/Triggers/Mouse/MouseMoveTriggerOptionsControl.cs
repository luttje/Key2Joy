﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Key2Joy.Mapping;

namespace Key2Joy
{
    public partial class MouseMoveTriggerOptionsControl : UserControl, ITriggerOptionsControl
    {
        public event Action OptionsChanged;
        
        public MouseMoveTriggerOptionsControl()
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
            OptionsChanged?.Invoke();
        }
    }
}