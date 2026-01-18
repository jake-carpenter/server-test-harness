namespace Poke.Models;

/// <summary>
/// Represents a selectable option in the server picker.
/// </summary>
public interface ISelectionOption
{
    /// <summary>
    /// The label displayed for the option.
    /// </summary>
    public string Label { get; }
}

/// <summary>
/// Selection option for an individual server.
/// </summary>
/// <param name="Server">The server for the option.</param>
public record ServerOption(Server Server) : ISelectionOption
{
    /// <summary>
    /// The formatted label shown in the prompt.
    /// </summary>
    public string Label => $"[white]{Server.Instance}[/] [grey]{Server.Host}[/]";
}

/// <summary>
/// Selection option for a server group.
/// </summary>
/// <param name="Label">The label shown for the group.</param>
public record GroupOption(string Label) : ISelectionOption;
