using System.Text.Json;

namespace DbTestHarness.Models;

public class UserConfig
{
    public required Server[] Servers { get; init; }

    public static async Task<UserConfig> FromFile(string filePath)
    {
        var path = Path.Combine(Environment.CurrentDirectory, filePath);
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