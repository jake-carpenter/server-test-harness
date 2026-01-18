using System.Text.Json;
using Poke.Infrastructure;

namespace Poke.Config.Migrations;

public class V1 : IMigration
{
    public int From => 0;
    public int To => 1;

    public JsonDocument Migrate(JsonDocument jsonDocument)
    {
        var config = jsonDocument.Deserialize<UserConfigV0>(AppJsonSerializer.Options);
        if (config is null)
            throw new InvalidOperationException("Invalid config file");

        var newConfig = new UserConfigV1 { Version = 1, Servers = config.Servers };
        return AppJsonSerializer.SerializeToDocument(newConfig);
    }
}
