using Poke.Commands;
using Poke.Models;
using Poke.Runners;

namespace Poke.Infrastructure;

/// <summary>
/// Selects the appropriate runner for a given server type.
/// </summary>
public class RunnerFactory(IEnumerable<IRunner> runners)
{
    /// <summary>
    /// Gets a runner for the provided server and settings.
    /// </summary>
    /// <param name="server">The server to execute.</param>
    /// <param name="settings">The run settings.</param>
    /// <returns>The matching runner.</returns>
    public IRunner GetRunner(Server server, RunSettings settings)
    {
        IRunner? runner = server switch
        {
            SqlServer => runners.OfType<SqlServerRunner>().FirstOrDefault(),
            HttpServer => runners.OfType<HttpServerRunner>().FirstOrDefault(),
            _ => throw new NotSupportedException(
                $"No runner available for server type: {server.GetType().Name}"
            ),
        };

        if (runner == null)
            throw new InvalidOperationException("No suitable runner found.");

        return runner;
    }
}
