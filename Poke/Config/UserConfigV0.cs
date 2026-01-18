using Poke.Models;

namespace Poke.Config;

public record UserConfigV0
{
    public required IReadOnlyCollection<Server> Servers { get; init; }
}
