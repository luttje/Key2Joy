using System.Windows.Forms;
using CommandLine;
using Key2Joy.Setup.Installation;

namespace Key2Joy.Setup.Cmd;

[Verb("uninstall", HelpText = "Uninstall Key2Joy")]
internal class UninstallOptions : Options
{
    public override void Handle()
    {
        Program.ShouldStart = false;

        if (!Program.IsElevated)
        {
            Program.Elevate(Parser.Default.FormatCommandLine(new UninstallOptions()));
            return;
        }

        Installer.Uninstall();
        MessageBox.Show("Key2Joy has been uninstalled.", "Uninstall", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
}
