using System;

namespace Key2Joy.Contracts.Util;

/// <summary>
/// Helps get TimeSpans that are a lot longer on GitHub Actions (to prevent test timeouts)
/// </summary>
public static class TimingHelper
{
    public static int Modifier
        => Environment.GetEnvironmentVariable("GITHUB_ACTIONS") == "true" ? 5 : 1;

    public static TimeSpan FromSeconds(double seconds)
        => TimeSpan.FromSeconds(seconds * Modifier);

    public static TimeSpan FromMilliseconds(double milliseconds)
        => TimeSpan.FromMilliseconds(milliseconds * Modifier);
}
