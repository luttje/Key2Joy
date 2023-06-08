namespace Key2Joy.Setup.Installation
{
    internal class InstallProgressEventArgs
    {
        internal InstallProgressEventArgs(int progress)
        {
            ProgressPercentage = progress;
        }

        internal int ProgressPercentage { get; private set; }
    }
}