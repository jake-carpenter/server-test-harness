using Poke.Models;

namespace Poke.Infrastructure;

public interface IRunnerFormatter
{
  string FormatExceptionLine(Server server);
  string FormatInProgressLine(Server server);
  string FormatResultLine(Server server, RunResult result);
}