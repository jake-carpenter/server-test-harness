using CSharpFunctionalExtensions;
using Poke.Config;
using Poke.Runners;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Poke.Commands;

/// <summary>
/// Command for adding an HTTP Server entry via the 'new http' command.
/// </summary>
public class NewHttpCommand(ConfigManager configManager) : AsyncCommand<NewHttpSettings>
{
    public override async Task<int> ExecuteAsync(
        CommandContext context,
        NewHttpSettings settings,
        CancellationToken cancellationToken
    )
    {
        var config = await configManager.Read(settings.ConfigFile);
        var (_, isFailure, server, error) = CreateServer(settings);

        if (isFailure)
        {
            AnsiConsole.MarkupLineInterpolated($"[red]Error:[/] {error}");
            return 1;
        }

        var serversList = config.Servers.ToList();

        serversList.Add(server);
        config = config with { Servers = [.. serversList] };
        await configManager.Save(config, settings.ConfigFile);

        AnsiConsole.MarkupLineInterpolated(
            $"[green]✓[/] Successfully added HTTP Server: [yellow]{server.Uri}[/] / [yellow]{server.Instance}[/] to group [yellow]{server.GroupName}[/]"
        );

        return 0;
    }

    private static Result<HttpServer> CreateServer(NewHttpSettings settings)
    {
        AnsiConsole.MarkupLine("[bold cyan]Add New HTTP Server[/]");
        AnsiConsole.WriteLine();

        var uriString = PromptIfMissing(settings.Uri, "URI", "URI cannot be empty");
        if (!TryValidateAndParseUri(uriString, out var uri))
            return Result.Failure<HttpServer>("Invalid URI format.");

        return new HttpServer
        {
            Id = Guid.NewGuid(),
            GroupName = PromptIfMissing(settings.Group, "Group Name", "Group name cannot be empty"),
            Instance = PromptIfMissing(settings.Instance, "Instance", "Instance cannot be empty"),
            Uri = uri!,
            Insecure = settings.Insecure,
        };
    }

    private static bool TryValidateAndParseUri(string uriString, out Uri? uri)
    {
        if (!Uri.TryCreate(uriString, UriKind.Absolute, out uri))
            return false;

        return uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps;
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
