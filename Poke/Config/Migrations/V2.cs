using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Data.SqlClient;

namespace Poke.Config.Migrations;

/// <summary>
/// Migration from V1 to V2: converts Host property to ConnectionString on Server entries.
/// </summary>
public class V2 : IMigration
{
    public int From => 1;

    public int To => 2;

    public JsonDocument Migrate(JsonDocument jsonDocument)
    {
        var jsonNode = JsonNode.Parse(jsonDocument.RootElement.GetRawText());
        if (jsonNode is null)
            throw new InvalidOperationException("Invalid config file");

        // Update version
        jsonNode["version"] = 2;

        // Transform each server's Host to ConnectionString
        if (jsonNode["servers"] is not JsonArray serversArray)
            return JsonDocument.Parse(jsonNode.ToJsonString());

        foreach (var server in serversArray)
        {
            if (server?["host"] is JsonValue hostValue)
            {
                var host = hostValue.GetValue<string>();
                var builder = new SqlConnectionStringBuilder
                {
                    DataSource = host,
                    TrustServerCertificate = true,
                    IntegratedSecurity = true,
                };

                server["connectionString"] = builder.ConnectionString;
                server.AsObject().Remove("host");
            }
        }

        return JsonDocument.Parse(jsonNode.ToJsonString());
    }
}
