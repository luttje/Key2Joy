using KeyToJoy.Input;
using SimWinInput;
using System;
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
            var presets = BindingPreset.LoadAll();

            foreach (var preset in presets)
            {
                BindingPreset.Add(preset, false);
            }

            if(BindingPreset.All.Count == 0)
            {
                var preset = BindingPreset.Default;
                BindingPreset.Add(preset);
            }

            SimGamePad.Instance.Initialize();
            Program.GoToNextForm(new MainForm());
        }
    }
}
