using System.Text.Json;

namespace Poke.Infrastructure;

/// <summary>
/// Customized methods around <see cref="JsonSerializer"/> to ensure consistent serialization options.
/// </summary>
public static class AppJsonSerializer
{
    /// <summary>
    /// The common serializer options.
    /// </summary>
    public static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web);

    /// <summary>
    /// Serializes the value to a <see cref="JsonDocument"/> using common serializer options.
    /// </summary>
    /// <typeparam name="T">The type of the value to serialize.</typeparam>
    /// <param name="value">The value to serialize.</param>
    /// <returns>The <see cref="JsonDocument"/> instance.</returns>
    public static JsonDocument SerializeToDocument<T>(T value)
    {
        return JsonSerializer.SerializeToDocument(value, Options);
    }
}
