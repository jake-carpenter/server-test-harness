using Poke.Infrastructure;

namespace Poke.Config;

/// <summary>
/// Coordinates reading and writing the user configuration with migrations.
/// </summary>
public class ConfigManager(IConfigFile configFile, ConfigMigrator migrator)
{
    /// <summary>
    /// Reads the configuration from the specified file path, both creating the file
    /// and migrating it to the latest version when necessary.
    /// </summary>
    /// <param name="filePath">The optional user-provided configuration file path. The default path is used if not provided.</param>
    /// <returns>The configuration object.</returns>
    public async Task<UserConfig> Read(string? filePath)
    {
        await configFile.EnsureExists(filePath, UserConfig.CreateEmpty);
        return await migrator.ExecuteMigrations(filePath);
    }

    /// <summary>
    /// Saves the configuration to the specified file path or the default path if not provided.
    /// </summary>
    /// <param name="config">The configuration object to save.</param>
    /// <param name="filePath">The optional user-provided configuration file path. The default path is used if not provided.</param>
    /// <returns>The task result.</returns>
    public async Task Save(UserConfig config, string? filePath)
    {
        var jsonDocument = AppJsonSerializer.SerializeToDocument(config);

        await configFile.SaveFile(jsonDocument, filePath);
    }
}
