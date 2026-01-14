namespace Poke.Models;

public record ServerGroup(string GroupName, Server[] Servers)
{
    public string Type => Servers.First().Type;
}