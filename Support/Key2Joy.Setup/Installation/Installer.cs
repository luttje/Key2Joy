using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Key2Joy.Setup.AppReleases;
using Microsoft.Win32;

namespace Key2Joy.Setup.Installation;

internal class Installer
{
    private const string REGISTRY_KEY = "Key2Joy";

    internal event EventHandler<InstallProgressEventArgs> InstallProgressed;

    private readonly Release release;
    private readonly string installPath;
    private readonly UpdatePreference updatePreference;

    internal static string GetDefaultInstallPath()
    {
        var author = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyCompanyAttribute>().Company;
        var product = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyProductAttribute>().Product;

        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), author, product);
    }

    internal static InstallVersion GetInstalledVersion()
    {
        var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + REGISTRY_KEY);

        if (key == null)
        {
            return null;
        }

        var updatePreference = (UpdatePreference)key.GetValue("UpdatePreference", (int)UpdatePreference.ManualInstallation);

        return new InstallVersion
        {
            InstallPath = key.GetValue("InstallLocation").ToString(),
            VersionTagName = key.GetValue("DisplayVersion").ToString(),
            UpdatePreference = updatePreference
        };
    }

    internal static void WipeDirectory(string installPath)
    {
        if (Directory.Exists(installPath))
        {
            Directory.Delete(installPath, true);
        }
    }

    internal static void Uninstall()
    {
        var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + REGISTRY_KEY);
        var installPath = key.GetValue("InstallLocation").ToString();
        key.Close();

        WipeDirectory(installPath);
        Registry.LocalMachine.DeleteSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + REGISTRY_KEY);
    }

    internal Installer(Release release, string installPath, UpdatePreference updatePreference)
    {
        this.release = release;
        this.installPath = installPath;
        this.updatePreference = updatePreference;
    }

    internal async Task Start()
    {
        this.PrepareDirectory();
        await this.DownloadAndUnzip();
        this.RegisterUninstaller();
    }

    private void PrepareDirectory()
    {
        WipeDirectory(this.installPath);

        Directory.CreateDirectory(this.installPath);
    }

    private async Task DownloadAndUnzip()
    {
        var releasePackage = this.release.Assets.First();
        var localDownloadPath = Path.Combine(this.installPath, releasePackage.Name);
        WebClient client = new();
        client.Headers.Add("User-Agent: Other");
        client.DownloadProgressChanged += (s, ev) => InstallProgressed?.Invoke(this, new InstallProgressEventArgs(ev.ProgressPercentage));

        try
        {
            await client.DownloadFileTaskAsync(
                new Uri(releasePackage.BrowserDownloadUrl),
                localDownloadPath
            );
        }
        catch (WebException ex)
        {
            throw new Exception("Download failed", ex);
        }

        using (var zip = ZipFile.OpenRead(localDownloadPath))
        {
            foreach (var entry in zip.Entries)
            {
                var destinationPath = Path.Combine(this.installPath, entry.FullName);

                if (entry.FullName.EndsWith("/"))
                {
                    Directory.CreateDirectory(destinationPath);
                }
                else
                {
                    if (!Directory.Exists(Path.GetDirectoryName(destinationPath)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));
                    }

                    entry.ExtractToFile(destinationPath, true);
                }
            }
        }

        File.Delete(localDownloadPath);
    }

    private void RegisterUninstaller()
    {
        var key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" + REGISTRY_KEY);

        if (!Directory.Exists(this.installPath))
        {
            Directory.CreateDirectory(this.installPath);
        }

        if (this.release == null)
        {
            throw new Exception("The specified release does not exist remotely.");
        }

        key.SetValue("DisplayName", "Key2Joy");
        key.SetValue("DisplayIcon", Path.Combine(this.installPath, "Key2Joy.exe"));
        key.SetValue("DisplayVersion", this.release.TagName);
        key.SetValue("InstallLocation", this.installPath);
        key.SetValue("UpdatePreference", (int)this.updatePreference);

        var installerPath = Assembly.GetExecutingAssembly().Location;
        key.SetValue("UninstallString", $"{installerPath} uninstall");

        key.Close();
    }
}
