using Poke.Infrastructure;

namespace Poke.Config;

public class ConfigManager(JsonConfigFile configFile, ConfigMigrator migrator)
{
    public async Task<UserConfig> Read(string? filePath)
    {
        await configFile.EnsureExists(filePath, UserConfig.CreateEmpty);
        return await migrator.ExecuteMigrations(filePath);
    }

    public async Task Save(UserConfig config, string? filePath)
    {
        var jsonDocument = AppJsonSerializer.SerializeToDocument(config);

        await configFile.SaveFile(jsonDocument, filePath);
    }
}
