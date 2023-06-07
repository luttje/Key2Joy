using CommandLine.Text;
using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Key2Joy.Setup.Installation;
using Key2Joy.Setup.Releases;
using System.Windows.Forms;

namespace Key2Joy.Setup.Cmd
{
    [Verb("install", HelpText = "Install Key2Joy with the provided settings.")]
    internal class InstallOptions : Options
    {
        [Option(
            shortName: 'p',
            longName: "path",
            Required = true,
            HelpText = "Path where to install Key2Joy."
        )]
        public string InstallPath { get; set; }


        [Option(
            shortName: 'v',
            longName: "version",
            Required = false,
            HelpText = "Version of Key2Joy to install. By default the latest version is installed."
        )]
        public string Version { get; set; }

        [Option(
            shortName: 'u',
            longName: "update",
            Required = false,
            HelpText = "Update preference. By default Key2Joy is updated automatically.",
            Default = UpdatePreference.DownloadAndUpdate
        )]
        public UpdatePreference UpdatePreference { get; set; }

        public override void Handle()
        {
            var releases = ReleaseManager.GetReleases();
            var release = releases.FirstOrDefault(r => r.TagName == Version);

            if (release == null)
            {
                MessageBox.Show($"The version {Version} is not available. Please select a version from the list.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SetupForm.PendingInstallVersion = new InstallVersion
            {
                InstallPath = InstallPath,
                VersionTagName = Version,
                UpdatePreference = UpdatePreference
            };
        }
    }
}
