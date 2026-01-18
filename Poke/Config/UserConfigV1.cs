using Poke.Models;

namespace Poke.Config;

public record UserConfigV1
{
    public int Version { get; init; } = 1;
    public required IReadOnlyCollection<Server> Servers { get; init; }
}
