using System.Text.Json;
using Poke.Models;
using Spectre.Console;

namespace Poke.Infrastructure;

public class UserConfigManager
{
    private const string WindowsDirectoryName = "Poke";
    private const string UnixDirectoryName = "poke";
    private const string ConfigFilename = "config.json";

    private readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true,
    };

    /// <summary>
    /// Reads the configuration file, creating it if it doesn't exist.
    /// </summary>
    /// <param name="filePath">The full path to the configuration file. If not provided, the platform-specific default path is used.</param>
    /// <returns>The user configuration</returns>
    public async Task<UserConfig> ReadConfigAsync(string? filePath = null)
    {
        UserConfig? config = null;
        var configFilePath = GetConfigFilePath(filePath);

        if (File.Exists(configFilePath))
        {
            await using var stream = File.OpenRead(configFilePath);
            config = await JsonSerializer.DeserializeAsync<UserConfig>(stream, _jsonOptions);
        }

        if (config is null)
        {
            var directory = Path.GetDirectoryName(configFilePath);
            if (directory == null)
            {
                throw new InvalidOperationException("Invalid config file path");
            }

            AnsiConsole.MarkupLineInterpolated(
                $"[yellow]Warning:[/] Configuration file not found. It will be created at {configFilePath}");

            config = await CreateConfigFileAsync(configFilePath, directory);
        }

        return config;
    }

    /// <summary>
    /// Saves the configuration to the specified file path.
    /// </summary>
    /// <param name="config">The configuration to save</param>
    /// <param name="filePath">The full path to the configuration file. If not provided, the platform-specific default path is used.</param>
    public async Task SaveConfigAsync(UserConfig config, string? filePath = null)
    {
        var configFilePath = GetConfigFilePath(filePath);
        var directory = Path.GetDirectoryName(configFilePath);
        if (directory != null && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var json = JsonSerializer.Serialize(config, _jsonOptions);
        await File.WriteAllTextAsync(configFilePath, json);
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
            {
                return Path.Combine(customConfigPath, ConfigFilename);
            }
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
        var configDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".config");
        return Path.Combine(configDir, UnixDirectoryName);
    }

    private async Task<UserConfig> CreateConfigFileAsync(string filePath, string directory)
    {
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var cfg = new UserConfig { Servers = [] };
        var json = JsonSerializer.Serialize(cfg, _jsonOptions);
        await File.WriteAllTextAsync(filePath, json);

        return cfg;
    }
}
