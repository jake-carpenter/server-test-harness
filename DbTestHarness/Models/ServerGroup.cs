namespace DbTestHarness.Models;

public class ServerGroup
{
    public required string Name { get; init; }
    public required SqlServer[] Servers { get; init; }

    public SqlServerWithGroup[] ServerWithGroups => Servers
        .Select(server => new SqlServerWithGroup(server.Name, server.Host, Name)
        {
            GroupName = Name
        })
        .ToArray();
}

public record SqlServerWithGroup(string Name, string Host, string GroupName) : SqlServer(Name, Host)
{
    public string GetUncoloredStatusLabel()
    {
        return $"{GroupName} | {Name} | {Host}";
    }

    public string GetColoredStatusLabel()
    {
        return $"[blue]{GroupName}[/] | [yellow]{Name}[/] | {Host}";
    }

    public override string GetOptionLabel()
    {
        return $"[white]{Name}[/] [grey]{Host}[/]";
    }
}