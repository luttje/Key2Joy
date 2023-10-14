namespace Key2Joy.Setup.Installation;

internal class InstallProgressEventArgs
{
    internal InstallProgressEventArgs(int progress) => this.ProgressPercentage = progress;

    internal int ProgressPercentage { get; private set; }
}
