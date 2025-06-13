using DbTestHarness.Infrastructure;
using DbTestHarness.Models;
using Spectre.Console;
using Spectre.Console.Cli;

namespace DbTestHarness.Commands;

public class SelectCommand(UserConfig userConfig, RunnerStatus runnerStatus) : AsyncCommand<Settings>
{
    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var selected = AnsiConsole.Prompt(BuildPromptWithGroups(userConfig.SqlServerGroups));
        var servers = selected.OfType<ServerOption>().Select(x => x.Server).ToArray();
        var result = await runnerStatus.Start("Executing", servers, settings);

        return result;
    }

    private static MultiSelectionPrompt<ISelectionOption> BuildPromptWithGroups(
        ServerGroup[] groups)
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

        foreach (var group in groups)
        {
            var serverOptions = group.ServerWithGroups.Select(s => new ServerOption(s));
            prompt.AddChoiceGroup(new GroupOption(group.Name), serverOptions);
        }

        return prompt;
    }
}