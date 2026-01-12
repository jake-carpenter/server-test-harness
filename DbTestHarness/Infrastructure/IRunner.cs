using DbTestHarness.Models;

namespace DbTestHarness.Infrastructure;

public interface IRunner
{
    Task<RunResult> Execute(Server server);
    
    /// <summary>
    /// Gets the formatted description for the progress task.
    /// </summary>
    string GetProgressDescription(Server server);
    
    /// <summary>
    /// Gets the formatted description for the result (success or failure) with symbol.
    /// </summary>
    string GetResultDescription(Server server, RunResult result);
    
    /// <summary>
    /// Gets the formatted line for exception summary display.
    /// </summary>
    string GetExceptionDisplayLine(Server server);
}