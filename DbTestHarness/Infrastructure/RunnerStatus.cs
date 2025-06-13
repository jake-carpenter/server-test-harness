using DbTestHarness.Models;
using Spectre.Console;

namespace DbTestHarness.Infrastructure;

public class RunnerStatus(RunnerFactory runnerFactory)
{
    public async Task<int> Start(string initialStatus, Server[] servers, Settings settings)
    {
        if (servers.Length == 0)
        {
            AnsiConsole.MarkupLine("[red]No servers selected.[/]");

            return 1;
        }

        return await AnsiConsole.Status().StartAsync(
            initialStatus,
            ctx => IterateServers(ctx, servers, settings)
        );
    }

    private async Task<int> IterateServers(StatusContext ctx, Server[] servers, Settings settings)
    {
        var hasFailure = false;

        foreach (var server in servers)
        {
            if (server is not SqlServerWithGroup sqlServer)
            {
                AnsiConsole.MarkupLine($"[red]Skipping unsupported server type: {server.GetType().Name}[/]");
                continue;
            }

            ctx.Status("Executing");
            ctx.Spinner(Spinner.Known.Dots);

            var runner = runnerFactory.GetRunner(sqlServer, settings);
            var result = await runner.Execute(sqlServer);
            hasFailure |= !result.Succeeded;

            WriteResult(result, sqlServer);
        }

        return hasFailure ? 1 : 0;
    }

    private static void WriteResult(RunResult result, Server server)
    {
        if (server is not SqlServerWithGroup sqlServer)
            return;

        var (color, symbol) = result.Succeeded
            ? (color: "green", symbol: "✔")
            : (color: "red", symbol: "✘");

        var line =
            $"[{color}]{symbol}[/] [blue]{sqlServer.GroupName}[/] | [yellow]{sqlServer.Name}[/] | {sqlServer.Host}";
        AnsiConsole.MarkupLine(line);
    }
}