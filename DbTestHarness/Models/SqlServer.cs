namespace DbTestHarness.Models;

public record SqlServer(string Name, string Host) : Server(Name)
{
    public string ServerLabel => $"{Host} | {Name,-8}";
    public override string ToString() => $"{Host} | {Name,-8}";
}