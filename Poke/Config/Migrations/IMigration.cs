using System.Text.Json;

namespace Poke.Config.Migrations;

public interface IMigration
{
    int From { get; }
    int To { get; }

    JsonDocument Migrate(JsonDocument jsonDocument);
}
