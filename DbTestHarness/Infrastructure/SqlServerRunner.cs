using DbTestHarness.Models;
using Microsoft.Data.SqlClient;

namespace DbTestHarness.Infrastructure;

public class SqlServerRunner : IRunner
{
    public async Task<bool> Execute(Server server)
    {
        const string template =
            "Data Source={0};TrustServerCertificate=True;Trusted_Connection=Yes";
        var connectionString = string.Format(template, server.Host);
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