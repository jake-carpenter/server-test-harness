using System.Text.Json;

namespace Poke.Infrastructure;

public static class AppJsonSerializer
{
    public static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web);

    public static JsonDocument SerializeToDocument<T>(T value)
    {
        return JsonSerializer.SerializeToDocument(value, Options);
    }
}
