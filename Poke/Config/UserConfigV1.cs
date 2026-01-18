using Poke.Models;

namespace Poke.Config;

/// <summary>
/// Version 1 user configuration model.
/// </summary>
public record UserConfigV1
{
    /// <summary>
    /// The configuration version.
    /// </summary>
    public int Version { get; init; } = 1;

    /// <summary>
    /// The configured servers.
    /// </summary>
    public required IReadOnlyCollection<Server> Servers { get; init; }
}
