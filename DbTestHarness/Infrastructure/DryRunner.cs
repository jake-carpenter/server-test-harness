using DbTestHarness.Models;

namespace DbTestHarness.Infrastructure;

public class DryRunner : IRunner
{
    public async Task<RunResult> Execute(Server server)
    {
        await Task.Delay(500);

        return RunResult.Success();
    }

    public string GetProgressDescription(Server server)
    {
        return server.Instance;
    }

    public string GetResultDescription(Server server, RunResult result)
    {
        var (color, symbol) = result.Succeeded
            ? ("green", "✔")
            : ("red", "✘");

        return $"[{color}]{symbol}[/] {server.Instance}";
    }

    public string GetExceptionDisplayLine(Server server)
    {
        return server.Instance;
    }
}