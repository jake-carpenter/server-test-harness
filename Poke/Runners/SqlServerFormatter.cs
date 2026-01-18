using Poke.Infrastructure;
using Poke.Models;

namespace Poke.Runners;

/// <summary>
/// Formats SQL Server status output for the console.
/// </summary>
public class SqlServerFormatter : IRunnerFormatter
{
    /// <summary>
    /// Formats the progress line for a running server.
    /// </summary>
    /// <param name="server">The server being processed.</param>
    /// <returns>The formatted progress line.</returns>
    public string FormatInProgressLine(Server server)
    {
        return $"[blue]{server.GroupName}[/] | [yellow]{server.Instance}[/] | {server.Host}";
    }

    /// <summary>
    /// Formats the result line after execution completes.
    /// </summary>
    /// <param name="server">The server that was processed.</param>
    /// <param name="result">The run result.</param>
    /// <returns>The formatted result line.</returns>
    public string FormatResultLine(Server server, RunResult result)
    {
        var (color, symbol) = result.Succeeded ? ("green", "✔") : ("red", "✘");

        return $"[{color}]{symbol}[/] [blue]{server.GroupName}[/] | [yellow]{server.Instance}[/] | {server.Host}";
    }

    /// <summary>
    /// Formats the server line shown before the exception details.
    /// </summary>
    /// <param name="server">The server that failed.</param>
    /// <returns>The formatted exception line.</returns>
    public string FormatExceptionLine(Server server)
    {
        return $"[blue]{server.GroupName}[/] | [yellow]{server.Instance}[/] | {server.Host}";
    }
}
