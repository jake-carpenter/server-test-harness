using Microsoft.Data.SqlClient;
using Poke.Config;
using Poke.Runners;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Poke.Commands;

/// <summary>
/// Command for adding a SQL Server entry via the 'new sqlserver' command.
/// </summary>
public class NewSqlServerCommand(ConfigManager configManager, SqlServerFormatter formatter)
    : AsyncCommand<NewSqlServerSettings>
{
    private const string InputModeConnectionString = "Connection String";
    private const string InputModeIndividualDetails = "Individual Details (Data Source)";

    public override async Task<int> ExecuteAsync(
        CommandContext context,
        NewSqlServerSettings settings,
        CancellationToken cancellationToken
    )
    {
        var config = await configManager.Read(settings.ConfigFile);
        var newServer = CreateServer(settings);
        var serversList = config.Servers.ToList();

        serversList.Add(newServer);
        config = config with { Servers = [.. serversList] };
        await configManager.Save(config, settings.ConfigFile);

        AnsiConsole.MarkupLine(formatter.FormatCreated(newServer));

        return 0;
    }

    private static SqlServer CreateServer(NewSqlServerSettings settings)
    {
        // Connection string takes precedence if provided
        if (!string.IsNullOrWhiteSpace(settings.ConnectionString))
            return CreateFromConnectionString(settings);

        // If data source is provided, use individual details mode
        if (!string.IsNullOrWhiteSpace(settings.DataSource))
            return CreateFromIndividualDetails(settings);

        // No connection string or data source provided - prompt for input mode
        var inputMode = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[bold cyan]How would you like to configure the SQL Server?[/]")
                .AddChoices(InputModeConnectionString, InputModeIndividualDetails)
        );

        return inputMode == InputModeConnectionString
            ? CreateFromConnectionString(settings)
            : CreateFromIndividualDetails(settings);
    }

    private static SqlServer CreateFromConnectionString(NewSqlServerSettings settings)
    {
        var connectionString = settings.ConnectionString;

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            connectionString = AnsiConsole.Prompt(
                new TextPrompt<string>("[green]Connection String:[/]")
                    .PromptStyle("yellow")
                    .Validate(input =>
                        ValidateConnectionString(input)
                            ? ValidationResult.Success()
                            : ValidationResult.Error("[red]Invalid connection string format[/]")
                    )
            );
        }
        else
        {
            // Validate the provided connection string
            if (!ValidateConnectionString(connectionString))
            {
                AnsiConsole.MarkupLine("[red]Error:[/] Invalid connection string format.");
                throw new InvalidOperationException("Invalid connection string provided.");
            }
        }

        var builder = new SqlConnectionStringBuilder(connectionString);

        return new SqlServer
        {
            GroupName = PromptIfMissing(settings.Group, "Group Name", "Group name cannot be empty"),
            Instance = PromptIfMissing(settings.Instance, "Instance", "Instance cannot be empty"),
            ConnectionString = builder.ConnectionString,
        };
    }

    private static SqlServer CreateFromIndividualDetails(NewSqlServerSettings settings)
    {
        AnsiConsole.MarkupLine("[bold cyan]Add New SQL Server[/]");
        AnsiConsole.WriteLine();

        var dataSource = PromptIfMissing(
            settings.DataSource,
            "Data Source",
            "Data source cannot be empty"
        );

        var builder = new SqlConnectionStringBuilder
        {
            DataSource = dataSource,
            TrustServerCertificate = true,
            IntegratedSecurity = true,
        };

        return new SqlServer
        {
            GroupName = PromptIfMissing(settings.Group, "Group Name", "Group name cannot be empty"),
            Instance = PromptIfMissing(settings.Instance, "Instance", "Instance cannot be empty"),
            ConnectionString = builder.ConnectionString,
        };
    }

    private static bool ValidateConnectionString(string connectionString)
    {
        try
        {
            _ = new SqlConnectionStringBuilder(connectionString);
            return true;
        }
        catch
        {
            return false;
        }
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
