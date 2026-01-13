using DbTestHarness.Models;

namespace DbTestHarness.Infrastructure;

public interface IRunnerFormatter
{
  string FormatExceptionLine(Server server);
  string FormatInProgressLine(Server server);
  string FormatResultLine(Server server, RunResult result);
}