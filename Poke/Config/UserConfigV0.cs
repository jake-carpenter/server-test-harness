using Poke.Models;

namespace Poke.Config;

/// <summary>
/// Version 0 user configuration model.
/// </summary>
public record UserConfigV0
{
    /// <summary>
    /// The configured servers.
    /// </summary>
    public required IReadOnlyCollection<Server> Servers { get; init; }
}
