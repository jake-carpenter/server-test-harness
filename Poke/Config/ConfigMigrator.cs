using System.Text.Json;
using Poke.Config.Migrations;
using Poke.Infrastructure;

namespace Poke.Config;

public class ConfigMigrator(JsonConfigFile configFile, IEnumerable<IMigration> migrators)
{
    public async Task<UserConfig> ExecuteMigrations(string? filePath)
    {
        var currentJson = await configFile.ReadAsJson(filePath);
        var currentVersion = ReadCurrentVersion(currentJson); // Disposable.

        foreach (var migrator in migrators.OrderBy(m => m.From))
        {
            if (migrator.From < currentVersion)
                continue;

            var previousJson = currentJson;
            currentJson = migrator.Migrate(currentJson);
            currentVersion = migrator.To;

            // Dispose the previous json document.
            previousJson.Dispose();
        }

        await configFile.SaveFile(currentJson, filePath);

        var cfg = currentJson.Deserialize<UserConfig>(AppJsonSerializer.Options);

        // Dispose the final json document.
        currentJson.Dispose();

        if (cfg is null)
            throw new Exception("There was an unknown error parsing the saved configuration file.");

        return cfg;
    }

    private static int ReadCurrentVersion(JsonDocument jsonDocument)
    {
        if (!jsonDocument.RootElement.TryGetProperty("version", out var versionProperty))
            return 0;

        if (!versionProperty.TryGetInt32(out var version))
            return 0;

        return version;
    }
}
