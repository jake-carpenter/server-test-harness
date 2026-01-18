using Poke.Models;

namespace Poke.Infrastructure;

/// <summary>
/// Formats runner output for the console.
/// </summary>
public interface IRunnerFormatter
{
    /// <summary>
    /// Formats the line shown before exception details.
    /// </summary>
    /// <param name="server">The server being processed.</param>
    /// <returns>The formatted exception line.</returns>
    string FormatExceptionLine(Server server);

    /// <summary>
    /// Formats the progress line while a server is running.
    /// </summary>
    /// <param name="server">The server being processed.</param>
    /// <returns>The formatted progress line.</returns>
    string FormatInProgressLine(Server server);

    /// <summary>
    /// Formats the result line after a server finishes.
    /// </summary>
    /// <param name="server">The server that was processed.</param>
    /// <param name="result">The run result.</param>
    /// <returns>The formatted result line.</returns>
    string FormatResultLine(Server server, RunResult result);
}
