using System.Linq;
using System.Net;
using System.Text.Json;

namespace Key2Joy.Setup.AppReleases;

internal static class ReleaseManager
{
    private const string RELEASES_URL = "https://api.github.com/repos/luttje/Key2Joy/releases";
    private static Release[] releases = null;

    internal static Release[] GetReleases()
    {
        if (releases != null)
        {
            return releases;
        }

        WebClient client = new();
        client.Headers.Add("User-Agent: Other");
        var response = client.DownloadString(RELEASES_URL);

        return releases = JsonSerializer.Deserialize<Release[]>(response);
    }

    internal static Release GetRelease(string versionTagName) => GetReleases().FirstOrDefault(r => r.TagName == versionTagName);
}
