using Poke.Infrastructure;
using Poke.Models;
using Poke.Runners;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Poke.Commands;

public class AddCommand(UserConfigManager configManager) : AsyncCommand<AddSettings>
{
    public override async Task<int> ExecuteAsync(
        CommandContext context,
        AddSettings settings,
        CancellationToken cancellationToken)
    {
        var config = settings.GetConfig();
        var newServer = PromptForSettings(settings);
        var serversList = config.Servers.ToList();

        serversList.Add(newServer);
        config = new UserConfig { Servers = [.. serversList] };
        await configManager.SaveConfigAsync(config, settings.ConfigFile);

        AnsiConsole.MarkupLineInterpolated(
            $"[green]✓[/] Successfully added SQL Server: [yellow]{newServer.Host}[/] / [yellow]{newServer.Instance}[/] to group [yellow]{newServer.GroupName}[/]");

        return 0;
    }

    private static SqlServer PromptForSettings(AddSettings settings)
    {
        // Prompt for server details
        AnsiConsole.MarkupLine("[bold cyan]Add New SQL Server[/]");
        AnsiConsole.WriteLine();

        return new SqlServer
        {
            GroupName = PromptIfMissing(settings.Group, "Group Name", "Group name cannot be empty"),
            Host = PromptIfMissing(settings.Host, "Host", "Host cannot be empty"),
            Instance = PromptIfMissing(settings.Instance, "Instance", "Instance cannot be empty")
        };
    }

    private static string PromptIfMissing(string? value, string promptLabel, string errorMessage)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            return value;
        }

        return AnsiConsole.Prompt(
            new TextPrompt<string>($"[green]{promptLabel}:[/]")
                .PromptStyle("yellow")
                .Validate(input => !string.IsNullOrWhiteSpace(input)
                    ? ValidationResult.Success()
                    : ValidationResult.Error($"[red]{errorMessage}[/]")));
    }
}
