using DbTestHarness.Models;
using Microsoft.Data.SqlClient;

namespace DbTestHarness.Infrastructure;

public class SqlServerRunner : IRunner
{
    public async Task<bool> Execute(Server server)
    {
        if (server is not SqlServer sqlServer)
            throw new InvalidOperationException(
                $"Expected {nameof(SqlServer)} but got {server.GetType().Name}");

        const string template =
            "Data Source={0};TrustServerCertificate=True;Trusted_Connection=Yes";
        var connectionString = string.Format(template, sqlServer.Host);
        await using var connection = new SqlConnection(connectionString);

        try
        {
            await connection.OpenAsync();
            await connection.CloseAsync();

            return true;
        }
        catch
        {
            return false;
        }
    }
}