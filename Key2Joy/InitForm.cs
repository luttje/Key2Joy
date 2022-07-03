using Key2Joy.Mapping;
using SimWinInput;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Key2Joy
{
    public partial class InitForm : Form
    {
        public InitForm()
        {
            InitializeComponent();
        }

        private void InitForm_Load(object sender, EventArgs e)
        {
            Config.Load();
            MappingPreset.ExtractDefaultIfNotExists();
            SimGamePad.Instance.Initialize();
            Program.GoToNextForm(new MainForm());
        }
    }
}
