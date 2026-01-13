using System.Text.Json;

namespace DbTestHarness.Models;

public class UserConfig
{
    public required Server[] Servers { get; init; }

    public static async Task<UserConfig> FromConfigDirectory()
    {
        var home = Environment.GetEnvironmentVariable("HOME");
        var path = Path.Combine(home ?? "./", ".config", "test-harness", "config.json");

        return await Read(path);
    }

    private static async Task<UserConfig> Read(string path)
    {
        UserConfig? config = null;

        if (File.Exists(path))
        {
            var stream = File.OpenRead(path);
            config = await JsonSerializer.DeserializeAsync<UserConfig>(stream, JsonSerializerOptions.Web);
        }

        if (config is null)
            throw new FileNotFoundException($"Configuration file not found: {path}");

        return config;
    }
}