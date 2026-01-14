namespace Poke.Models;

public class RunResult
{
    public bool Succeeded { get; }
    public Exception? Exception { get; }

    private RunResult(bool succeeded, Exception? exception = null)
    {
        Succeeded = succeeded;
        Exception = exception;
    }

    public static RunResult Success() => new(true);
    public static RunResult Failure(Exception? exception) => new(false, exception);
}
