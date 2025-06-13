namespace DbTestHarness.Models;

public abstract record Server(string Name)
{
    public abstract string GetStatusLabel(string groupName);
    public abstract string GetOptionLabel();
}
