using Poke.Infrastructure;
using Poke.Models;

namespace Poke.Runners;

/// <summary>
/// Formats HTTP Server status output for the console.
/// </summary>
public class HttpServerFormatter : IRunnerFormatter
{
    /// <inheritdoc/>
    public string FormatInProgressLine(Server server)
    {
        if (server is not HttpServer httpServer)
            return string.Empty;

        return $"[blue]{server.GroupName}[/] | [yellow]{server.Instance}[/] | {httpServer.Uri}";
    }

    /// <inheritdoc/>
    public string FormatResultLine(Server server, RunResult result)
    {
        var (color, symbol) = result.Succeeded ? ("green", "✔") : ("red", "✘");
        if (server is not HttpServer httpServer)
            return string.Empty;

        return $"[{color}]{symbol}[/] [blue]{server.GroupName}[/] | [yellow]{server.Instance}[/] | {httpServer.Uri}";
    }

    /// <inheritdoc/>
    public string FormatExceptionLine(Server server)
    {
        if (server is not HttpServer httpServer)
            return string.Empty;

        return $"[blue]{server.GroupName}[/] | [yellow]{server.Instance}[/] | {httpServer.Uri}";
    }

    /// <inheritdoc/>
    public string FormatCreated(Server server)
    {
        if (server is not HttpServer httpServer)
            return string.Empty;

        return $"[green]✓[/] Successfully added HTTP Server: [yellow]{httpServer.Uri}[/] / [yellow]{server.Instance}[/] to group [yellow]{server.GroupName}[/]";
    }
}
