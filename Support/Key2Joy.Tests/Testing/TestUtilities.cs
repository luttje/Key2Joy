using System;
using System.Threading.Tasks;

namespace Key2Joy.Tests.Testing;

internal class TestUtilities
{
    /// <summary>
    /// Waits for a task to complete, or throws an exception if the task does not complete within the specified timeout.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="asyncMethod"></param>
    /// <param name="timeout"></param>
    /// <returns></returns>
    /// <exception cref="TimeoutException"></exception>
    internal static async Task<T> TestAsyncMethodWithTimeout<T>(
        Task<T> testTask,
        TimeSpan timeout
    )
    {
        var timeoutTask = Task.Delay(timeout);

        var completedTask = await Task.WhenAny(testTask, timeoutTask);

        if (completedTask == timeoutTask)
        {
            throw new TimeoutException("Test exceeded timeout.");
        }

        // Ensure the test task completes and propagates any exceptions
        return await testTask;
    }

    /// <summary>
    ///  for a task to complete, or throws an exception if the task does not complete within the specified timeout.
    /// </summary>
    /// <param name="testTask"></param>
    /// <param name="timeout"></param>
    /// <returns></returns>
    /// <exception cref="TimeoutException"></exception>
    internal static async Task TestAsyncMethodWithTimeout(
        Task testTask,
        TimeSpan timeout
    )
    {
        var timeoutTask = Task.Delay(timeout);

        var completedTask = await Task.WhenAny(testTask, timeoutTask);

        if (completedTask == timeoutTask)
        {
            throw new TimeoutException("Test exceeded timeout.");
        }

        // Ensure the test task completes and propagates any exceptions
        await testTask;
    }
}
