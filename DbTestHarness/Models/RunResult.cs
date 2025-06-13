namespace DbTestHarness.Models;

public class RunResult
{
    public bool Succeeded { get; }

    private RunResult(bool succeeded)
    {
        Succeeded = succeeded;
    }

    public static RunResult Success() => new(true);
    public static RunResult Failure() => new(false);
}
