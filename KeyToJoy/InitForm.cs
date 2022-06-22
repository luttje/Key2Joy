using KeyToJoy.Input;
using KeyToJoy.Mapping;
using SimWinInput;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace KeyToJoy
{
    public partial class InitForm : Form
    {
        public InitForm()
        {
            InitializeComponent();
        }

        private void InitForm_Load(object sender, EventArgs e)
        {
            // Load presets from documents
            var presets = MappingPreset.LoadAll();

            foreach (var preset in presets)
            {
                MappingPreset.Add(preset, false);
            }

            SimGamePad.Instance.Initialize();
            Program.GoToNextForm(new MainForm());
        }
    }
}
