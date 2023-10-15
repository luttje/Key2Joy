using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Key2Joy.Tests.Testing;

[TestClass]
public class TestUtilitiesTests
{
    [TestMethod]
    public async Task TestAsyncMethodWithTimeout_CompletesWithinTimeout_ReturnsResult()
    {
        var expectedResult = 42;
        var task = Task.FromResult(expectedResult);
        var timeout = TimeSpan.FromSeconds(1);

        var result = await TestUtilities.TestAsyncMethodWithTimeout(task, timeout);

        Assert.AreEqual(expectedResult, result);
    }

    [TestMethod]
    [ExpectedException(typeof(TimeoutException))]
    public async Task TestAsyncMethodWithTimeout_ExceedsTimeout_ThrowsTimeoutException()
    {
        var task = Task.Delay(TimeSpan.FromSeconds(2)); // Longer than the timeout
        var timeout = TimeSpan.FromSeconds(1);

        await TestUtilities.TestAsyncMethodWithTimeout(task, timeout);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))] // Replace with the expected exception type
    public async Task TestAsyncMethodWithTimeout_InnerTaskThrowsException_PropagatesException()
    {
        var task = Task.FromException<int>(new InvalidOperationException()); // Replace with your specific exception and type
        var timeout = TimeSpan.FromSeconds(1);

        await TestUtilities.TestAsyncMethodWithTimeout(task, timeout);
    }

    [TestMethod]
    [ExpectedException(typeof(TaskCanceledException))]
    public async Task TestAsyncMethodWithTimeout_InnerTaskCancelled_PropagatesCancellationException()
    {
        var cts = new CancellationTokenSource();
        var task = Task.Delay(TimeSpan.FromSeconds(2), cts.Token);
        var timeout = TimeSpan.FromSeconds(1);

        cts.Cancel(); // Cancel the inner task
        await TestUtilities.TestAsyncMethodWithTimeout(task, timeout);
    }

    [TestMethod]
    public async Task TestAsyncMethodWithTimeout_ZeroTimeout_CompletesImmediately()
    {
        var expectedResult = 42;
        var task = Task.FromResult(expectedResult);
        var timeout = TimeSpan.Zero;

        var result = await TestUtilities.TestAsyncMethodWithTimeout(task, timeout);

        Assert.AreEqual(expectedResult, result);
    }

    [TestMethod]
    public async Task TestAsyncMethodWithTimeout_InnerTaskCompletesBeforeTimeout_DoesNotThrow()
    {
        var expectedResult = 42;
        var task = Task.FromResult(expectedResult);
        var timeout = TimeSpan.FromSeconds(2); // Longer than the inner task duration

        var result = await TestUtilities.TestAsyncMethodWithTimeout(task, timeout);

        Assert.AreEqual(expectedResult, result);
    }
}
