using DbTestHarness.Models;
using Spectre.Console;

namespace DbTestHarness.Infrastructure;

public class RunnerStatus(RunnerFactory runnerFactory)
{
    public async Task<int> Start(Server[] servers, RunSettings settings)
    {
        if (servers.Length == 0)
        {
            AnsiConsole.MarkupLine("[red]No servers selected.[/]");

            return 1;
        }

        var results = new Dictionary<Server, RunResult>();

        await AnsiConsole.Progress()
            .AutoClear(false)
            .HideCompleted(false)
            .Columns(
                new SpinnerColumn(),
                new TaskDescriptionColumn { Alignment = Justify.Left })
            .StartAsync(async ctx =>
            {
                var progressTasks = ApplyTasks(ctx, servers, settings);
                var tasks = servers
                    .Where(progressTasks.ContainsKey)
                    .Select(server => ExecuteServerAsync(server, progressTasks[server], settings, results));

                await Task.WhenAll(tasks);
            });

        if (settings.Debug)
        {
            DisplayExceptionSummary(results, settings);
        }

        var hasFailure = results.Values.Any(r => !r.Succeeded);

        return hasFailure ? 1 : 0;
    }

    private Dictionary<Server, ProgressTask> ApplyTasks(ProgressContext ctx, Server[] servers, RunSettings settings)
    {
        var progressTasks = new Dictionary<Server, ProgressTask>();
        foreach (var server in servers)
        {
            var runner = runnerFactory.GetRunner(server, settings);
            var description = runner.Formatter.FormatInProgressLine(server);

            progressTasks[server] = ctx.AddTask(description, autoStart: true, maxValue: 1);
        }

        return progressTasks;
    }

    private async Task ExecuteServerAsync(
        Server server,
        ProgressTask progressTask,
        RunSettings settings,
        Dictionary<Server, RunResult> results)
    {
        var runner = runnerFactory.GetRunner(server, settings);
        var result = await runner.Execute(server, settings);

        lock (results)
        {
            results[server] = result;
        }

        progressTask.Description = runner.Formatter.FormatResultLine(server, result);
        progressTask.Value = 1;
        progressTask.StopTask();
    }

    private void DisplayExceptionSummary(Dictionary<Server, RunResult> results, RunSettings settings)
    {
        var exceptions = results
            .Where(kvp => kvp.Value is { Succeeded: false, Exception: not null })
            .ToList();

        if (exceptions.Count == 0)
            return;

        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[yellow]Exception Summary:[/]");
        AnsiConsole.WriteLine();

        foreach (var (server, result) in exceptions)
        {
            var runner = runnerFactory.GetRunner(server, settings);
            var exceptionMessage = result.Exception?.Message ?? "Unknown error";
            var escapedMessage = Markup.Escape(exceptionMessage);
            
            AnsiConsole.MarkupLine(runner.Formatter.FormatExceptionLine(server));
            AnsiConsole.WriteLine();

            var exceptionText = new Markup($"[red]{escapedMessage}[/]");
            var paddedException = new Padder(exceptionText, new Padding(2, 0));
            AnsiConsole.Write(paddedException);
            AnsiConsole.WriteLine("\n");
        }
    }
}