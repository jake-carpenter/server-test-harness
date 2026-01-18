using Poke.Models;

namespace Poke.Config;

/// <summary>
/// Version 2 user configuration model.
/// Changed Host property to ConnectionString on Server entries.
/// </summary>
public record UserConfigV2
{
    /// <summary>
    /// The configuration version.
    /// </summary>
    public int Version { get; init; } = 2;

    /// <summary>
    /// The configured servers.
    /// </summary>
    public IReadOnlyCollection<Server> Servers { get; init; } = [];
}
