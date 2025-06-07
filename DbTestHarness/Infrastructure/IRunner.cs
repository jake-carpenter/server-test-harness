using DbTestHarness.Models;

namespace DbTestHarness.Infrastructure;

public interface IRunner
{
    Task<bool> Execute(Server server);
}