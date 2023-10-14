using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CommandLine;
using Key2Joy.Setup.Cmd;
using Key2Joy.Setup.Installation;
using Key2Joy.Setup.Releases;

namespace Key2Joy.Setup;

public partial class SetupForm : Form
{
    internal static InstallVersion PendingInstallVersion { get; set; } = null;

    public SetupForm() => this.InitializeComponent();

    private void SetupForm_Load(object sender, EventArgs e)
    {
        var releases = ReleaseManager.GetReleases();

        foreach (var release in releases)
        {
            this.cmbVersions.Items.Add(release.TagName);
        }

        var version = PendingInstallVersion ?? Installer.GetInstalledVersion();

        if (version == null)
        {
            this.txtInstallPath.Text = Installer.GetDefaultInstallPath();
            this.cmbVersions.SelectedIndex = 0;
            this.btnInstallUpdate.Text = "Install";
            return;
        }

        this.txtInstallPath.ReadOnly = true;
        this.btnBrowseInstallPath.Enabled = false;
        this.txtInstallPath.Text = version.InstallPath;
        this.cmbVersions.SelectedIndex = releases.Where(r => r.TagName == version.VersionTagName).Select((r, i) => i).FirstOrDefault();
        this.btnInstallUpdate.Text = "Update";

        switch (version.UpdatePreference)
        {
            case UpdatePreference.DownloadAndUpdate:
                this.rdoUpdateAndInstall.Checked = true;
                break;
            case UpdatePreference.DownloadAndUpdatePreRelease:
                this.rdoUpdateAndInstallPreRelease.Checked = true;
                break;
            case UpdatePreference.DownloadOnly:
                this.rdoDownloadOnly.Checked = true;
                break;
            case UpdatePreference.ManualInstallation:
            default:
                this.rdoManualInstallation.Checked = true;
                break;
        }

        if (PendingInstallVersion != null)
        {
            this.btnInstallUpdate.PerformClick();
        }
    }

    private void BtnBrowseInstallPath_Click(object sender, EventArgs e)
    {
        FolderBrowserDialog folderPicker = new()
        {
            SelectedPath = this.txtInstallPath.Text
        };

        // If the path is invalid, find the nearest parent that exists
        while (!Directory.Exists(folderPicker.SelectedPath))
        {
            folderPicker.SelectedPath = Path.GetDirectoryName(folderPicker.SelectedPath);
        }

        if (folderPicker.ShowDialog() == DialogResult.OK)
        {
            this.txtInstallPath.Text = folderPicker.SelectedPath;
        }
    }

    private async void BtnInstallUpdate_Click(object sender, EventArgs e)
    {
        var installPath = this.txtInstallPath.Text;
        var version = this.cmbVersions.SelectedItem.ToString();
        var updatePreference = UpdatePreference.ManualInstallation;
        var release = ReleaseManager.GetRelease(version);

        if (this.rdoUpdateAndInstall.Checked)
        {
            updatePreference = UpdatePreference.DownloadAndUpdate;
        }
        else if (this.rdoUpdateAndInstallPreRelease.Checked)
        {
            updatePreference = UpdatePreference.DownloadAndUpdatePreRelease;
        }
        else if (this.rdoDownloadOnly.Checked)
        {
            updatePreference = UpdatePreference.DownloadOnly;
        }
        else
        {
            updatePreference = UpdatePreference.ManualInstallation;
        }

        if (release == null)
        {
            MessageBox.Show("The selected version is not available for download.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (!Program.IsElevated)
        {
            Program.Elevate(Parser.Default.FormatCommandLine(new InstallOptions
            {
                InstallPath = installPath,
                Version = version,
                UpdatePreference = updatePreference,
            }));

            Application.Exit();
            return;
        }

        Installer installer = new(release, installPath, updatePreference);
        var finished = false;
        installer.InstallProgressed += (s, ev) => this.BeginInvoke((MethodInvoker)delegate
            {
                this.progressUpdate.Value = ev.ProgressPercentage;

                if (ev.ProgressPercentage >= 100 && !finished)
                {
                    finished = true;
                    this.btnInstallUpdate.Enabled = true;
                    MessageBox.Show("Done downloading and installing!");
                }
            });

        this.progressUpdate.Value = 0;
        this.btnInstallUpdate.Enabled = false;

        try
        {
            await installer.Start();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Download failed. Please check your internet connection and try again. Error was {ex.Message}");
        }
    }
}
