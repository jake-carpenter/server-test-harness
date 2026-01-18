namespace Poke.Models;

/// <summary>
/// Represents the outcome of running a server connection check.
/// </summary>
public class RunResult
{
    /// <summary>
    /// Whether the run succeeded.
    /// </summary>
    public bool Succeeded { get; }

    /// <summary>
    /// The exception thrown during the run, if any.
    /// </summary>
    public Exception? Exception { get; }

    private RunResult(bool succeeded, Exception? exception = null)
    {
        Succeeded = succeeded;
        Exception = exception;
    }

    /// <summary>
    /// Creates a successful run result.
    /// </summary>
    /// <returns>A successful run result.</returns>
    public static RunResult Success() => new(true);

    /// <summary>
    /// Creates a failed run result.
    /// </summary>
    /// <param name="exception">The exception, if any.</param>
    /// <returns>A failed run result.</returns>
    public static RunResult Failure(Exception? exception) => new(false, exception);
}
