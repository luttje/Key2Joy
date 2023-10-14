using System;

namespace Key2Joy.Cmd;

internal abstract class Options
{
    private const int MAX_RETRIES = 1;

    public abstract void Handle();

    protected int Retries { get; set; } = 0;

    protected void SafelyRetry(Action value)
    {
        if (++this.Retries > MAX_RETRIES)
        {
            return;
        }

        value();
    }
}
