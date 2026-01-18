using Microsoft.Data.SqlClient;
using Poke.Config;
using Poke.Runners;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Poke.Commands;

public class AddCommand(ConfigManager configManager) : AsyncCommand<AddSettings>
{
    public override async Task<int> ExecuteAsync(
        CommandContext context,
        AddSettings settings,
        CancellationToken cancellationToken
    )
    {
        var config = await configManager.Read(settings.ConfigFile);
        var (newServer, host) = PromptForSettings(settings);
        var serversList = config.Servers.ToList();

        serversList.Add(newServer);
        config = config with { Servers = [.. serversList] };
        await configManager.Save(config, settings.ConfigFile);

        AnsiConsole.MarkupLineInterpolated(
            $"[green]✓[/] Successfully added SQL Server: [yellow]{host}[/] / [yellow]{newServer.Instance}[/] to group [yellow]{newServer.GroupName}[/]"
        );

        return 0;
    }

    private static (SqlServer Server, string Host) PromptForSettings(AddSettings settings)
    {
        // Prompt for server details
        AnsiConsole.MarkupLine("[bold cyan]Add New SQL Server[/]");
        AnsiConsole.WriteLine();

        var host = PromptIfMissing(settings.Host, "Host", "Host cannot be empty");
        var builder = new SqlConnectionStringBuilder
        {
            DataSource = host,
            TrustServerCertificate = true,
            IntegratedSecurity = true,
        };

        var server = new SqlServer
        {
            GroupName = PromptIfMissing(settings.Group, "Group Name", "Group name cannot be empty"),
            ConnectionString = builder.ConnectionString,
            Instance = PromptIfMissing(settings.Instance, "Instance", "Instance cannot be empty"),
        };

        return (server, host);
    }

    private static string PromptIfMissing(string? value, string promptLabel, string errorMessage)
    {
        if (!string.IsNullOrWhiteSpace(value))
            return value;

        return AnsiConsole.Prompt(
            new TextPrompt<string>($"[green]{promptLabel}:[/]")
                .PromptStyle("yellow")
                .Validate(input =>
                    !string.IsNullOrWhiteSpace(input)
                        ? ValidationResult.Success()
                        : ValidationResult.Error($"[red]{errorMessage}[/]")
                )
        );
    }
}
