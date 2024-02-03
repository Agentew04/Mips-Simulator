namespace MipsSimulator.Ui;

/// <summary>
/// Provides many useful extension methods
/// </summary>
public static class Extensions {

    /// <summary>
    /// Awaits a ValueTask and returns false if it was canceled 
    /// without throwing <see cref="OperationCanceledException"/>
    /// </summary>
    /// <param name="valueTask"></param>
    /// <returns>The original value or false if it was cancelled</returns>
    public static async ValueTask<bool> NoThrow(this ValueTask<bool> valueTask) {
        try {
            bool result = await valueTask;
            return result;
        } catch (OperationCanceledException) {
            return false;
        }
    }

    /// <summary>
    /// Awaits a generic task and returns the original value or
    /// null if it was cancelled without throwing <see cref="OperationCanceledException"/>
    /// </summary>
    /// <param name="valueTask">The task to await</param>
    /// <returns>The original value or null if it was cancelled</returns>
    public static async Task<T?> NoThrow<T>(this Task<T> valueTask) {
        try {
            T result = await valueTask;
            return result;
        } catch (OperationCanceledException) {
            return default;
        }
    }
}
