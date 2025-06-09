using DbTestHarness.Infrastructure;
using DbTestHarness.Models;
using Spectre.Console;
using Spectre.Console.Cli;

namespace DbTestHarness.Commands;

public class SelectCommand(UserConfig userConfig, SqlServerRunner sqlServerRunner, DryRunner dryRunner)
    : AsyncCommand<Settings>
{
    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var options = AnsiConsole.Prompt(BuildPromptWithGroups(userConfig.SqlServerGroups));
        IRunner runner = settings.DryRun ? dryRunner : sqlServerRunner;

        await AnsiConsole.Status().StartAsync(
            "Executing",
            async ctx =>
            {
                foreach (var selectionOption in options.Where(o => o is ServerOption))
                {
                    await Execute(ctx, (ServerOption)selectionOption, runner);
                }
            });

        return 0;
    }

    private static async Task Execute(StatusContext ctx, ServerOption option, IRunner runner)
    {
        ctx.Status(option.Label);
        ctx.Spinner(Spinner.Known.Dots);

        if (await runner.Execute(option.Server!))
        {
            AnsiConsole.MarkupLineInterpolated($"[green]✔[/] [blue]{option.Server!.ToString()}[/]");

            return;
        }

        AnsiConsole.MarkupLineInterpolated($"[red]✘[/] [blue]{option.Server!.ToString()}[/]");
    }

    private static MultiSelectionPrompt<ISelectionOption> BuildPromptWithGroups(
        ServerGroup[] groups)
    {
        var prompt = new MultiSelectionPrompt<ISelectionOption>()
            .Title("Select which servers to test:")
            .PageSize(20)
            .Required()
            .InstructionsText(
                "Press [blue]<space>[/] to select an option\nPress [green]<enter>[/] to accept\nPress [red]<cmd+c>[/] to cancel")
            .HighlightStyle(new Style(Color.Blue, decoration: Decoration.Bold))
            .UseConverter(opt => opt switch
            {
                ServerOption serverOption => $"[grey]{serverOption.Label}[/]",
                GroupOption groupOption => $"[yellow]{groupOption.Label}[/]",
                _ => opt.Label
            });

        foreach (var group in groups)
        {
            prompt.AddChoiceGroup(
                new GroupOption(group.Name),
                group.Servers.Select(s => new ServerOption(s.ServerLabel, s)));
        }

        return prompt;
    }
}