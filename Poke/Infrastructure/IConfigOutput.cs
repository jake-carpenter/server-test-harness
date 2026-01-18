using Poke.Models;

namespace Poke.Infrastructure;

/// <summary>
/// An interface for writing configuration output for a specific server type.
/// </summary>
public interface IConfigOutput
{
    /// <summary>
    /// The type of server the output is for.
    /// </summary>
    string ServerType { get; }

    /// <summary>
    /// Writes the configuration output for the given server groups.
    /// </summary>
    /// <param name="groups">The server groups to write the output for.</param>
    void Write(IEnumerable<ServerGroup> groups);
}
