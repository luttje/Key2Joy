using System;
using Key2Joy.Contracts.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Key2Joy.Tests.Contracts.Util;

[TestClass]
public class RetryHelperTest
{
    [TestMethod]
    public void RetryOnException_RetriesOnException()
    {
        var attempts = 0;

        void action()
        {
            attempts++;
            if (attempts < 3)
            {
                throw new InvalidOperationException("Simulated exception");
            }
        }

        RetryHelper.RetryOnException(action, TimeSpan.FromMilliseconds(100), maxRetries: 3, typeof(InvalidOperationException));

        Assert.AreEqual(3, attempts);
    }

    [TestMethod]
    public void RetryOnException_MaxRetriesReached_ThrowsException()
    {
        var attempts = 0;

        void action()
        {
            attempts++;
            throw new InvalidOperationException("Simulated exception");
        }

        Assert.ThrowsException<InvalidOperationException>(() =>
        {
            RetryHelper.RetryOnException(action, TimeSpan.FromMilliseconds(100), maxRetries: 2, typeof(InvalidOperationException));
        });

        Assert.AreEqual(2, attempts);
    }

    [TestMethod]
    public void RetryOnException_DoesNotRetryOnNonMatchingException()
    {
        var attempts = 0;

        void action()
        {
            attempts++;
            throw new ArgumentException("Simulated exception");
        }

        Assert.ThrowsException<ArgumentException>(() =>
        {
            RetryHelper.RetryOnException(action, TimeSpan.FromMilliseconds(100), maxRetries: 3, typeof(InvalidOperationException));
        });

        Assert.AreEqual(1, attempts);
    }

    [TestMethod]
    public void RetryOnException_NoException_DoesNotRetry()
    {
        var attempts = 0;

        void action() => attempts++;

        RetryHelper.RetryOnException(action, TimeSpan.FromMilliseconds(100), maxRetries: 3, typeof(InvalidOperationException));

        Assert.AreEqual(1, attempts);
    }

    [TestMethod]
    public void RetryOnException_CustomExceptionType()
    {
        var attempts = 0;

        void action()
        {
            attempts++;
            if (attempts < 3)
            {
                throw new CustomException("Simulated custom exception");
            }
        }

        RetryHelper.RetryOnException(action, TimeSpan.FromMilliseconds(100), maxRetries: 3, typeof(CustomException));

        Assert.AreEqual(3, attempts);
    }

    private class CustomException : Exception
    {
        public CustomException(string message) : base(message)
        {
        }
    }
}
