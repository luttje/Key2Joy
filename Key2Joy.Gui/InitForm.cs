using System;
using System.Windows.Forms;
using Key2Joy.Mapping;
using SimWinInput;

namespace Key2Joy.Gui;

public partial class InitForm : Form
{
    private readonly bool shouldStartMinimized;

    public InitForm(bool shouldStartMinimized = false)
    {
        this.shouldStartMinimized = shouldStartMinimized;

        this.InitializeComponent();

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

        MainForm mainForm = new(this.shouldStartMinimized);
        Program.GoToNextForm(mainForm);
    }
}
