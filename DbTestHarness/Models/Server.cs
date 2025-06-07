namespace DbTestHarness.Models;

public readonly record struct Server(string Environment, string Name, string Instance, string Host)
{
    public string ServerLabel => $"{Host} | {Environment} | {Instance,-8}";
    public override string ToString() => $"{Host} | {Environment} | {Name,-4} | {Instance,-8}";
}