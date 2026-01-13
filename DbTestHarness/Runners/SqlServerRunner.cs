using DbTestHarness.Infrastructure;
using DbTestHarness.Models;
using Microsoft.Data.SqlClient;

namespace DbTestHarness.Runners;

public class SqlServerRunner : IRunner
{
    public IRunnerFormatter Formatter => new SqlServerFormatter();

    public async Task<RunResult> Execute(Server server, RunSettings settings)
    {
        if (server is not SqlServer sqlServer)
            throw new InvalidOperationException($"Expected {nameof(SqlServer)} but got {server.GetType().Name}");

        if (settings.DryRun)
            return RunResult.Success();

        const string template = "Data Source={0};TrustServerCertificate=True;Trusted_Connection=Yes;";
        var connectionString = string.Format(template, sqlServer.Host);
        await using var connection = new SqlConnection(connectionString);

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
