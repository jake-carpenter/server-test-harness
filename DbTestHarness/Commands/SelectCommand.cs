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
        var groups = userConfig.Servers.GroupBy(s => s.Name);
        var options = AnsiConsole.Prompt(BuildPromptWithGroups(groups));
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

        if (await runner.Execute(option.Server!.Value))
        {
            AnsiConsole.MarkupLineInterpolated($"[green]✔[/] [blue]{option.Server.ToString()}[/]");

            return;
        }

        AnsiConsole.MarkupLineInterpolated($"[red]✘[/] [blue]{option.Server.ToString()}[/]");
    }

    private static MultiSelectionPrompt<ISelectionOption> BuildPromptWithGroups(
        IEnumerable<IGrouping<string, Server>> groups)
    {
        var prompt = new MultiSelectionPrompt<ISelectionOption>()
            .Title("Select which servers to test:")
            .PageSize(20)
            .InstructionsText(
                "Press [blue]<space>[/] to toggle a server, [green]<enter>[/] to accept")
            .UseConverter(opt => opt.Label);

        foreach (var group in groups)
        {
            prompt.AddChoiceGroup(
                new GroupOption(group.Key),
                group.Select(s => new ServerOption(s.ServerLabel, s)));
        }

        return prompt;
    }
}