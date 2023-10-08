using Key2Joy.Mapping;
using SimWinInput;
using System;
using System.Windows.Forms;

namespace Key2Joy.Gui
{
    public partial class InitForm : Form
    {
        private bool shouldStartMinimized;

        public InitForm(bool shouldStartMinimized = false)
        {
            this.shouldStartMinimized = shouldStartMinimized;

            InitializeComponent();

            if (shouldStartMinimized)
            {
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
            }
        }

        private void InitForm_Load(object sender, EventArgs e)
        {
            MappingProfile.ExtractDefaultIfNotExists();
            SimGamePad.Instance.Initialize();

            var mainForm = new MainForm(shouldStartMinimized);
            Program.GoToNextForm(mainForm);
        }
    }
}
