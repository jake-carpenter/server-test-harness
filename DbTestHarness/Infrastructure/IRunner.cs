using DbTestHarness.Models;

namespace DbTestHarness.Infrastructure;

public interface IRunner
{
    Task<RunResult> Execute(Server server);
}