using Poke.Commands;
using Poke.Models;

namespace Poke.Infrastructure;

/// <summary>
/// An interface for a runner that can execute a specific server type.
/// </summary>
public interface IRunner
{
    /// <summary>
    /// The formatter for the runner.
    /// </summary>
    IRunnerFormatter Formatter { get; }

    /// <summary>
    /// Executes the runner for the given server and settings.
    /// </summary>
    /// <param name="server">The server to execute the runner for.</param>
    /// <param name="settings">The settings for the runner.</param>
    /// <returns>The result of the runner execution.</returns>
    Task<RunResult> Execute(Server server, RunSettings settings);
}
