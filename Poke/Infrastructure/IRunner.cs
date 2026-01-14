using Poke.Commands;
using Poke.Models;

namespace Poke.Infrastructure;

public interface IRunner
{
    IRunnerFormatter Formatter { get; }

    Task<RunResult> Execute(Server server, RunSettings settings);
}