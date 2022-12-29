﻿using Key2Joy.LowLevelInput;
using Key2Joy.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Key2Joy.Gui.Mapping
{
    public partial class GamePadActionControl : UserControl, IActionOptionsControl
    {
        public event EventHandler OptionsChanged;
        
        public GamePadActionControl()
        {
            InitializeComponent();

            cmbGamePad.DataSource = GamePadAction.GetAllButtons();
            cmbPressState.DataSource = LegacyPressStateConverter.GetPressStatesWithoutLegacy();
            cmbPressState.SelectedIndex = 0;
            cmbGamePadIndex.DataSource = GamePadManager.Instance.GetAllGamePadIndices();
            cmbGamePadIndex.SelectedIndex = 0;
        }

        public void Select(BaseAction action)
        {
            var thisAction = (GamePadAction)action;

            cmbGamePad.SelectedItem = thisAction.Control;
            cmbPressState.SelectedItem = thisAction.PressState;
            cmbGamePadIndex.SelectedItem = thisAction.GamePadIndex;
        }

        public void Setup(BaseAction action)
        {
            var thisAction = (GamePadAction)action;

            thisAction.Control = (SimWinInput.GamePadControl)cmbGamePad.SelectedItem;
            thisAction.PressState = (PressState) cmbPressState.SelectedItem;
            thisAction.GamePadIndex = (int)cmbGamePadIndex.SelectedItem;
        }

        public bool CanMappingSave(BaseAction action)
        {
            return true;
        }

        private void cmbGamePad_SelectedIndexChanged(object sender, EventArgs e)
        {
            OptionsChanged?.Invoke(this, EventArgs.Empty);
        }

        private void cmbPressState_SelectedIndexChanged(object sender, EventArgs e)
        {
            OptionsChanged?.Invoke(this, EventArgs.Empty);
        }

        private void cmbGamePadIndex_SelectedIndexChanged(object sender, EventArgs e)
        {
            OptionsChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}