namespace DbTestHarness.Models;

public record SqlServer(string Name, string Host) : Server(Name)
{
    public override string GetStatusLabel(string groupName)
    {
        return $"[blue]{groupName}[/] | [yellow]{Name}[/] | {Host}";
    }

    public override string GetOptionLabel()
    {
        return $"[white]{Name}[/] [grey]{Host}[/]";
    }
}