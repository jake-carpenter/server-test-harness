using DbTestHarness.Infrastructure;
using DbTestHarness.Models;
using Spectre.Console.Cli;

namespace DbTestHarness.Commands;

public class AllCommand(UserConfig userConfig, RunnerStatus runnerStatus) : AsyncCommand<Settings>
{
    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        var result = await runnerStatus.Start(userConfig.Servers, settings);

        return result;
    }
}