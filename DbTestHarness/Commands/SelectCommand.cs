using DbTestHarness.Infrastructure;
using DbTestHarness.Models;
using Spectre.Console;
using Spectre.Console.Cli;

namespace DbTestHarness.Commands;

public class SelectCommand(UserConfig userConfig, RunnerStatus runnerStatus) : AsyncCommand<RunSettings>
{
    public override async Task<int> ExecuteAsync(CommandContext context, RunSettings settings, CancellationToken cancellationToken)
    {
        var selected = AnsiConsole.Prompt(BuildPromptWithGroups(userConfig.Servers));
        var servers = selected.OfType<ServerOption>().Select(x => x.Server).ToArray();
        var result = await runnerStatus.Start(servers, settings);

        return result;
    }

    private static MultiSelectionPrompt<ISelectionOption> BuildPromptWithGroups(
        Server[] servers)
    {
        const string instructions =
            """
            Press [blue]<space>[/] to select an option
            Press [green]<enter>[/] to accept
            Press [red]<cmd+c>[/] to cancel
            """;

        var prompt = new MultiSelectionPrompt<ISelectionOption>()
            .Title("Select which servers to test:")
            .PageSize(20)
            .Required()
            .InstructionsText(instructions)
            .HighlightStyle(new Style(Color.Blue, decoration: Decoration.Bold))
            .UseConverter(opt => opt switch
            {
                ServerOption serverOption => $"[grey]{serverOption.Label}[/]",
                GroupOption groupOption => $"[yellow]{groupOption.Label}[/]",
                _ => opt.Label
            });

        foreach (var group in servers.GroupBy((s) => s.GroupName))
        {
            var serverOptions = group.Select(s => new ServerOption(s));
            prompt.AddChoiceGroup(new GroupOption(group.Key), serverOptions);
        }

        return prompt;
    }
}
