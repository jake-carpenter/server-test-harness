using System.Text.Json;
using Poke.Config;
using Poke.Infrastructure;

namespace Poke.Tests.Fakes;

/// <summary>
/// A fake implementation of <see cref="IConfigFile"/> for testing purposes.
/// Set <see cref="Config"/> to a non-null value to simulate an existing file.
/// Leave it null to simulate a missing file.
/// </summary>
public class FakeConfigFile : IConfigFile
{
    private JsonDocument? _storedDocument;

    /// <summary>
    /// Gets or sets the configuration to return when reading.
    /// When null, simulates a missing config file.
    /// </summary>
    public UserConfig? Config { get; set; }

    /// <summary>
    /// Gets the last saved JSON document.
    /// </summary>
    public JsonDocument? LastSavedDocument => _storedDocument;

    /// <summary>
    /// Gets the number of times SaveFile was called.
    /// </summary>
    public int SaveFileCallCount { get; private set; }

    /// <summary>
    /// Gets the number of times EnsureExists was called.
    /// </summary>
    public int EnsureExistsCallCount { get; private set; }

    /// <summary>
    /// Gets the number of times ReadAsJsonDocument was called.
    /// </summary>
    public int ReadCallCount { get; private set; }

    /// <summary>
    /// Gets the last file path that was passed to ReadAsJsonDocument.
    /// </summary>
    public string? LastReadFilePath { get; private set; }

    public Task SaveFile(JsonDocument jsonDocument, string? filePath = null)
    {
        SaveFileCallCount++;
        _storedDocument?.Dispose();
        _storedDocument = JsonDocument.Parse(jsonDocument.RootElement.GetRawText());

        return Task.CompletedTask;
    }

    public Task EnsureExists(string? filePath, Func<UserConfig> create)
    {
        EnsureExistsCallCount++;
        Config ??= create();
        return Task.CompletedTask;
    }

    public Task<JsonDocument> ReadAsJsonDocument(string? filePath)
    {
        ReadCallCount++;
        LastReadFilePath = filePath;
        var config = Config ?? UserConfig.CreateEmpty();
        return Task.FromResult(AppJsonSerializer.SerializeToDocument(config));
    }
}
