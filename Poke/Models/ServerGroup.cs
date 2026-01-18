namespace Poke.Models;

/// <summary>
/// Represents a group of servers with the same group name.
/// </summary>
/// <param name="GroupName">The group name.</param>
/// <param name="Servers">The servers in the group.</param>
public record ServerGroup(string GroupName, Server[] Servers)
{
    /// <summary>
    /// The server type for the group.
    /// </summary>
    public string Type => Servers.First().Type;
}
