namespace Poke.Models;

public interface ISelectionOption
{
    public string Label { get; }
}

public record ServerOption(Server Server) : ISelectionOption
{
    public string Label => $"[white]{Server.Instance}[/] [grey]{Server.Host}[/]";
}

public record GroupOption(string Label) : ISelectionOption;

