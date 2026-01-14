using Poke.Commands;
using Poke.Models;
using Poke.Runners;

namespace Poke.Infrastructure;

public class RunnerFactory(IEnumerable<IRunner> runners)
{
    public IRunner GetRunner(Server server, RunSettings settings)
    {
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