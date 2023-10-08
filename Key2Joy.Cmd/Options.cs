using System;

namespace Key2Joy.Cmd
{
    internal abstract class Options
    {
        const int MAX_RETRIES = 1;

        abstract public void Handle();

        protected int retries = 0;

        protected void SafelyRetry(Action value)
        {
            if (++retries > MAX_RETRIES)
                return;

            value();
        }
    }
}
