using System.Diagnostics;

namespace MipsSimulator;

/// <summary>
/// Provides many useful extension methods
/// </summary>
public static class Extensions
{

    /// <summary>
    /// Awaits a ValueTask and returns false if it was canceled 
    /// without throwing <see cref="OperationCanceledException"/>
    /// </summary>
    /// <param name="valueTask"></param>
    /// <returns>The original value or false if it was cancelled</returns>
    public static async ValueTask<bool> NoThrow(this ValueTask<bool> valueTask)
    {
        try
        {
            bool result = await valueTask;
            return result;
        }
        catch (OperationCanceledException)
        {
            return false;
        }
    }

    /// <summary>
    /// Awaits a generic task and returns the original value or
    /// null if it was cancelled without throwing <see cref="OperationCanceledException"/>
    /// </summary>
    /// <param name="valueTask">The task to await</param>
    /// <returns>The original value or null if it was cancelled</returns>
    public static async Task<T?> NoThrow<T>(this Task<T> valueTask)
    {
        try
        {
            T result = await valueTask;
            return result;
        }
        catch (OperationCanceledException)
        {
            return default;
        }
    }

    /// <summary>
    /// Blocks while condition is true or timeout occurs.
    /// </summary>
    /// <param name="condition">The condition that will perpetuate the block.</param>
    /// <param name="frequency">The frequency at which the condition will be check, in milliseconds.</param>
    /// <param name="timeout">Timeout in milliseconds.</param>
    /// <exception cref="TimeoutException"></exception>
    [DebuggerHidden]
    public static async Task WaitWhile(Func<bool> condition, int frequency = 25, int timeout = -1)
    {
        var waitTask = Task.Run(async () =>
        {
            while (condition()) await Task.Delay(frequency);
        });

        if (waitTask != await Task.WhenAny(waitTask, Task.Delay(timeout)))
            throw new TimeoutException();
    }

    /// <summary>
    /// Blocks until condition is true or timeout occurs.
    /// </summary>
    /// <param name="condition">The break condition.</param>
    /// <param name="frequency">The frequency at which the condition will be checked.</param>
    /// <param name="timeout">The timeout in milliseconds.</param>
    [DebuggerHidden]
    public static async Task WaitUntil(Func<bool> condition, int frequency = 25, int timeout = -1)
    {
        var waitTask = Task.Run(async () =>
        {
            while (!condition()) await Task.Delay(frequency);
        });

        if (waitTask != await Task.WhenAny(waitTask,
                Task.Delay(timeout)))
            throw new TimeoutException();
    }
}
