namespace Poke.Models;

/// <summary>
/// Groups servers by their group name and provides helper queries.
/// </summary>
public class ServerGroups : Dictionary<string, ServerGroup>
{
    /// <summary>
    /// Creates grouped server entries from a collection.
    /// </summary>
    /// <param name="servers">The servers to group.</param>
    public ServerGroups(IReadOnlyCollection<Server> servers)
    {
        foreach (var group in servers.GroupBy(s => s.GroupName))
        {
            var orderedServers = group.OrderBy(s => s.Instance).ToArray();
            this[group.Key] = new ServerGroup(group.Key, orderedServers);
        }
    }

    /// <summary>
    /// Returns groups matching the specified server type.
    /// </summary>
    /// <param name="type">The server type discriminator.</param>
    /// <returns>The matching server groups.</returns>
    public IEnumerable<ServerGroup> ByType(string type)
    {
        return Values.Where(g => g.Type == type);
    }
}
