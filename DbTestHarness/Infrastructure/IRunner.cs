using DbTestHarness.Models;

namespace DbTestHarness.Infrastructure;

public interface IRunner
{
    IRunnerFormatter Formatter { get; }

    Task<RunResult> Execute(Server server, RunSettings settings);
}