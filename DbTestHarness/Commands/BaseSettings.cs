using System.ComponentModel;
using System.Text.Json;
using DbTestHarness.Models;
using Spectre.Console;
using Spectre.Console.Cli;

namespace DbTestHarness.Commands;

public class BaseSettings : CommandSettings
{
    private UserConfig? _userConfig;

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    private const string WindowsDirectoryName = "Server Test Harness";
    private const string UnixDirectoryName = "server-test-harness";
    private const string ConfigFilename = "config.json";

    [CommandOption("-c|--config")]
    [Description("Configuration file to use")]
    public string? ConfigFile { get; init; }

    public UserConfig GetConfig()
    {
        _userConfig ??= ReadConfig(ConfigFile ?? GetPlatformPath()).GetAwaiter().GetResult();

        return _userConfig;
    }

    private static string GetPlatformPath()
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

    private async Task<UserConfig> ReadConfig(string directory)
    {
        UserConfig? config = null;
        var filePath = Path.Combine(directory, ConfigFilename);

        if (File.Exists(filePath))
        {
            var stream = File.OpenRead(filePath);
            config = await JsonSerializer.DeserializeAsync<UserConfig>(stream, _jsonOptions);
        }

        if (config is null)
        {
            AnsiConsole.MarkupLineInterpolated(
                $"[yellow]Warning:[/] Configuration file not found. It will be created at {filePath}");

            config = await CreateConfigFile(directory, filePath);
        }

        return config;
    }

    private async Task<UserConfig> CreateConfigFile(string directory, string filePath)
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