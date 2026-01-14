namespace Poke.Models;

public class ServerGroups : Dictionary<string, ServerGroup>
{
    public ServerGroups(IReadOnlyCollection<Server> servers)
    {
        foreach (var group in servers.GroupBy(s => s.GroupName))
        {
            var orderedServers = group.OrderBy(s => s.Instance).ToArray();
            this[group.Key] = new ServerGroup(group.Key, orderedServers);
        }
    }

    public IEnumerable<ServerGroup> ByType(string type)
    {
        return Values.Where(g => g.Type == type);
    }
}

