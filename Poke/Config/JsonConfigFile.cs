using System.Text.Json;
using Poke.Infrastructure;
using Spectre.Console;

namespace Poke.Config;

/// <summary>
/// A class for managing the JSON configuration file.
/// </summary>
public class JsonConfigFile
{
    private const string WindowsDirectoryName = "Poke";
    private const string UnixDirectoryName = "poke";
    private const string ConfigFilename = "config.json";
    private readonly JsonWriterOptions _jsonWriterOptions = new() { Indented = true };

    /// <summary>
    /// Saves the configuration to the specified file path.
    /// </summary>
    /// <param name="jsonDocument">The JSON document to save</param>
    /// <param name="filePath">The full path to the configuration file. If not provided, the platform-specific default path is used.</param>
    public async Task SaveFile(JsonDocument jsonDocument, string? filePath = null)
    {
        filePath ??= GetConfigFilePath(filePath);
        EnsureDirectoryExists(filePath);
        await WriteFile(filePath, jsonDocument);
    }

    /// <summary>
    /// Ensures the configuration file exists by creating it if it doesn't.
    /// </summary>
    /// <param name="filePath">The full path to the configuration file. If not provided, the platform-specific default path is used.</param>
    /// <param name="create">A function to create the configuration object.</param>
    /// <returns>The task result.</returns>
    public async Task EnsureExists(string? filePath, Func<UserConfig> create)
    {
        filePath ??= GetConfigFilePath();
        if (File.Exists(filePath))
            return;

        AnsiConsole.MarkupLineInterpolated(
            $"[yellow]Warning:[/] Configuration file not found. It will be created at {filePath}"
        );

        EnsureDirectoryExists(filePath);

        var cfg = create();
        using var jsonDocument = AppJsonSerializer.SerializeToDocument(cfg);
        await WriteFile(filePath, jsonDocument);
    }

    /// <summary>
    /// Reads the configuration file as a <see cref="JsonDocument"/>.
    /// </summary>
    /// <param name="filePath">The full path to the configuration file. If not provided, the platform-specific default path is used.</param>
    /// <returns>The <see cref="JsonDocument"/> instance.</returns>
    public async Task<JsonDocument> ReadAsJson(string? filePath)
    {
        filePath ??= GetConfigFilePath();
        await using var stream = File.OpenRead(filePath);

        return await JsonDocument.ParseAsync(stream);
    }

    private void EnsureDirectoryExists(string filePath)
    {
        var directory = Path.GetDirectoryName(filePath);
        if (directory is null)
            throw new InvalidOperationException("Invalid config file path");

        Directory.CreateDirectory(directory);
    }

    private async Task WriteFile(string filePath, JsonDocument jsonDocument)
    {
        try
        {
            // Using `File.Create` to ensure an existing file is overwritten.
            await using var stream = File.Create(filePath);
            await using var writer = new Utf8JsonWriter(stream, _jsonWriterOptions);

            jsonDocument.WriteTo(writer);
            await writer.FlushAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to save configuration file to {filePath}", ex);
        }
    }

    /// <summary>
    /// Gets the configuration file path. If a custom config path is provided, it's used.
    /// Otherwise, the platform-specific default path is returned.
    /// </summary>
    /// <param name="customConfigPath">Optional custom configuration file path or directory</param>
    /// <returns>The full path to the configuration file</returns>
    private static string GetConfigFilePath(string? customConfigPath = null)
    {
        if (!string.IsNullOrEmpty(customConfigPath))
        {
            // If it's a directory, combine with filename
            if (Directory.Exists(customConfigPath) || !Path.HasExtension(customConfigPath))
                return Path.Combine(customConfigPath, ConfigFilename);

            // Otherwise, assume it's a full file path
            return customConfigPath;
        }

        var directory = GetPlatformConfigDirectory();
        return Path.Combine(directory, ConfigFilename);
    }

    /// <summary>
    /// Gets the platform-specific configuration directory path.
    /// </summary>
    /// <returns>The configuration directory path</returns>
    private static string GetPlatformConfigDirectory()
    {
        // Check for XDG_CONFIG_HOME first (Linux/macOS)
        var xdgConfigHome = Environment.GetEnvironmentVariable("XDG_CONFIG_HOME");
        if (!string.IsNullOrEmpty(xdgConfigHome))
        {
            return Path.Combine(xdgConfigHome, UnixDirectoryName);
        }

        if (OperatingSystem.IsWindows())
        {
            // Windows: Use AppData\Roaming
            var winConfigDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return Path.Combine(winConfigDir, WindowsDirectoryName);
        }

        // Linux/macOS: Use ~/.config (XDG Base Directory Specification)
        var configDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".config"
        );

        return Path.Combine(configDir, UnixDirectoryName);
    }
}
