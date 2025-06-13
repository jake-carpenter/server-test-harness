namespace DbTestHarness.Models;

public interface ISelectionOption
{
    public string Label { get; }
}

public record ServerOption(Server Server) : ISelectionOption
{
    public string Label => Server.GetOptionLabel();
}

public record GroupOption(string Label) : ISelectionOption;

