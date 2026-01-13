namespace DbTestHarness.Models;

public class ServerGroups : Dictionary<string, ServerGroup>
{
    public ServerGroups(Server[] servers)
    {
        foreach (var group in servers.GroupBy(s => s.GroupName))
        {
            var orderedServers = group.OrderBy(s => s.Instance).ToArray();
            this[group.Key] = new ServerGroup(group.Key, orderedServers);
        }
    }
}

