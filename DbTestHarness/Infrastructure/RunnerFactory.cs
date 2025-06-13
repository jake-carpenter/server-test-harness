using DbTestHarness.Models;

namespace DbTestHarness.Infrastructure;

public class RunnerFactory(IEnumerable<IRunner> runners)
{
    public IRunner GetRunner(Server server, Settings settings)
    {
        if (settings.DryRun)
        {
            return runners.OfType<DryRunner>().FirstOrDefault()
                   ?? throw new InvalidOperationException("DryRunner not found.");
        }

        IRunner? runner = server switch
        {
            SqlServer => runners.OfType<SqlServerRunner>().FirstOrDefault(),
            _ => throw new NotSupportedException($"No runner available for server type: {server.GetType().Name}")
        };

        if (runner == null)
            throw new InvalidOperationException("No suitable runner found.");

        return runner;
    }
}