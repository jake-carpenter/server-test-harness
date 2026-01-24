using System.Text.Json;
using System.Text.Json.Nodes;

namespace Poke.Config.Migrations;

/// <summary>
/// Added ID field to each server.
/// </summary>
public class V3 : IMigration
{
    public int From => 2;
    public int To => 3;

    public JsonDocument Migrate(JsonDocument jsonDocument)
    {
        var jsonNode = JsonNode.Parse(jsonDocument.RootElement.GetRawText());
        if (jsonNode is null)
            throw new InvalidOperationException("Invalid config file");

        // Update version.
        jsonNode["version"] = 3;

        if (jsonNode["servers"] is not JsonArray serversArray)
            return JsonDocument.Parse(jsonNode.ToJsonString());

        // Generate ID field on each server.
        foreach (var server in serversArray.OfType<JsonObject>())
        {
            if (server["id"] is not null)
                continue;

            server["id"] = Guid.NewGuid().ToString();
        }

        return JsonDocument.Parse(jsonNode.ToJsonString());
    }
}
