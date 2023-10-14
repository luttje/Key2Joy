using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Key2Joy.Contracts.Util;

public static class RetryHelper
{
    public static void RetryOnException(Action action, TimeSpan retryInterval, int maxRetries = -1, params Type[] exceptionTypes)
    {
        var retries = 0;

        while (maxRetries == -1 || retries < maxRetries)
        {
            try
            {
                action();
                break;
            }
            catch (Exception ex) when (exceptionTypes.Contains(ex.GetType()))
            {
                retries++;

                if (retries < maxRetries)
                {
                    Debug.WriteLine($"RetryHelper failed to run action, retrying {retries} / {maxRetries}");
                    Task.Delay(retryInterval);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
