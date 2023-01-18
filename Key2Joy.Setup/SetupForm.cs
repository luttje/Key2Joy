using CommandLine;
using Key2Joy.Setup.Installation;
using Key2Joy.Setup.Releases;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using Key2Joy.Setup.Cmd;

namespace Key2Joy.Setup
{
    public partial class SetupForm : Form
    {
        internal static InstallVersion PendingInstallVersion { get; set; } = null;

        public SetupForm()
        {
            InitializeComponent();
        }

        private void SetupForm_Load(object sender, EventArgs e)
        {
            var releases = ReleaseManager.GetReleases();

            foreach (var release in releases)
                cmbVersions.Items.Add(release.TagName);

            var version = PendingInstallVersion != null ? PendingInstallVersion : Installer.GetInstalledVersion();

            if(version == null)
            {
                txtInstallPath.Text = Installer.GetDefaultInstallPath();
                cmbVersions.SelectedIndex = 0;
                btnInstallUpdate.Text = "Install";
                return;
            }
            
            txtInstallPath.ReadOnly = true;
            btnBrowseInstallPath.Enabled = false;
            txtInstallPath.Text = version.InstallPath;
            cmbVersions.SelectedIndex = releases.Where(r => r.TagName == version.VersionTagName).Select((r, i) => i).FirstOrDefault();
            btnInstallUpdate.Text = "Update";

            switch (version.UpdatePreference)
            {
                case UpdatePreference.DownloadAndUpdate:
                    rdoUpdateAndInstall.Checked = true;
                    break;
                case UpdatePreference.DownloadAndUpdatePreRelease:
                    rdoUpdateAndInstallPreRelease.Checked = true;
                    break;
                case UpdatePreference.DownloadOnly:
                    rdoDownloadOnly.Checked = true;
                    break;
                case UpdatePreference.ManualInstallation:
                default:
                    rdoManualInstallation.Checked = true;
                    break;
            }

            if(PendingInstallVersion != null)
            {
                btnInstallUpdate.PerformClick();
            }
        }

        private void btnBrowseInstallPath_Click(object sender, EventArgs e)
        {
            var folderPicker = new FolderBrowserDialog();
            folderPicker.SelectedPath = txtInstallPath.Text;

            // If the path is invalid, find the nearest parent that exists
            while (!Directory.Exists(folderPicker.SelectedPath))
                folderPicker.SelectedPath = Path.GetDirectoryName(folderPicker.SelectedPath);

            if (folderPicker.ShowDialog() == DialogResult.OK)
                txtInstallPath.Text = folderPicker.SelectedPath;
        }

        private async void btnInstallUpdate_Click(object sender, EventArgs e)
        {
            var installPath = txtInstallPath.Text;
            var version = cmbVersions.SelectedItem.ToString();
            var updatePreference = UpdatePreference.ManualInstallation;
            var release = ReleaseManager.GetRelease(version);

            if (rdoUpdateAndInstall.Checked)
                updatePreference = UpdatePreference.DownloadAndUpdate;
            else if (rdoUpdateAndInstallPreRelease.Checked)
                updatePreference = UpdatePreference.DownloadAndUpdatePreRelease;
            else if (rdoDownloadOnly.Checked)
                updatePreference = UpdatePreference.DownloadOnly;
            else
                updatePreference = UpdatePreference.ManualInstallation;

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

            var installer = new Installer(release, installPath, updatePreference);
            bool finished = false;
            installer.InstallProgressed += (s, ev) =>
            {
                BeginInvoke((MethodInvoker)delegate {
                    progressUpdate.Value = ev.ProgressPercentage;

                    if (ev.ProgressPercentage >= 100 && !finished)
                    {
                        finished = true;
                        btnInstallUpdate.Enabled = true;
                        MessageBox.Show("Done downloading and installing!");
                    }
                });
            };

            progressUpdate.Value = 0;
            btnInstallUpdate.Enabled = false;

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
}
