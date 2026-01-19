using System.Text.Json;

namespace Poke.Config;

/// <summary>
/// Interface for managing the JSON configuration file.
/// </summary>
public interface IConfigFile
{
    /// <summary>
    /// Saves the configuration to the specified file path.
    /// </summary>
    /// <param name="jsonDocument">The JSON document to save</param>
    /// <param name="filePath">The full path to the configuration file. If not provided, the platform-specific default path is used.</param>
    Task SaveFile(JsonDocument jsonDocument, string? filePath = null);

    /// <summary>
    /// Ensures the configuration file exists by creating it if it doesn't.
    /// </summary>
    /// <param name="filePath">The full path to the configuration file. If not provided, the platform-specific default path is used.</param>
    /// <param name="create">A function to create the configuration object.</param>
    /// <returns>The task result.</returns>
    Task EnsureExists(string? filePath, Func<UserConfig> create);

    /// <summary>
    /// Reads the configuration file as a <see cref="JsonDocument"/>.
    /// </summary>
    /// <param name="filePath">The full path to the configuration file. If not provided, the platform-specific default path is used.</param>
    /// <returns>The <see cref="JsonDocument"/> instance.</returns>
    Task<JsonDocument> ReadAsJsonDocument(string? filePath);
}
