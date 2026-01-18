using Microsoft.Data.SqlClient;
using Poke.Commands;
using Poke.Infrastructure;
using Poke.Models;

namespace Poke.Runners;

/// <summary>
/// Executes SQL Server connectivity checks.
/// </summary>
public class SqlServerRunner : IRunner
{
    /// <summary>
    /// The formatter for SQL Server output.
    /// </summary>
    public IRunnerFormatter Formatter => new SqlServerFormatter();

    /// <summary>
    /// Executes the SQL Server connectivity check.
    /// </summary>
    /// <param name="server">The server to execute.</param>
    /// <param name="settings">The run settings.</param>
    /// <returns>The run result.</returns>
    public async Task<RunResult> Execute(Server server, RunSettings settings)
    {
        if (server is not SqlServer sqlServer)
            throw new InvalidOperationException(
                $"Expected {nameof(SqlServer)} but got {server.GetType().Name}"
            );

        if (settings.DryRun)
            return RunResult.Success();

        await using var connection = new SqlConnection(sqlServer.ConnectionString);

        try
        {
            await connection.OpenAsync();
            await connection.CloseAsync();

            return RunResult.Success();
        }
        catch (Exception ex)
        {
            return RunResult.Failure(ex);
        }
    }
}
