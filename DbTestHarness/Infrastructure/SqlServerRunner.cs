using DbTestHarness.Models;
using Microsoft.Data.SqlClient;

namespace DbTestHarness.Infrastructure;

public class SqlServerRunner : IRunner
{
    public async Task<RunResult> Execute(Server server)
    {
        if (server is not SqlServer sqlServer)
            throw new InvalidOperationException(
                $"Expected {nameof(SqlServer)} but got {server.GetType().Name}");

        const string template =
            "Data Source={0};TrustServerCertificate=True;Trusted_Connection=Yes;";
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

    public string GetProgressDescription(Server server)
    {
        if (server is not SqlServerWithGroup sqlServer)
            return server.Name;

        return $"[blue]{sqlServer.GroupName}[/] | [yellow]{sqlServer.Name}[/] | {sqlServer.Host}";
    }

    public string GetResultDescription(Server server, RunResult result)
    {
        if (server is not SqlServerWithGroup sqlServer)
            return server.Name;

        var (color, symbol) = result.Succeeded
            ? ("green", "✔")
            : ("red", "✘");

        return $"[{color}]{symbol}[/] [blue]{sqlServer.GroupName}[/] | [yellow]{sqlServer.Name}[/] | {sqlServer.Host}";
    }

    public string GetExceptionDisplayLine(Server server)
    {
        if (server is not SqlServerWithGroup sqlServer)
            return server.Name;

        return $"[blue]{sqlServer.GroupName}[/] | [yellow]{sqlServer.Name}[/] | {sqlServer.Host}";
    }
}
