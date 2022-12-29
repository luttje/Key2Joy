using Key2Joy.Config;
using Key2Joy.Mapping;
using SimWinInput;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Key2Joy.Gui
{
    public partial class InitForm : Form
    {
        public InitForm()
        {
            InitializeComponent();
        }

        private void InitForm_Load(object sender, EventArgs e)
        {
            MappingPreset.ExtractDefaultIfNotExists();
            SimGamePad.Instance.Initialize();
            Program.GoToNextForm(new MainForm());
        }
    }
}
