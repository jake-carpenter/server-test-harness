using DbTestHarness.Models;

namespace DbTestHarness.Infrastructure;

public class DryRunner : IRunner
{
    public async Task<bool> Execute(Server server)
    {
        await Task.Delay(500);

        return true;
    }
}