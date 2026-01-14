using Poke.Infrastructure;
using Spectre.Console.Cli;

namespace Poke.Commands;

public class AllCommand(RunnerStatus runnerStatus) : AsyncCommand<RunSettings>
{
    public override async Task<int> ExecuteAsync(
        CommandContext context,
        RunSettings settings,
        CancellationToken cancellationToken)
    {
        var config = settings.GetConfig();
        var result = await runnerStatus.Start(config.Servers, settings);

        return result;
    }
}