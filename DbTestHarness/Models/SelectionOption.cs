namespace DbTestHarness.Models;

public interface ISelectionOption
{
    public string Label { get; }
}

public record ServerOption(string Label, Server? Server = null) : ISelectionOption;

public record GroupOption(string Label) : ISelectionOption;

