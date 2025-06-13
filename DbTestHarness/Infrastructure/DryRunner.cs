using DbTestHarness.Models;

namespace DbTestHarness.Infrastructure;

public class DryRunner : IRunner
{
    public async Task<RunResult> Execute(Server server)
    {
        await Task.Delay(500);

        return RunResult.Success();
    }
}